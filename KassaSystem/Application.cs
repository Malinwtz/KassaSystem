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
            decimal totalPrice = 0;
            decimal partOfTotalPrice = 0;
            Products product;
            while (true)
            {
                var sel =  Menu();

                if (sel == 1)
                {
                    bool register = true;
                    while (register)
                    {   
                       // product = ProductRegistration(allProducts); //returnera lista och ? produkter här

                        var userInput = new string[2];
                        var numberOfProducts = 0;
                        Products product1;
                        Console.WriteLine("Ange produktID och antal (*** *):");
                        while (true)
                        {
                            
                            userInput = Console.ReadLine().Trim().Split(' ');  //userInput[0] är ID och userInput[1] är antal produkter

                            product1 = FindProductFromProductID(allProducts, userInput[0]);    //  product.ProductID = userInput[0];

                            while (true) //kolla om produktID finns //kolla om antal stämmer
                            {

                                if (product1 == null || TryNumberOfProducts(userInput[1]) == false) //produktkod hittas inte || ogiltigt antal produkter
                                {
                                    Console.WriteLine("Felaktig input");
                                    userInput = Console.ReadLine().Trim().Split(' ');
                                }

                                else if (product1 != null || TryNumberOfProducts(userInput[1]) == true)
                                {

                                    Console.WriteLine($"{product1.ProductName}: {product1.ProductPrice}kr {product1.ProductUnit}"); //var price = Convert.ToDecimal(Console.ReadLine());

                                    partOfTotalPrice = CalculateTotalPrice(numberOfProducts, Convert.ToDecimal(product1.ProductPrice));
                                    break;
                                }
                            }
                        }

                        Console.WriteLine("Nu har programmet gått för långt");
                       

                        //PAY
                        Console.WriteLine("Kommando PAY");
                        var pay = Console.ReadLine();   
                        if (pay.ToUpper() == "PAY")
                            Pay(allProducts, totalPrice); //sparar till kvitto

                    }
                    
                  
                }
                if (sel == 2)
                {
                    //Adm
                   
                }
                if (sel == 3)
                    break;

                 Console.WriteLine($"KVITTO {DateTime.Now.ToString("yyy-MM-dd-HH-mm-ss")}");
                 Console.WriteLine($"Total: {totalPrice}");
            }
            Console.WriteLine("Avslutar kassasystem");
        }

        private void Pay(List<Products> products, decimal total)         //spara ner allt till en fil 
        {
            var fileName = DateTime.Now.ToString("yyy-MM-dd") + ".txt"; //all info lägger vi in på samma rad i filen //relativ sökväg skickas in i filename
            var line = "";
            foreach (var product in products)
            {
                line = $"{product.ProductName}:{product.ProductPrice}kr {product.ProductUnit}, {total}kr"; //sparar inskriven data till en stringvariabel 
                Console.WriteLine($"Sparar {line} i fil: {fileName}");
            
                File.AppendAllText(fileName, line + Environment.NewLine);     //lägger till all inskriven data i EN rad sist i filen //environment.newline för att få en ny rad i filen. 
            }
        }
        private int Menu()
        {
            while (true)
            {
                Console.WriteLine("KASSA");
                Console.WriteLine("1.Ny kund");
                Console.WriteLine("2.Administreringsverktyg");
                Console.WriteLine("0.Avsluta");
                var sel = Convert.ToInt32(Console.ReadLine());
                if (sel > 0 && sel < 4) return sel;    //om valt ett menyval - return var
                Console.WriteLine("Felaktig input");   //om valt inte menyval - felaktig input, kör loop igen
            }
        }

 //       private decimal ProductRegistration(List<Products> allProducts) { ]
       

        private bool TryNumberOfProducts(string userInput)
        {
            int numberOfProducts = 0;
            while (true)
            {
                try
                {
                    numberOfProducts = Convert.ToInt32(userInput);
                  //  Int32.TryParse(userInput, out int numberOfProducts);
                    return true;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    userInput = Console.ReadLine();
                }
                return false;
            }
                
            
                
                
                //if (numberOfProducts! >= 0)
                //{
                //    Console.WriteLine("Antal produkter är i fel format");
                //    
                //    continue;

                //}   
                //else
                //{
                //    return numberOfProducts;
                //    break;
                //}
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
                var product = new Products(parts[0], parts[1], Convert.ToInt32(parts[2]), Convert.ToDecimal(parts[3])); //skapar nytt objekt av products där strängarna blir properties
                
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