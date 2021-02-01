using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Expenses.Data
{
    public class GastosFileSaver
    {
        private const string personaFilename = "personas.csv";
        private const string gastoFileName = "gastos.csv";

        private void IfNotCreate() 
        {
            if (!File.Exists(personaFilename))
            {
                var personaFile = File.Create(personaFilename);
                personaFile.Dispose();
            }
            if (!File.Exists(gastoFileName))
            {
                var gastosFile = File.Create(gastoFileName);
                gastosFile.Dispose();
            }
        }
        public void SavePersonas(List<Persona> personas) {
            List<string> personaLines = new List<string>();
            List<string> gastosLines = new List<string>();
            personaLines.Add("NumeroPersona,Nombre");
            gastosLines.Add("Monto,Descripcion,NumeroPersona");
            foreach (var currentPersona in personas)
            {
                personaLines.Add($"{currentPersona.NumeroPersona},{currentPersona.Nombre}");
                foreach (var currentGasto in currentPersona.Gastos)
                {
                    gastosLines.Add($"{currentGasto.Monto},{currentGasto.Descripcion},{currentPersona.NumeroPersona}");
                }
            }
            File.WriteAllLines(personaFilename, personaLines);
            File.WriteAllLines(gastoFileName, gastosLines);
        }
        public List<Persona> ReadPersonas() {
            IfNotCreate();
            var ret = new List<Persona>();
            var dic = new Dictionary<int, Persona>();
            List<string> personaLines = File.ReadAllLines(personaFilename).ToList();
            List<string> gastosLines = File.ReadAllLines(gastoFileName).ToList();
            Persona current;
            for(int i = 1; i < personaLines.Count; i++)
            {
                var split = personaLines[i].Split(",");
                current = new Persona {
                    NumeroPersona = int.Parse(split[0]),
                    Nombre = split[1]
                };
                ret.Add(current);
                dic.Add(current.NumeroPersona, current);
            }
            Gasto currentGasto;
            for(int i = 1; i < gastosLines.Count; i++)
            {
                var split = gastosLines[i].Split(",");
                currentGasto = new Gasto {
                    NumeroPersona = int.Parse(split[2]),
                    Monto = decimal.Parse(split[0]),
                    Descripcion = split[1]
                };
                dic[currentGasto.NumeroPersona].Gastos.Add(currentGasto);
            }
            return ret;
        }
    }
}