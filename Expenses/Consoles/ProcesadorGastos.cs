using System.Collections.Generic;
using System;
using System.Linq;
using Expenses.Utils;
using Expenses.Data;
namespace Expenses.Consoles
{
    public class ProcesadorGastos
    {
        public ProcesadorGastos()
        {
            PersonaData = new PersonaData();
        }
        public PersonaData PersonaData;
        public void Start()
        {
            char opcion = ' ';
            while (opcion != 'x')
            {
                PersonaData.loadPersonas();
                Console.Clear();
                MostrarPersonas();
                opcion = Util.CapturarOpcion();
                SeleccionarAccion(opcion);
            }
        }
        private void MostrarPersonas()
        {
            Console.WriteLine(PersonaData.TablaDePersonas());
            MostrarOpciones();
        }
        private void MostrarOpciones()
        {
            Console.WriteLine("\tCrear Persona [N]");
            Console.WriteLine("\tMostrar Gastos [S]");
            Console.WriteLine("\tActualizar Persona [U]");
            Console.WriteLine("\tEliminar Persona [D]");
            Console.WriteLine("\tSalir [X]");
        }
        private void SeleccionarAccion(char opt)
        {
            switch (opt)
            {
                case 'n':
                    NuevaPersona();
                    break;
                case 's':
                    VerGastos();
                    break;
                case 'u':
                    ActualizarPersona();
                    break;
                case 'd':
                    EliminarPersona();
                    break;
            }
        }
        private void NuevaPersona()
        {
            
            Console.Clear();
            Console.WriteLine("Agregar persona");
            Console.Write("Nombre: ");
            PersonaData.NuevaPersona(Console.ReadLine());
        }
        
        private void VerGastos()
        {
            var elegido = GetOrden("para Gastos");
            if (elegido != 0)
            {
                Console.Clear();
                var visorGastos = new ConsolaGastos(PersonaData.GetPersona(elegido));
                visorGastos.Start();
            }
        }
        private void ActualizarPersona()
        {
            var elegido = GetOrden("Modificacion");
            if (elegido != 0)
            {
                Console.Clear();
                var personaActual = PersonaData.GetPersona(elegido);
                Console.WriteLine("Modificar persona");
                Console.WriteLine($"Nombre: {personaActual.Nombre}");
                Console.Write($"Nombre: ");
                PersonaData.ActualizarPersona(elegido, Console.ReadLine());
            }
        }
        
        private int GetOrden(string accion)
        {
            Console.Write($"\n\tNumero de orden de {accion} ");
            return PersonaData.ValidNumberOrZero(Console.ReadLine());
        }
        private void EliminarPersona()
        {
            var elegido = GetOrden("Eliminacion");
            if (elegido != 0)
            {
                Console.Clear();
                var personaActual = PersonaData.GetPersona(elegido);
                Console.WriteLine("Eliminar Persona");
                Console.WriteLine($"Seguro que desea eliminar a {personaActual.Nombre}? [Y/N]: ");
                if (Util.GetConsoleChar() == 'y')
                {
                    PersonaData.EliminarPersona(elegido);
                }
            }
        }

    }
}