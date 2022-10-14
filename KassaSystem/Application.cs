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
            decimal singleReceiptTotalAmount = 0;
            //metod getTotalAmount
            //looopa igenom alla rows i singlereceiptlista och hämta totalpris av varje vara
            
            while (true)
            {
                var sel =  ShowMenu(); 

                if (sel == 1)
                {
                    Products product1 = new Products();
                    AllReceipts allReceipt = new AllReceipts();
                    Console.Clear();

                  //  var numberOfProducts = 0; //GÖR OM TILL PROPERTY I SINGLERECEIPT?

                    while (true)
                    {   
                        ShowCommands();
                        //GÖR OM INPUT TILL PRODUKTKOD OCH ANTAL
                        string input = Console.ReadLine().Trim();
                        var userInput = input.Split(' ');
                        //LETAR OCH HÄMTAR PRODUKTKOD I LISTAN ALLAPRODUKTER
                        var currentProduct = product1.FindProductFromProductID(allProducts, userInput[0]);
                       
                        if (input.ToUpper() == "PAY")
                        {
                            //VG:kvitto ska ha löpnr. Kanske counter i ++; vid varje pay och lägg till som
                            //variabel när kvittot skickas till filen
                            //FUNKTION SOM LÄSER SENASTE RAD I FILEN? KAN ISF SKRIVA LÖPNR SIST PÅ KVITTOT
                            //OCH LÄSA INNAN BÖRJAN AV NYTT KVITTO
                            ShowReceiptHead();
                             
                            foreach (var row in allReceipt.ListOfSingleReceipts)
                            { 
                                //VARFÖR BLIR ROW COUNT 1 FAST JAG SKRIVIT IN TVÅ PRODUKTER?
                                Console.WriteLine($"{row.ProductName} {row.Count} * {row.Price} " +
                                    $"= {row.Price * row.Count}kr");
                            }
                            Console.WriteLine($"Total: {singleReceiptTotalAmount}" + Environment.NewLine);
                            
                            var fileName = DateTime.Now.ToString("RECEIPT_yyy-MM-dd") + ".txt"; 

                                //VG: LÄS SPECIFIK RAD I FÖRRA KVITTOT FÖR ATT FÅ NÄSTA NR PÅ KVITTOT---------
                                var i = 0;
                                i++;
                            var numberLine = $"KVITTO NR {i}";
                            File.AppendAllText(fileName, Environment.NewLine); //SPARAR TOM RAD I KVITTO
                            File.AppendAllText(fileName, numberLine + Environment.NewLine); //SPARAR KVITTONR
                                //-----FÖR ATT FÅ NÄSTA NR I ORDNINGEN KAN SENASTE NR LÄSAS FRÅN KVITTOFIL?-----

                                //FÖR VARJE RAD I LISTAN RECEIPTROWS
                            foreach (SingleReceipt row in allReceipt.ListOfSingleReceipts)
                            {   
                                var line = ""; 
                                line = $"{row.ProductName} {row.Count} * {row.Price} = {row.Price * row.Count}kr"; 
                                File.AppendAllText(fileName, line + Environment.NewLine);     //lägger till data i EN rad sist i fil
                            }
                            break;
                        }

                        //OM PRODUKT EJ FINNS ELLER OM ANTAL INSKRIVNA PRODUKTER OGILTIGT
                        if (currentProduct == null || TryNumberOfProducts(userInput[1]) == false) //produktkod hittas inte || ogiltigt antal produkter
                            {
                                Console.WriteLine("Felaktig input");
                                continue;
                            }

                        //OM PRODUKT FINNS OCH ANTAL PRODUKTER ÄR GILTIGT
                        else if (currentProduct != null && TryNumberOfProducts(userInput[1]) == true)
                        {  
                            //CalculateWriteInConsoleAndSaveToFile(); // FUNKAR BARA OM GÖR OM TILL PROPERTY?
                                var numberOfProducts = Convert.ToInt32(userInput[1]); //GÖR OM TILL PROPERTIY?
                                currentProduct.Count = numberOfProducts;
                                currentProduct.TotalPrice = CalculateTotalPriceSingleProduct(currentProduct.Count,
                                    Convert.ToDecimal(currentProduct.ProductPrice));
                                singleReceiptTotalAmount += currentProduct.TotalPrice; //GÖR OM TILL PROPERTY?

                            //WriteProductPropertiesAndAmount(currentProduct, singleReceiptTotalAmount, numberOfProducts);    
                                Console.Clear();
                                Console.WriteLine($"{currentProduct.ProductName} {currentProduct.Count} * " +
                                    $"{currentProduct.ProductPrice} = {currentProduct.TotalPrice}");
                                Console.WriteLine($"Total: {singleReceiptTotalAmount}kr" + Environment.NewLine);

                            //---VARFÖR BLIR INTE TOTALSUMMAN RÄTT? VERKAR LÄGGA IHOP ALLA SINGLERECEIPTS.
                            //---ANTAL COUNT LÄGGS INTE IHOP PÅ RÄTT SÄTT
    ////forts
                            foreach (var row in allReceipt.ListOfSingleReceipts.ToList())
                            {
                                if (row.ProductID == currentProduct.ProductID) //OM PRODUKTEN REDAN FINNS
                                {//LÄGG TILL ANTAL PRODUKTER I NUVARANDE PRODUKT.COUNT
                                    currentProduct.Count = currentProduct.Count + numberOfProducts;
                                }
                            }  
                                allReceipt.AddToListOfSingleReceipts(currentProduct.ProductID, currentProduct.ProductName, 
                                    currentProduct.ProductUnit, currentProduct.ProductPrice, currentProduct.TotalPrice, 
                                    currentProduct.Count);

                            //IF PRODUKTID EXISTS ADD COUNT++;

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
        private int ShowMenu()
        {
            while (true)
            {
                Console.WriteLine($"KASSA" + Environment.NewLine);
                Console.WriteLine("1.Ny kund");
                Console.WriteLine("2.Administreringsverktyg");
                Console.WriteLine("0.Avsluta");
                var sel = Convert.ToInt32(Console.ReadLine());
                if (sel >= 0 && sel < 3) return sel;    
                Console.WriteLine("Felaktig input");   
            }
        }
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