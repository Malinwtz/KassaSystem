using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystem
{
    internal class Admin
    {
        //NewProduct
        //DeleteProduct
        //ChangeProduct



        public decimal CheckIfPromotionalPrice(DateTime start, DateTime end, int price)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday && DateTime.Now.Hour < 13)
                    return Convert.ToInt32(price * 0.8);
                else return price;
        }
       
        
        public void CreateNewProduct()
        {
                Console.WriteLine("Skapa en ny produkt genom att skriva in:");
                Console.WriteLine("ID");
                Console.WriteLine("Namn");
                Console.WriteLine("kg/st");
                Console.WriteLine("Pris per enhet");
                var productID = Console.ReadLine();
                var productName = Console.ReadLine();
                var productUnit = Console.ReadLine();
                var productPrice = Convert.ToDecimal(Console.ReadLine());
                Products products = new Products(productID, productName, productUnit, productPrice, 0);
                //spara till fil Produkter.txt
        }
        public static int AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("ADMINISTRERINGSMENY");
                Console.WriteLine(" ");
                Console.WriteLine("1. Skapa ny produkt");
                Console.WriteLine("2. Radera produkt");
                Console.WriteLine("3. Ändra produkt");
                Console.WriteLine("0. Avsluta");
                var sel = Convert.ToInt32(Console.ReadLine());
                if (sel >= 0 && sel < 4) return sel;
                Console.WriteLine("Felaktig input");
            }
        }


    }
}
