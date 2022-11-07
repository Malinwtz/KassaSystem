using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystem
{
    internal class ErrorHandling
    {
        public static DateTime TryDate()
        {
            while (true)
            {
                try
                {
                    var date = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.CurrentCulture);
                    return date;
                }
                catch {Console.WriteLine("Felaktig input");}
            }
        }

        public static int TryInt()
        {
            while (true)
            {
                try
                {
                    Int32.TryParse(Console.ReadLine(), out int saldo);
                    if (saldo > 0)
                        return saldo;
                }
                catch { }
            }
        }
        public static decimal TryPrice()
        {
            while (true)
            {
                try
                {
                    var price = Convert.ToDecimal(Console.ReadLine());
                    if (price >= 1)
                    {
                        return price;
                    }
                    Console.WriteLine("Felaktig input");
                }
                catch { Console.WriteLine("Felaktig input"); }
            }
        }
        public static string TryUnit()
        {
            while (true)
            {
                var unit = Console.ReadLine();
                if (unit == "kilopris" || unit == "styckpris") return unit;
                else Console.WriteLine("Felaktig input");
            }
        }
        public static string TryName()
        {
            while (true)
            {
                var name = Console.ReadLine();
                if (name != null && name.Length > 1) return name;
                else Console.WriteLine("Felaktig input");
            }
        }
        public static string TryId()
        {
            while (true)
            {
                try
                {
                    var newId = Convert.ToInt32(Console.ReadLine());
                    if (newId >= 1)
                       return Convert.ToString(newId);
                    else Console.WriteLine("Felaktig input");
                }
                catch { Console.WriteLine("Felaktig input"); }
            }
        }
    }
}
