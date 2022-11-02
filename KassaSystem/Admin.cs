using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
            _listOfProducts = MakeListOfTextProducts();
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
            var productId = _listOfProducts
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
                    Convert.ToDecimal(parts[4]), parts[5], parts[6], Convert.ToInt32(parts[7]));
           //     result.Sort(); sortera i nummerordning
                result.Add(prodct);
            }
            return result;
            //_listOfProducts.OrderByDescending();
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
            Console.WriteLine("Skriv in startdatum för kampanjpriset (yyyy MM dd) :");
            var dateStart = TryDate();
            row.DiscountStartDate = dateStart.ToString("yyyy-MM-dd");
                   
            Console.WriteLine("Skriv in slutdatum för kampanjpriset (yyyy MM dd) :");
            var dateEnd = TryDate();
            row.DiscountEndDate = dateEnd.ToString("yyyy-MM-dd");
               
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
            ReadProductsFromTextFile();
           
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
        public void ReadProductsFromTextFile()
        {
            var productList = File.ReadAllLines("Products.txt").ToList();
            foreach (var row in productList)
            {
                row.ToString();
                var rowArray = row.Split(';');
                _listOfProducts.Add(new Products(rowArray[0].ToString(), rowArray[1].ToString(),
                    rowArray[2].ToString(), Convert.ToDecimal(rowArray[3]), 0, Convert.ToDecimal(rowArray[4]),
                    rowArray[5], rowArray[6]));
            }
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
            File.AppendAllText("Products.txt", line + Environment.NewLine);     
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
            Console.WriteLine("4. Visa försäljningsstatistik");
            Console.WriteLine("0. Avsluta" + Environment.NewLine);
            while (true)
            {
                var sel = 0;
                try { sel = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.WriteLine("Felaktig input"); }
                if (sel >= 0 && sel <= 4) 
                    return sel;
                Console.WriteLine("Felaktig input");
            }
        }
        public DateTime TryDate()
        {
            while (true)
            {
                try
                {
                    var date = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.CurrentCulture);
                    return date;
                }
                catch
                {
                    Console.WriteLine("Felaktig input");
                }
            }

            return DateTime.Now;
        }
        public void SalesStatistics()
        {
            Console.Clear();
            Console.WriteLine("FÖRSÄLJNINGSSTATISTIK");
            var statProductList = new List<Statistics>();

            Console.WriteLine("Skriv in startdatum: ");
            var start = TryDate();
            Console.WriteLine("Skriv in slutdatum: ");
            var end = TryDate();

            var ts = Convert.ToInt32((end - start).TotalDays);

            foreach (var p in ListOfProducts)
            {
                if (File.Exists(p.ProductName + ".txt"))
                {
                    var list = File.ReadAllLines(p.ProductName + ".txt").ToList(); 
                    //       var dates = productList.Where(a => a.Convert.ToTimespan == ts );
                    var count = 0;
                    foreach (var row in list)
                    {   
                        for (var i = 0; i <= ts; i++)
                        {
                            var addedDays = start.AddDays(i);
                            var parts = addedDays.ToString().Split(' ');

                            if (parts[0] == row)
                            {
                                count++;
                            }
                        }
                    }
                    statProductList.Add(new Statistics(p.ProductName, count));
                }
            }
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Antal sålda produkter under tidsperioden:");
            var orderedList = statProductList.OrderByDescending(p=>p.Count);
            foreach (var stat in orderedList)
                Console.WriteLine($"{stat.Name} {stat.Count}"); //skriver ut produkterna flera ggr men 0 i antal

            Console.ReadKey(); 

        }
    }
}

//VG: 
                //rapport med försäljningsstatistik
                //skapa en lista med datum. gå igenom filerna. Kolla hur många av
                //datumen som är mellan en viss tidpunkt. 

                //menyval försälj.statistik
                //ange startdatum - ska skrivas in i datetime (parseexact?)
                //ange slutdatum

                //kan ha en folder för filerna för att skilja på dem. obs valfritt!


//public void DeleteProduct()
//{
//    var fileName = "Products.txt";
//    File.WriteAllLines(fileName,
//        File.ReadLines(fileName).Where(l => l != "removeme").ToList());
//    //readfromfile

//    //ta bort objekt från fil
//}



