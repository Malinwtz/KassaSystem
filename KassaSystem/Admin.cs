using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystem
{
    internal class Admin
    {
        private List<Products> _listOfProducts = new List<Products>();
        public Admin()
        {

        }
        public List<Products> ListOfProducts
        {
            get { return _listOfProducts; }
            set { _listOfProducts = value; }
        }
       
        public void CreateNewProduct()
        {   
            Console.Clear();
            Console.WriteLine("SKAPA NY PRODUKT" + Environment.NewLine);
            Console.WriteLine("PRODUKTLISTA:");
            
            var result = MakeListOfTextProducts();
            foreach (var p in result)
            {
                Console.WriteLine($"{p.ProductID};{ p.ProductName};{p.ProductUnit}; " +
                    $"{p.ProductPrice}");
            }

            Console.WriteLine(Environment.NewLine + "Skriv in ny produkt");
            Console.WriteLine("ProduktID:");
            var newId = CheckIfIdExists();
            Console.WriteLine("Namn:");
            var newName = TryName();
            Console.WriteLine("Enhet: (kilopris/styckpris)");
            var newUnit = TryUnit();
            Console.WriteLine("Pris: (per enhet)");
            var newPrice = TryPrice();

            Products product = new Products(newId, newName, newUnit, newPrice);
            product.DiscountPrice = 0;
            product.DiscountStartDate = "0001-01-01";
            product.DiscountEndDate = "0001-01-01";
                AddToFile(product);

            Console.WriteLine("Ny produkt sparad" + Environment.NewLine);
        }
        public string CheckIfIdExists()
        {
            while (true)
            {
                var newId = TryId();
                var id = FindProductWithId(newId);
                if (id != null) Console.WriteLine(Environment.NewLine + "ID finns redan");
                else return newId; 
            }
        }
        public Products FindProductWithId(string id)
        {
            var productId = MakeListOfTextProducts()
                .FirstOrDefault(p => p.ProductID == id);
            return productId;
        }
        public List<Products> MakeListOfTextProducts()
        {
            var result = new List<Products>();
            foreach (var line in File.ReadLines("Products.txt"))
            {
                var parts = line.Split(';');
                var prodct = new Products(parts[0], parts[1], parts[2], Convert.ToDecimal(parts[3]), 0,
                    Convert.ToDecimal(parts[4]), parts[5], parts[6]);
           //     result.Sort(); sortera i nummerordning
                result.Add(prodct);
            }
            return result;
            //
            //var productList = File.ReadAllLines("Products.txt").ToList();
            //foreach (var row in productList)
            //{
            //    row.ToString();
            //    var rowArray = row.Split(';');
            //    _listOfProducts.Add(new Products(rowArray[0].ToString(), rowArray[1].ToString(),
            //        rowArray[2].ToString(), Convert.ToDecimal(rowArray[3]), 0, Convert.ToDecimal(rowArray[4]),
            //        rowArray[5], rowArray[6]));
            //}
            ////   _listOfProducts.OrderByDescending();
        }
        private decimal TryPrice()
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
        private string TryUnit()
        {
            while (true)
            {
               var unit = Console.ReadLine();
                if (unit == "kilopris" || unit == "styckpris") return unit;
                else Console.WriteLine("Felaktig input");
            }
        }
        public string TryName()
        {
            while (true)
            {
                var name = Console.ReadLine();
                if (name != null && name.Length > 1) return name;
                else Console.WriteLine("Felaktig input");
            }
        }
        public string TryId()
        {
            while (true)
            {  
                try
                {
                    var newId = Convert.ToInt32(Console.ReadLine());
                    if (newId.ToString().Count() == 3)
                        return Convert.ToString(newId);
                    else Console.WriteLine("Felaktig input");
                }
                catch { Console.WriteLine("Felaktig input"); }
            }
        }
        public void ChangeProduct()
        {
            Console.Clear();
            Console.WriteLine("ÄNDRA PRODUKT" + Environment.NewLine);
            ShowProductsWithDiscount();
            
            Console.WriteLine("Skriv in ID på den produkt du vill ändra:");
            var selChange = "";

            while (true)
            {
                selChange = TryId();
                var id = FindProductWithId(selChange);
                if (id == null) Console.WriteLine(Environment.NewLine + "ID finns inte");
                else break;
            }

            foreach (var row in _listOfProducts.ToList())
            {
                if (row.ProductID.ToLower() == selChange.ToLower())
                {
                    var select = ShowMenuChangeProduct();
                    
                    if (select == 1)
                    {
                        Console.WriteLine("Tar bort från listan");
                        _listOfProducts.Remove(row);
                    }
                    else if (select == 2)
                    {
                        Console.WriteLine("Skriv in ett nytt namn på produkten:");
                        row.ProductName = TryName();
                        Console.WriteLine("Namn sparat" + Environment.NewLine );
                    }
                    else if (select == 3)
                    {   
                        Console.WriteLine("Skriv in ett nytt ID på produkten:");
                        var newId = CheckIfIdExists();
                        row.ProductID = newId;
                        Console.WriteLine("ID sparat" + Environment.NewLine );
                    }
                    else if (select == 4)
                    {
                        Console.WriteLine("Skriv in en ny enhet på produkten:");
                        row.ProductUnit = TryUnit();
                        Console.WriteLine("Enhet sparad" + Environment.NewLine);
                    }
                    else if (select == 5)
                    {
                        DiscountChoice(row);
                    }
                    
                    else if (select == 0)
                        break;
                }
            }
           
            File.Delete("Products.txt");
         
            foreach (var row in _listOfProducts)
            {   
                var line = $"{row.ProductID};{row.ProductName};{row.ProductUnit};{row.ProductPrice};" +
                    $"{row.DiscountPrice};{row.DiscountStartDate};{row.DiscountEndDate}";
                File.AppendAllText("Products.txt", line + Environment.NewLine);
            }
        }

        private void DiscountChoice(Products row)
        {
            Console.WriteLine("1. Skriv in ett nytt pris");
            Console.WriteLine("2. Skriv in ett kampanjpris");
            Console.WriteLine("0. Tillbaka till huvudmenyn");
            while (true)
            {
                try
                {
                    var slct = Convert.ToInt32(Console.ReadLine());
                    if (slct == 1)
                    {
                        Console.WriteLine("Skriv in ett nytt pris på produkten:");
                        row.ProductPrice = TryPrice();
                        Console.WriteLine("Pris sparat" + Environment.NewLine);
                        Console.ReadKey();
                        break;
                    }
                    else if (slct == 2)
                    {
                        CreateDiscount(row);
                        Console.WriteLine(Environment.NewLine + "Kampanjpris sparat");
                        break;
                    }
                    else if (slct == 0)
                        break;
                    else
                        Console.WriteLine("Felaktig input");
                }
                catch { Console.WriteLine("Felaktig input"); }
            }
        }
        private void CreateDiscount(Products row)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Skriv in startdatum för kampanjpriset (yyyy MM dd) :");
                    DateTime dateStart = Convert.ToDateTime(Console.ReadLine());
                    row.DiscountStartDate = dateStart.ToString("yyyy-MM-dd");
                    break;
                }
                catch { Console.WriteLine("Felaktig input"); }
            }
            while (true)
            {
                try
                {
                    Console.WriteLine("Skriv in slutdatum för kampanjpriset (yyyy MM dd) :");
                    DateTime dateEnd = Convert.ToDateTime(Console.ReadLine());
                    row.DiscountEndDate = dateEnd.ToString("yyyy-MM-dd");
                    break;
                }
                catch { Console.WriteLine("Felaktig input"); }

            }
            while (true)
            {
                try
                {
                    Console.WriteLine("Skriv in kampanjpris:");
                    decimal discount = Convert.ToDecimal(Console.ReadLine());
                    row.DiscountPrice = discount;
                    break;
                }
                catch { Console.WriteLine("Felaktig input"); }
            }
        }
        
        public void ShowProductsWithDiscount()
        {
            Console.WriteLine("PRODUKTLISTA");
            var productList = File.ReadAllLines("Products.txt").ToList();
            foreach (var row in productList)
            {
                row.ToString();
                var rowArray = row.Split(';');
                _listOfProducts.Add(new Products(rowArray[0].ToString(), rowArray[1].ToString(),
                    rowArray[2].ToString(), Convert.ToDecimal(rowArray[3]), 0, Convert.ToDecimal(rowArray[4]),
                    rowArray[5], rowArray[6]));
            }
            //   _listOfProducts.OrderByDescending();
            foreach (var product in _listOfProducts)
            {
                Console.WriteLine($"{product.ProductID};{product.ProductName};{product.ProductUnit};" +
                    $"{product.ProductPrice}kr");
                if (product.DiscountPrice > 0)
                {
                    Console.WriteLine($"  *KAMPANJPRIS: {product.DiscountPrice}kr; {product.DiscountStartDate:yyyy-MM-dd}" +
                        $" - {product.DiscountEndDate:yyyy-MM-dd}");
                }
            }
            Console.WriteLine(Environment.NewLine);
        }

        private int ShowMenuChangeProduct()
        {
            Console.WriteLine("1. Ta bort");
            Console.WriteLine("2. Ändra namn");
            Console.WriteLine("3. Ändra ID");
            Console.WriteLine("4. Ändra enhet");
            Console.WriteLine("5. Ändra pris");
            Console.WriteLine("Ange val 1-5. Välj 0 för att avbryta." + Environment.NewLine);
            var select = -1;
            while (true)
            {
                try { select = Convert.ToInt32(Console.ReadLine()); }
                catch (Exception x) { /* Console.WriteLine(x.Message);*/}

                if (select >= 0 && select <= 5) return select;

                Console.WriteLine("Felaktig input");
            }
        }
        private void AddToFile(Products product)
        {
            var line = $"{product.ProductID};{product.ProductName};{product.ProductUnit};{product.ProductPrice}" +
                $";{product.DiscountPrice};{product.DiscountStartDate};{product.DiscountEndDate}";
            File.AppendAllText("Products.txt", line + Environment.NewLine);     //lägger till data i EN rad sist i fil
        }

        public bool TryUserInputDecimal(string uInput)
        {
            if (uInput == null || Convert.ToDecimal(uInput) < 0)
            {
                return false;
            }
            else return true;
        }
        public static int AdminMenu()
        {
            Console.WriteLine("ADMINISTRERINGSMENY" + Environment.NewLine);
            Console.WriteLine("1. Skapa ny produkt");
            Console.WriteLine("2. Ta bort eller ändra produkt");
            Console.WriteLine("3. Visa produkter och kampanjpriser");
            Console.WriteLine("0. Avsluta" + Environment.NewLine);
            while (true)
            {
                var sel = 0;
                try { sel = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.WriteLine("Felaktig input"); }
                if (sel >= 0 && sel <= 3) 
                    return sel;
                Console.WriteLine("Felaktig input");
            }
        }
    }
}

//public void DeleteProduct()
//{
//    var fileName = "Products.txt";
//    File.WriteAllLines(fileName,
//        File.ReadLines(fileName).Where(l => l != "removeme").ToList());
//    //readfromfile

//    //ta bort objekt från fil
//}

////SKAPA NYTT OBJEKT AV TYPEN PRODUCT
//var result = new List<Products>();
//    //FÖR VARJE RAD I TEXTFILEN 
//   foreach(var line in File.ReadLines("Products.txt")) 
//         //ReadAllLines läser ALLA rader och tar upp ram-minne //ReadLines läser EN rad i taget. använd därför denna när läsa filer
//    {
//        //DELA RADEN LINE, LÄGG ALLA DELARNA I EN ARRAY 
//        var parts = line.Split(';');
////SKAPA NYTT OBJEKT AV PRODUCTS DÄR STRÄNGARNA BLIR PROPERTIES
//var product = new Products(parts[0], parts[1], parts[2], Convert.ToDecimal(parts[3]), 0);
////ADDERA TILL PRODUKTLISTAN
//result.Add(product);
//    }

//public decimal CheckIfDiscount(Products product) //DateTime start, DateTime end, decimal price
//{
//    if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday && DateTime.Now.Hour < 13)
//    {
//        return Convert.ToDecimal(product.ProductPrice);
//    }

//    else return Convert.ToDecimal(product.ProductPrice);
//}
//WriteAllText skriver bara ut sista raden i listan till filen
//AppendAllText lägger till EN rad sist i filen. Lägger till listan en gång till utan att ta bort den tidigare.
//CreateText 
//ReadAllLines läser ALLA rader och tar upp ram-minne 
//ReadLines läser EN rad i taget. använd därför denna när läsa filer
