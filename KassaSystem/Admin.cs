using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
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
            
            var result = new List<Products>();
            foreach (var line in File.ReadLines("Products.txt"))
            {   
                var parts = line.Split(';');
                //SKAPA NYTT OBJEKT AV PRODUCTS DÄR STRÄNGARNA BLIR PROPERTIES
                var product = new Products(parts[0], parts[1], parts[2], Convert.ToDecimal(parts[3]), 0);
                //ADDERA TILL PRODUKTLISTAN
                result.Add(product);
            }
            foreach (var product in result)
            {
                Console.WriteLine($"{product.ProductID};{ product.ProductName};{product.ProductUnit}; " +
                    $"{product.ProductPrice}");
            }

            Console.WriteLine("Skapa en ny produkt genom att skriva in: ID;namn;enhet(kg/st);pris(per enhet)");
            var newProduct = Console.ReadLine().Trim(); 
            var newProductArray = newProduct.Split(';');
            
            if (newProductArray.Length != 4 || TryUserInputDecimal(newProductArray[3]) == false)
            {
                Console.WriteLine("Felaktig input");
            }
            else
            {
                Products product = new Products(newProductArray[0], newProductArray[1], newProductArray[2], 
                    Convert.ToDecimal(newProductArray[3]), 0);

                AddToFile(product);
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
        public void ChangeProduct()
        {  
            var productList = File.ReadAllLines("Products.txt").ToList();
            foreach (var row in productList)
            {   
                row.ToString();
                var rowArray = row.Split(';');
                _listOfProducts.Add(new Products(rowArray[0].ToString(), rowArray[1].ToString(), 
                    rowArray[2].ToString(), Convert.ToDecimal(rowArray[3]), 0));
            }
         //   _listOfProducts.Sort();
            foreach (var product in _listOfProducts)
            {
                Console.WriteLine($"{product.ProductID};{product.ProductName};{product.ProductUnit};" +
                    $"{product.ProductPrice}");
            }
            
            Console.WriteLine(Environment.NewLine + "Skriv in ID på den produkt du vill ändra:");
            var selChange = Console.ReadLine(); 
            foreach (var row in _listOfProducts.ToList())
            {
                if (row.ProductID.ToLower() == selChange.ToLower())
                {
                    var select = ShowMenuChangeProduct();
                    
                    if (select == 1)
                    {
                        Console.WriteLine("Tar bort från listan");
                        _listOfProducts.Remove(row);

                        //var itemToRemove = resultlist.Single(r => r.Id == 2);
                        //resultList.Remove(itemToRemove);

                    }
                    else if (select == 2)
                    {
                        Console.WriteLine("Skriv in ett nytt namn på produkten:");
                        row.ProductName = Console.ReadLine();
                    }
                    else if (select == 3)
                    {
                        Console.WriteLine("Skriv in ett nytt ID på produkten:");
                        row.ProductID = Console.ReadLine();
                    }
                    else if (select == 4)
                    {
                        var slct = Convert.ToInt32(Console.ReadLine());
                        if (slct == 1)
                        {
                            Console.WriteLine("Skriv in ett nytt pris på produkten:");
                            row.ProductPrice = Convert.ToDecimal(Console.ReadLine());
                        }
                        else if (slct == 2) 
                        {
                            Console.WriteLine("Skriv in startdatum för kampanjpriset (yyyy MM dd) :");
                            DateTime dateStart = Convert.ToDateTime(Console.ReadLine());
                            Console.WriteLine("Skriv in slutdatum för kampanjpriset (yyyy MM dd) :");
                            DateTime dateEnd = Convert.ToDateTime(Console.ReadLine());
                            Console.WriteLine("Skriv in kampanjpris:");
                            decimal discount = Convert.ToDecimal(Console.ReadLine());
                            
                            row.DiscountStartDate = dateStart;
                            row.DiscountEndDate = dateEnd;
                            row.DiscountPrice = discount;

                            //---VARJE GÅNG NY VARA VÄLJS - KOLLA OM DISCOUNT DATE STÄMMER ÖVERRENS - 
                            //---I SÅ FALL, BYT UT PRICE MOT DISCOUNT PRICE
                            // - KSIfToday();
                           
                            //---ÄNDRA INTE URSPRUNGLIGT PRIS - ÄNDRA BARA JUST DEN GÅNGEN VARAN VÄLJS 
                            //---OCH SPARAS TILL KVITTOT

                            //----I ADMIN - TILLDELA VÄRDEN TILL DISCOUNT PRICE OCH DATE

                        }
                    }
                    else if (select == 5)
                    {
                        Console.WriteLine("Skriv in en ny enhet på produkten:");
                        row.ProductUnit = Console.ReadLine(); 
                    }
                    else if (select == 0)
                        break;
                }
            }
           
            File.Delete("Products.txt");
            var line = "";
            foreach (var row in _listOfProducts)
            {   
                line = $"{row.ProductID};{row.ProductName};{row.ProductUnit};{row.ProductPrice}";
                File.AppendAllText("Products.txt", line + Environment.NewLine);
            }

            //WriteAllText skriver bara ut sista raden i listan till filen
            //AppendAllText lägger till EN rad sist i filen. Lägger till listan en gång till utan att ta bort den tidigare.
            //CreateText 
            //ReadAllLines läser ALLA rader och tar upp ram-minne 
            //ReadLines läser EN rad i taget. använd därför denna när läsa filer

        }

        public decimal CheckIfDiscount(Products product) //DateTime start, DateTime end, decimal price
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday && DateTime.Now.Hour < 13)
            {
                return Convert.ToDecimal(product.ProductPrice /* * 0, 8*/);
            }
                
            else return Convert.ToDecimal(product.ProductPrice);
        }
        private int ShowMenuChangeProduct()
        {
            Console.WriteLine("1. Ta bort");
            Console.WriteLine("2. Ändra namn");
            Console.WriteLine("3. Ändra ID");
            Console.WriteLine("4. Ändra pris");
            Console.WriteLine("5. Ändra enhet");
            Console.WriteLine("Ange val 1-5. Välj 0 för att avbryta.");
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
            var line = $"{product.ProductID};{product.ProductName};{product.ProductUnit};{product.ProductPrice}";
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
            while (true)
            {
                Console.WriteLine("ADMINISTRERINGSMENY");
                Console.WriteLine(" ");
                Console.WriteLine("1. Skapa ny produkt");
                Console.WriteLine("2. Ta bort eller ändra produkt");
                Console.WriteLine("3. Kampanjpris");
                Console.WriteLine("0. Avsluta");
                var sel = Convert.ToInt32(Console.ReadLine());
                if (sel >= 0 && sel <= 4) return sel;
                Console.WriteLine("Felaktig input");
            }
        }
    }
}
//om fil finns
//ny fil skapas med product.name + discount.txt
//SKAPA FIL SOM HETER PRODUKTNAMNET - VARJE GÅNG NY PRODUKT - SKAPA DISCOUNT-FIL
//DÄR STÅR PRODUKT.ID OCH KAMPANJPRIS ex 300;15
//EN NY FIL FÖR VARJE PRODUKT
//SIST I FILEN DEN DATETIME - DATETIME FÖR KAMPANJEN SOM GÄLLER
//LÄS SISTA RAD I FIL VARJE GÅNG I KASSA
//----HUR SPARA NY FIL I KASSA? MÅSTE REDAN FINNAS FILER - EN/PRODUKT
//OM DAGENS DATUM ÄR I SPANNET: DATETIME - DATETIME
//- LÄS HELA FILEN OCH GÖR OM VARJE RAD TILL PRODUKT MED NYTT PRIS
//OM DATETIME STÄMMER - SKRIV UT NY PROPERTY SOM HETER DIISCOUNTPRICE?

