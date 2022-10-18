using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            var allProducts = ReadProductsFromFile();  
            //decimal singleReceiptTotalAmount = 0;
            //metod getTotalAmount
            //looopa igenom alla rows i singlereceiptlista och hämta totalpris av varje vara
            
            while (true)
            {
                ShowMenu();
                var sel = ReturnFromMenu(); 

                if (sel == 1)
                {
                    Products product1 = new Products();
                    AllReceipts allReceipt = new AllReceipts();
                    Console.Clear();

                    while (true)
                    {   
                        ShowCommands();
                        
                        var currentProduct = new Products();
                        var input = "";
                        
                        input = Console.ReadLine().Trim();
                        var userInput = input.Split(' ');
                       
                        currentProduct = product1.FindProductFromProductID(allProducts, userInput[0]);
                           
                        if (input.ToUpper() == "PAY")
                        {
                            //"utan hål i kvitto" med löpande nr. lagra undan kvittonr
                            //fil med lastreceipt.txt med bara senaste kvittonr. läs nr och plussa varje gång ny vara
                            //VG:kvitto ska ha löpnr. Kanske counter i ++; vid varje pay och lägg till som
                            //variabel när kvittot skickas till filen
                            //FUNKTION SOM LÄSER SENASTE RAD I FILEN? KAN ISF SKRIVA LÖPNR SIST PÅ KVITTOT
                            //OCH LÄSA INNAN BÖRJAN AV NYTT KVITTO
                            ShowReceiptHead();
                            allReceipt.ShowListOfProducts();
                            allReceipt.WriteTotalAmount();
                            
                            var fileName = DateTime.Now.ToString("RECEIPT_yyy-MM-dd") + ".txt"; 

                            //---VG: LÄS SPECIFIK RAD I FÖRRA KVITTOT FÖR ATT FÅ NÄSTA NR PÅ KVITTOT---------
                            var i = 0;
                            i++;
                            var numberLine = $"KVITTO NR {i}";
                            File.AppendAllText(fileName, Environment.NewLine); //SPARAR TOM RAD I KVITTO
                            File.AppendAllText(fileName, numberLine + Environment.NewLine); //SPARAR KVITTONR
                                //-----FÖR ATT FÅ NÄSTA NR I ORDNINGEN KAN SENASTE NR LÄSAS FRÅN KVITTOFIL?-----

                            foreach (var row in allReceipt.ListOfSingleReceipts)
                            {   
                                var line = $"{row.ProductName} {row.Count} * {row.Price} = {row.Price * row.Count}kr"; 
                                File.AppendAllText(fileName, line + Environment.NewLine);     //lägger till data i EN rad sist i fil
                            }
                            var total = $"Total: {Convert.ToString(allReceipt.CalculateTotal())}kr";
                            File.AppendAllText(fileName, total + Environment.NewLine);
                            break;
                        }

                        //OM PRODUKT EJ FINNS ELLER OM ANTAL INSKRIVNA PRODUKTER OGILTIGT
                        if (userInput.Length != 2 || currentProduct == null || userInput[1] == null 
                            || TryUserInputNumbers(userInput[1]) == false) 
                        {
                            Console.WriteLine("Felaktig input");
                                continue;
                        }

                        else if (currentProduct != null && TryUserInputNumbers(userInput[1]) == true)
                        {
                            var numberOfProducts = Convert.ToInt32(userInput[1]);
                            if (allReceipt.IsListContaining(currentProduct) == true)
                            {
                                SaveProductIfAlreadyInList(allReceipt, currentProduct, numberOfProducts);
                                allReceipt.WriteTotalAmount();
                            }
                            else if (!allReceipt.IsListContaining(currentProduct))
                            {
                                SaveNewProductToLIst(currentProduct, allReceipt, numberOfProducts);
                                Console.Clear();
                                Console.WriteLine($"{currentProduct.ProductName} {currentProduct.Count} * " +
                                        $"{currentProduct.ProductPrice} = {currentProduct.TotalPrice}");
                                allReceipt.WriteTotalAmount();
                            }
                        }
                    }
                }
                else if (sel == 2)
                {

                    Console.Clear();
                    var selected = Admin.AdminMenu();

                    //1.NewProduct
                        //lägg till objekt till fil: File.AppendAllText(fileName, line + Environment.NewLine); 
                    //2.DeleteProduct
                        //ta bort objekt från fil
                    //3.ChangeProduct
                        //ta ut eller radera från fil
                        //ändra objekt
                            //ändra namn 
                            //ändra pris
                            //kampanjpris mellan datetime-datetime
                        //ersätt objekt i fil 

                    //0.AVSLUTA
                }
                else if (sel == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Avslutar kassasystem");
                    break;
                }
                else
                    Console.WriteLine("Felaktig input");
            }
        }

        private void SaveProductIfAlreadyInList(AllReceipts allReceipt, Products currentProduct, int numberOfProducts)
        {
            foreach (var row in allReceipt.ListOfSingleReceipts.ToList())
            {
                if (row.ProductID == currentProduct.ProductID)
                {
                    row.Count += numberOfProducts;
                    row.TotalPrice = CalculateTotalPriceSingleProduct(row.Count, Convert.ToDecimal(row.Price));

                    Console.Clear();
                    Console.WriteLine($"{row.ProductName} {row.Count} * " + $"{row.Price} = {row.TotalPrice}");
                }
            }
        }
        private void SaveNewProductToLIst(Products currentProduct, AllReceipts allReceipt, int numberOfProducts)
        {
            currentProduct.Count = numberOfProducts;
            currentProduct.TotalPrice = CalculateTotalPriceSingleProduct(currentProduct.Count,
                Convert.ToDecimal(currentProduct.ProductPrice));

            allReceipt.AddToListOfSingleReceipts(currentProduct);
        }
        private void ShowReceiptHead()
        {
            Console.Clear();
            Console.WriteLine("KASSA");
            Console.WriteLine($"KVITTO {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
        private void ShowCommands()
        {
            Console.WriteLine("Kommandon:");
            Console.WriteLine("<productid> <antal>");
            Console.WriteLine("Kommando PAY");
            Console.WriteLine("Kommando:");
        }
        private void ShowMenu()
        {
            Console.WriteLine($"KASSA" + Environment.NewLine);
            Console.WriteLine("1.Ny kund");
            Console.WriteLine("2.Administreringsverktyg");
            Console.WriteLine("0.Avsluta");
        }
        public int ReturnFromMenu()
        {
            var sel = -1;
            while (true)
            {  
                try { sel = Convert.ToInt32(Console.ReadLine()); }
                catch (Exception x) { /* Console.WriteLine(x.Message);*/}
                    
                if (sel >= 0 && sel <= 3) return sel;

                Console.WriteLine("Felaktig input");
            }
        }
        public bool TryUserInputNumbers(string uInput)
        {
            if (uInput == null || Convert.ToInt32(uInput) < 0)
            {
                return false;
            }
            else return true;
        }
        private List<Products> ReadProductsFromFile()
        {
            //SKAPA NYTT OBJEKT AV TYPEN PRODUCT
            var result = new List<Products>();
            //FÖR VARJE RAD I TEXTFILEN 
           foreach(var line in File.ReadLines("Products.txt")) 
                 //ReadAllLines läser ALLA rader och tar upp ram-minne //ReadLines läser EN rad i taget. använd därför denna när läsa filer
            {
                //DELA RADEN LINE, LÄGG ALLA DELARNA I EN ARRAY 
                var parts = line.Split(';');
                 //SKAPA NYTT OBJEKT AV PRODUCTS DÄR STRÄNGARNA BLIR PROPERTIES
                var product = new Products(parts[0], parts[1], parts[2], Convert.ToDecimal(parts[3]), 0/*, 0*/); 
                //ADDERA TILL PRODUKTLISTAN
                result.Add(product);
            }
            //RETURNERA LISTAN
            return result;
        }
        private decimal CalculateTotalPriceSingleProduct(int numberOfProducts, decimal price)
        {
            var totalPrice = price * numberOfProducts;
            return totalPrice;
        }
    }
}





//METODER

//adm i en ny mapp

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


//Adminverktyg(VG).Där ska man kunna

//- Ändra namn, pris för produkter

//(OBS!!! Ändrar man produktens pris kan man ju inte ändra pris på befintliga kvitton osv)

//-kvitton ska ha ett löpnummer! Utan ”hål” i.

//Kampanjpris: man ska kunna säga att from 2022-10-12 till 2022-10-18 så ska banananerna kosta 10kr

//Kampanjpris: man ska kunna säga att from 2022-10-20 till 2022-10-24 så ska banananerna kosta 11kr

//FÖRBÄTTRA FELSÖKNING TRYNUMBEROFPRODUCTS