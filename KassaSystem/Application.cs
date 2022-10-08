using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KassaSystem
{
    public class Application
    {
        public void Run()
        {   
            //var allProducts = new List<Products>(); 
            var allProducts = ReadProductsFromFile();  // använd listan när vi kör //kodmeny nedan kan ligga i egen funktion

            while (true)
            {
                ShowMenu();
                var sel = Console.ReadLine();
                if (sel == "1")
                {
                    bool register = true;
                    while (register)
                    {
                        register = ProductRegistration(allProducts);
                    }
                    //break; avslutar program helt
                }
                if (sel == "2")
                {
                    //Adm
                    //NewProduct
                    //DeleteProduct
                    //ChangeProduct
                    //PromotionalPrice
                }
                if (sel == "0")
                    break;
            }
            Console.WriteLine("Avslutar kassasystem");
        }
        private void ShowMenu()
        {
            Console.WriteLine("KASSA");
            Console.WriteLine("1.Ny kund");
            Console.WriteLine("2.Administreringsverktyg");
            Console.WriteLine("0.Avsluta");
            Console.WriteLine(" ");
            Console.WriteLine("Ange val");
        }
        private bool ProductRegistration(List<Products> allProducts)
        {
            Products prod1; 
            while (true)
            {
                Console.WriteLine("Ange produktID:");
                var productID = Console.ReadLine();
                
                prod1 = FindProductFromProductID(allProducts, productID);
                if (prod1 == null)
                    Console.WriteLine("Ogiltig produktkod");
                else
                {
                    Console.WriteLine($"{prod1.ProductName}: {prod1.ProductPrice}kr {prod1.ProductUnit}"); //var price = Convert.ToDecimal(Console.ReadLine());
                    break;
                }
            }

            Console.WriteLine("Ange antal styck- eller kilo:"); //felhantering 
            Int32.TryParse(Console.ReadLine(), out int numbersOf);

            var totalPrice = CalculateTotalPrice( numbersOf, Convert.ToDecimal(prod1.ProductPrice)); 

            //all info lägger vi in på samma rad i filen nedan
            var fileName = DateTime.Now.ToString("yyy-MM-dd") + ".txt"; //relativ sökväg skickas in i filename
            var line = $"{prod1.ProductName}:{prod1.ProductPrice}kr {prod1.ProductUnit}, {totalPrice}kr"; //sparar inskriven data till en stringvariabel 
            Console.WriteLine($"Sparar {line} i fil: {fileName}");

            //lägg till all inskriven data i EN rad sist i filen
            File.AppendAllText(fileName, line + Environment.NewLine); //environment.newline för att få en ny rad i filen. 

            Console.WriteLine("Tryck på enter för att fortsätta inskrivning eller skriv avbryt för att avbryta inskrivning");
            var answer = Console.ReadLine().ToLower();
            if (answer == "avbryt")
                return false;
            return true;
        }

        private decimal CalculateTotalPrice(int numberOfProducts, decimal price)
        {   
            var totalPrice = price * numberOfProducts;
            return totalPrice;
        }

        //Products prod2; // gör metoder av dessa
        //while (true)
        //{
        //    Console.WriteLine("Ange produktID:"); //300 2
        //    var productID = Console.ReadLine();
        //    prod2 = FindProductFromProductID(allProducts, productID);
        //    if (prod2 == null)
        //        Console.WriteLine("Ogiltig produktkod");
        //    else break;
        //}

        private Products FindProductFromProductID(List<Products> allProducts, string prod)
        {
            foreach (var product in allProducts)
            {
                if (product.ProductID.ToLower() == prod.ToLower())
                    return product;
            }
            return null;
        }

        private List<Products> ReadProductsFromFile()
        {
            var result = new List<Products>();
           foreach(var line in File.ReadLines("Products.txt"))  //när vi ska läsa filer - använd inte streamreaders utan readlines - högre abstraktioner
                                                                //ReadAllLines läser ALLA rader och tar upp ram-minne //ReadLines läser EN rad i taget. använd därför denna.
            {
                var parts = line.Split(';'); //Split tar en sträng (line i det här fallet) och stoppar in stringdelarna i en array.
                var product = new Products(parts[0], parts[1], parts[2], Convert.ToDecimal(parts[3])); //skapar nytt objekt av products där strängarna blir properties
                
                result.Add(product);
            }
                
            //för varje rad - skapa ny objekt av typ prod
            //stoppa in i listan  
            //returnera listan
            return result;
        }
    }
}// parse + int : 300 4 

//adm i en ny mapp
//kvittoklass