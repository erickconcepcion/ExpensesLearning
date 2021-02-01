using System;
using Expenses.Utils;
using Expenses.Data;
namespace Expenses.Consoles
{
    public class ConsolaGastos
    {
        private readonly Persona _consolePersona;
        public ConsolaGastos(Persona persona)
        {
            _consolePersona = persona;
        }
        public void Start()
        {
            char opcion = ' ';
            while (opcion != 'x')
            {
                Console.Clear();
                ListarGastos();
                opcion = Util.CapturarOpcion();
                SeleccionarAccion(opcion);
            }
        }
        private void SeleccionarAccion(char opt)
        {
            switch (opt)
            {
                case 'n':
                    AnadirGastos();
                    break;
                case 'd':
                    EliminarGastos();
                    break;
            }
        }
        private void ListarGastos() {
            Console.WriteLine(_consolePersona.TablaDeGastos());
            MostrarOpciones();
        }
        private void MostrarOpciones() {
            
            Console.WriteLine("\tAgregar Gasto [N]");
            Console.WriteLine("\tEliminar Gasto [D]");
            Console.WriteLine("\tSalir [X]");
        }
        private void AnadirGastos() {
            Console.Clear();
            Console.WriteLine("Agregar gasto");
            do {
                AnadirUnGasto();
            }while (PreguntarOtro());
        }
        private bool PreguntarOtro() {
            Console.Write("Otro? [S/N]: ");
            return Util.GetConsoleChar() == 's';
        }
        private void AnadirUnGasto() {
            Console.WriteLine(string.Empty);
            Console.Write("Descripcion: ");
            var descripcion = Console.ReadLine();
            Console.Write("Monto: ");
            var monto = Util.DecimalPerfectParse(Console.ReadLine());
            _consolePersona.CrearGasto(descripcion, monto);
        }

        private void EliminarGastos() {
            var elegido = GetOrden();
            if (elegido != 0)
            {
                Console.Clear();
                var gastoActual = _consolePersona.GetGasto(elegido);
                _consolePersona.EliminarGasto(gastoActual);
            }
        }

        private int GetOrden()
        {
            Console.Write($"\n\tNumero de orden de Eliminacion: ");
            return ValidNumberOrZero(Console.ReadLine());
        }
        
        private int ValidNumberOrZero(string numero) {
            int num;
            var valid = int.TryParse(numero, out num);
            valid = valid && num <= _consolePersona.Gastos.Count;
            return valid ? num : 0;
        }
    }
}