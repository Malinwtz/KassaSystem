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
                        ProductRegistration(allProducts);
                        break;
                    }
                    //break; avslutar program helt
                }
                if (sel == "2")
                {
                    //Adm
                   
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
        private void ProductRegistration(List<Products> allProducts)
        {
            bool register = true;
            while (true)
            {
                var userInput = new string[2];
                var numberOfProducts = 0;
                Products product1;

                while (true) //kolla om produktID finns
                {
                    
                    Console.WriteLine("Ange produktID och antal (*** *):");
                    userInput = Console.ReadLine().Trim().Split(' ');  //userInput[0] är ID och userInput[1] är antal produkter


                    //       product.ProductID = userInput[0];

                    product1 = FindProductFromProductID(allProducts, userInput[0]); //[0] är produktid och [1] är antal
                    if (product1 == null)
                        Console.WriteLine("Ogiltig produktkod");
                    else
                    {
                        numberOfProducts = TryInputNumberOfProducts(userInput[1]);  //om antal skrivs in med bokstäver blir numberofproducts 0
                        break;
                    }
                }
                
                    var totalPrice = CalculateTotalPrice(numberOfProducts, Convert.ToDecimal(product1.ProductPrice));

                    Console.WriteLine($"{product1.ProductName}: {product1.ProductPrice}kr {product1.ProductUnit}"); //var price = Convert.ToDecimal(Console.ReadLine());

                    var fileName = DateTime.Now.ToString("yyy-MM-dd") + ".txt"; //all info lägger vi in på samma rad i filen //relativ sökväg skickas in i filename
                    var line = $"{product1.ProductName}:{product1.ProductPrice}kr {product1.ProductUnit}, {totalPrice}kr"; //sparar inskriven data till en stringvariabel 
                    Console.WriteLine($"Sparar {line} i fil: {fileName}");

                    File.AppendAllText(fileName, line + Environment.NewLine);     //lägger till all inskriven data i EN rad sist i filen //environment.newline för att få en ny rad i filen. 

                    Console.WriteLine("Tryck på enter för att fortsätta inskrivning eller skriv avbryt för att avbryta inskrivning");
                    var answer = Console.ReadLine().ToLower();
                    if (answer == "avbryt")
                    {  
                        break;
                    }
                   
                
                //Console.WriteLine("Avsluta = 0");
                //var uInput = Console.ReadLine();
                //if (uInput == "0")
                //    break;
            }
        }

        private int TryInputNumberOfProducts(string userInput)
        {
            
            while (true)
            {
               
                Int32.TryParse(userInput, out int numberOfProducts); //gör om antal produkter till int
                
                if (numberOfProducts >= 0)
                {
                    return numberOfProducts;
                    break;
                }   
                else
                {
                    Console.WriteLine("Antal produkter är i fel format");
                    userInput = Console.ReadLine();
                    continue;
                }
                    
            }
            
        }

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
    }
}// parse + int : 300 4 

//adm i en ny mapp
//kvittoklass