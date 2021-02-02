using System;
using System.Collections.Generic;
using System.Linq;
using Expenses.Utils;

namespace Expenses.Data
{
    public class PersonaData
    {
        public PersonaData()
        {
            _saver = new GastosSqliteSaver();
            Personas = new List<Persona>();
        }
        public void loadPersonas()=> Personas = _saver.ReadPersonas();
        private List<Persona> Personas;
        public List<Persona> GetAllPersonas() => Personas;
        private GastosSqliteSaver _saver;
        public string TablaDePersonas() => Personas.ToStringTable(
            new string[] {"#", "Nombre", "Gasto Total",
            "Gasto Promedio", "Gasto Maximo", "Gasto Minimo"},
            p => Personas.IndexOf(p) + 1,
            p => p.Nombre,
            p => p.GastoTotal,
            p => p.GastoPromedio,
            p => p.GastoMaximo,
            p => p.GastoMinimo);

        private int GetMaxNumPersona() => Personas.Count > 0 ? Personas.Max(p => p.NumeroPersona) + 1 : 1;
        public void NuevaPersona(string name)
        {
            var nuevaPersona = new Persona
            {
                Nombre = name,
            };
            nuevaPersona = _saver.SavePersonas(nuevaPersona);
            Personas.Add(nuevaPersona);
        }
        public Persona GetPersona(int orden) => Personas[orden - 1];

        public int ValidNumberOrZero(string numero)
        {
            int num;
            var valid = int.TryParse(numero, out num);
            valid = valid && num <= Personas.Count;
            return valid ? num : 0;
        }
        public void ActualizarPersona(int elegido, string name)
        {
            var personaActual = GetPersona(elegido);
            ActualizarPersona(personaActual, name);
        }
        public void ActualizarPersona(Persona personaActual, string name)
        {
            personaActual.Nombre = name;
            _saver.SavePersonas(personaActual.NumeroPersona, personaActual);
        }
        public void EliminarPersona(int elegido)
        {
            var personaActual = GetPersona(elegido);
            EliminarPersona(personaActual);
        }
        public void EliminarPersona(Persona personaActual)
        {
            Personas.Remove(personaActual);
            _saver.DeletePersonas(personaActual.NumeroPersona);
        }
    }
}