using System;
using System.Collections.Generic;
using System.Linq;
using Expenses.Utils;

namespace Expenses.Data
{
    public class Persona
    {
        public Persona()
        {
            Gastos = new List<Gasto>();
            _saver = new GastosSqliteSaver();
        }
        public string Nombre { get; set; }
        public List<Gasto> Gastos { get; set; }
        public int NumeroPersona {get; set;}
        public decimal GastoMaximo => Gastos.Count == 0 ? 0 : Gastos.Max(g => g.Monto);
        public decimal GastoMinimo => Gastos.Count == 0 ? 0 : Gastos.Min(g => g.Monto);
        public decimal GastoTotal => Gastos.Count == 0 ? 0 : Gastos.Sum(g => g.Monto);
        public decimal GastoPromedio => Gastos.Count == 0 ? 0 : Gastos.Average(g => g.Monto);

        GastosSqliteSaver _saver;


        public Gasto GetGasto(int orden) => Gastos[orden - 1];
        public void CrearGasto(string descripcion, decimal gasto)
        {
            var nuevoGasto = new Gasto{
                Descripcion = descripcion,
                Monto = gasto,
                NumeroPersona = NumeroPersona
            };
            _saver.SaveGastos(nuevoGasto);
            Gastos.Add(nuevoGasto);
        }
        public void EliminarGasto(Gasto gastoParaEliminar)
        {
            _saver.DeleteGastos(gastoParaEliminar);
            Gastos.Remove(gastoParaEliminar);
        }

        public string TablaDeGastos()
        {
            return Gastos.ToStringTable(new string[] {"#", "Descripcion", "Gasto"},
            g=> Gastos.IndexOf(g)+1, g => g.Descripcion, g => g.Monto);
        }

    }
}