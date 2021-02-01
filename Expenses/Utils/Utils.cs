using System;

namespace Expenses.Utils
{
    public class Util
    {
        public static char CapturarOpcion()
        {
            Console.Write("\tElija una opcion: ");
            return GetConsoleChar();
        }
        public static char GetConsoleChar() => Console.ReadKey().KeyChar.ToString().ToLower().ToCharArray()[0];

        public static decimal DecimalPerfectParse(string numero) {
            decimal num;
            var valid = decimal.TryParse(numero, out num);
            return valid ? num : DecimalPerfectParse(Console.ReadLine());
        }

    }
}