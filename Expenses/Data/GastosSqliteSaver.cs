using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
namespace Expenses.Data
{
    public class GastosSqliteSaver
    {
        private const string dbName = "Expenses.db";
        private SqliteConnection connection;
        public GastosSqliteSaver()
        {
            connection = new SqliteConnection($"Data Source={dbName}");
        }
        private void InitDbTables()
        {
            ExecuteQuery(@"
                        CREATE TABLE Personas(NumPersona integer PRIMARY KEY AUTOINCREMENT, Nombre varchar(100));
                        CREATE TABLE Gastos(NumPersona integer, Descripcion varchar, Monto real);
                    ");
        }
        private void ExecuteQuery(string commandText)
        {
            connection.Open();
            CreateMyCommand(commandText).ExecuteNonQuery();
            connection.Close();
        }
        private SqliteCommand CreateMyCommand(string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }
        private void IfNotCreate()
        {
            if (!File.Exists(dbName))
            {
                var personaFile = File.Create(dbName);
                personaFile.Dispose();
                InitDbTables();
            }
        }
        private void DeleteAll()
        {
            ExecuteQuery(@"
                delete from Personas;
                delete from Gastos;
            ");
        }
        public Persona SavePersonas(Persona personaInserting)
        {
            string insertPersonasCommand = $"INSERT INTO Personas(Nombre) VALUES('{personaInserting.Nombre}');";
            ExecuteQuery(insertPersonasCommand);
            return ReadSinglePersona(personaInserting.Nombre);
        }
        public void SavePersonas(int personaId, Persona personaInserting)
        {
            string updatePersonasCommand = $"update Personas set Nombre = '{personaInserting.Nombre}' where NumPersona = {personaInserting.NumeroPersona}";
            ExecuteQuery(updatePersonasCommand);
        }
        public void DeletePersonas(int personaId)
        {
            DeleteGastosFromPersona(personaId);
            string deletePersonasCommand = $"delete from Personas where NumPersona = {personaId}";
            ExecuteQuery(deletePersonasCommand);
        }
        public void SaveGastos(Gasto gasto)
        {
            string insertPersonasCommand = $"INSERT INTO Gastos VALUES({gasto.NumeroPersona}, '{gasto.Descripcion}', {gasto.Monto});";
            ExecuteQuery(insertPersonasCommand);
        }
        public void DeleteGastos(Gasto gasto)
        {
            string insertPersonasCommand = $"delete from Gastos where NumPersona = {gasto.NumeroPersona} and Descripcion='{gasto.Descripcion}'";
            ExecuteQuery(insertPersonasCommand);
        }
        public void DeleteGastosFromPersona(int numPersona)
        {
            string insertPersonasCommand = $"delete from Gastos where NumPersona = {numPersona}";
            ExecuteQuery(insertPersonasCommand);
        }

        public Persona ReadSinglePersona(string name)
        {
            Persona ret = null;
            connection.Open();
            using (var reader = CreateMyCommand($"SELECT * FROM Personas where Nombre = '{name}'").ExecuteReader())
            {
                reader.Read();
                ret = new Persona
                {
                    NumeroPersona = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                };
            }
            connection.Close();
            return ret;
        }

        public List<Persona> ReadPersonas()
        {
            IfNotCreate();
            var ret = new List<Persona>();
            int currPersona = 0;
            int prev = 0;
            Persona current = null;
            connection.Open();
            using (var reader = CreateMyCommand("SELECT * FROM Personas p left join Gastos g on p.NumPersona = g.NumPersona;").ExecuteReader())
            {
                while (reader.Read())
                {
                    currPersona = reader.GetInt32(0);
                    if (currPersona != prev)
                    {
                        current = new Persona
                        {
                            NumeroPersona = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                        ret.Add(current);
                    }
                    if (!reader.IsDBNull(3))
                    {
                        current.Gastos.Add(new Gasto
                        {
                            NumeroPersona = reader.GetInt32(2),
                            Descripcion = reader.GetString(3),
                            Monto = reader.GetDecimal(4)
                        });
                    }
                    prev = current.NumeroPersona;
                }
            }
            connection.Close();
            return ret;
        }
    }
}