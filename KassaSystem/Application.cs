using KassaSystem.Models;
using System;
using System.Collections.Generic;
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
            var allProducts = new List<Products>(); //läs in alla produkter till en lista  //skapa products-objekt

            //if(File.Exists("Products.txt")) //skicka in path "products.txt. kan vara bara filnamn - relativ sökväg/path  //läser in filen om den finns till en lista
            allProducts = ReadProductsFromFile();
            // använd listan när vi kör //kodmeny nedan kan ligga i egen funktion

            while (true)
            {
                Console.WriteLine("1.Ny produkt");
                Console.WriteLine("2.Avsluta");
                Console.WriteLine("Ange val");
                var sel = Console.ReadLine();
                if (sel == "0")
                    break;
                if (sel == "1")
                {
                    ProductRegistration(allProducts);
                }
            }
            
        }

        private void ProductRegistration(List<Products> allProducts)
        {
            Products product1; // gör metoder av dessa
            while (true)
            {
                Console.WriteLine("Ange produkt:"); //productname
                var product1 = Console.ReadLine();
                //om valid shortname - break

                FindProductFromProductID(allProducts, p);

              
            }
            
            Console.WriteLine("Ange pris:"); //price
            var price = Convert.ToDecimal(Console.ReadLine());
              //all info ovan lägger vi in på samma rad i filen nedan
            var filename = DateTime.Now.ToString("yyy-MM-dd") + ".txt"; //relativ sökväg skickas in i filename

            var line = $"{productID}; {price}"; //sparar inskriven data till en stringvariabel
   
            //lägg till all inskriven data i EN rad sist i filen
            File.AppendAllText( filename, line + Environment.NewLine); //mata in product1 och price1 i filen filename
            //environment.newline för att få en ny rad i filen. Ungefär som \n
        }

        private Products FindProductFromProductID(List<Products> allProducts, string product)
        {
            foreach (var productID in allProducts)
            {
                if()
            }
            return null;
        }

        private List<Products> ReadProductsFromFile()
        {
            var result = new List<Products>();
            //läs fil rad för rad
           foreach(var line in File.ReadLines("Products.txt"))  //när vi ska läsa filer - använd inte streamreaders utan readlines - högre abstraktioner
                                                                //ReadAllLines läser ALLA rader och tar upp ram-minne //ReadLines läser EN rad i taget. använd därför denna.
            {
                var parts = line.Split(';'); //Split tar en sträng (line i det här fallet) och stoppar in stringdelarna i en array.
                var product = new Products //skapar nytt objekt av products där strängarna blir properties
                {
                    ProductID = parts[0],
                    Name = parts[1],
                    Unit = parts[2],
                    Price = Convert.ToDecimal(parts[3]),
                }; 
                result.Add(product);
            }
                
            //för varje rad - skapa ny objekt av typ prod
            //stoppa in i listan  
            //returnera listan
            return result;
        }
    }
}
