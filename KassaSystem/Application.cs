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
            //HÄMTAR PRODUKTER FRÅN TEXTFILEN OCH SPARA I EN LISTA ALLPRODUCTS
            var allProducts = ReadProductsFromFile();  
            decimal singleReceiptTotalAmount = 0;
            decimal receiptTotal = 0;
            
            while (true)
            {
                var sel =  Menu(); 

                if (sel == 1)
                {
                    Products product1 = new Products();
                    AllReceipts allReceipt = new AllReceipts();
                    Console.Clear();
                    var pay = "";
                    bool register = true;
                    var userInput = new string[2];
                    var numberOfProducts = 0;

                        while (register)
                        {
                            //SKriov en loop som lopar igeniom reciptens receiptrows och skriver
                            //ut. skriv alltså om kvittot för varje ny vara
                            Commands();
                            //GÖR OM INPUT TILL PRODUKTKOD OCH ANTAL
                            string input = Console.ReadLine().Trim();
                            userInput = input.Split(' ');
                            //LETAR OCH HÄMTAR PRODUKTKOD I LISTAN ALLAPRODUKTER
                            var currentProduct = product1.FindProductFromProductID(allProducts, userInput[0]);


                            if (input.ToUpper() == "PAY")
                            {
 //VG:kvitto ska ha löpnr. Kanske counter i ++; vid varje pay och lägg till som
 //variabel när kvittot skickas till filen
 //FUNKTION SOM LÄSER SENASTE RAD I FILEN? KAN ISF SKRIVA LÖPNR SIST PÅ KVITTOT
 //OCH LÄSA INNAN BÖRJAN AV NYTT KVITTO
                                Console.WriteLine("KASSA");
                                Console.WriteLine($"KVITTO {DateTime.Now.ToString("yyy-MM-dd-HH-mm-ss")}");
                                //SKRIV UT SINGLERECEIPT I KONSOLEN-----------
                                //LISTAN SKRIVS INTE UT -----------------
                                foreach (SingleReceipt row in allReceipt.ListOfSingleReceipts)
                                { 
                                    Console.WriteLine($"{row.ProductName}:{row.Price}kr " +
                                        $"{row.ProductUnit}, {row.Price * numberOfProducts}kr");
                                }
                                Console.WriteLine($"Total: {singleReceiptTotalAmount}");
                                
                                //----DE PRODUKTER SOM SKRIVS IN UNDER LOOPEN SKA SPARAS HÄR I LOOPEN NEDAN

                                //----KVITTOAVSKILJARE MÅSTE FINNAS-------------
                                var fileName = DateTime.Now.ToString("RECEIPT_yyy-MM-dd") + ".txt"; //all info lägger vi in på samma rad i filen //relativ sökväg skickas in i filename
                                var i = 0; //LÄS SPECIFIK RAD I FÖRRA KVITTOT FÖR ATT FÅ NÄSTA NR PÅ KVITTOT---------
                                i++;
                                var numberLine = $"KVITTO NR {i}";
                                File.AppendAllText(fileName, Environment.NewLine); //SPARAR TOM RAD I KVITTO
                                File.AppendAllText(fileName, numberLine + Environment.NewLine); //SPARAR KVITTONR
                                //-----FÖR ATT FÅ NÄSTA NR I ORDNINGEN MÅSTE SENASTE NR LÄSAS FRÅN KVITTOFIL?-------------

                                //FÖR VARJE RAD I LISTAN RECEIPTROWS
                                foreach (SingleReceipt row in allReceipt.ListOfSingleReceipts)
                                {   
                                    var line = "";
                                    //SPARAR //SKRIV UT 
                                    line = $"{row.ProductName}:{row.Price}kr {row.ProductUnit}, {row.Price * numberOfProducts}kr"; //sparar inskriven data till en stringvariabel 
                                    Console.WriteLine($"Sparar {line} i fil: {fileName}");
                                    File.AppendAllText(fileName, line + Environment.NewLine);     //lägger till all inskriven data i EN rad sist i filen //environment.newline för att få en ny rad i filen. 
                                    
                                }
                                register = false;
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
                                //KONVERTERAR ANTAL PRODUKTER TILL INT 
                                numberOfProducts = Convert.ToInt32(userInput[1]);
                                //RÄKNAR UT TOTALSUMMA AV PRODUKTEN
                                currentProduct.TotalPrice = CalculateTotalPriceSingleProduct(numberOfProducts,
                                    Convert.ToDecimal(currentProduct.ProductPrice));
                                //LÄGGER TILL TOTALSUMMA AV PRODUKTEN TILL HELA KVITTOSUMMAN
                                singleReceiptTotalAmount += currentProduct.TotalPrice;
                                
                                //SKRIVER UT PRODUKTENS EGENSKAPER
                                Console.WriteLine($"{currentProduct.ProductName} {numberOfProducts} * " +
                                    $"{currentProduct.ProductPrice} = {currentProduct.TotalPrice}");
                              
                                Console.WriteLine($"Total: {singleReceiptTotalAmount}kr");
                           

                                //totalsumma för singlereceipt = för varje rad i  single.receipt finns ett totalprice. loopa igenom 
                    
                          //KASSA - ska se ut som nedan:                                
                         //KVITTO 2019-09-08 05:13:31 ÄNDRA BINDESTRECK I KLOCKSLAGET TILL KOLON, TA BORT BINDESTRECK MELLAN DATUM OCH KLOCKSLAG
                         //PRODUKTNAMN ANTAL * PRIS = TOTAL

                                //OM SAMMA PRODUKT LÄGGS TILL IGEN - ÄNDRA ANTAL PRODUKTER I KVITTOT. kanske görs automatiskt när ändrar objektet?
                                //om det finns en sån egenskap i ett objekt. kanske kan lägga till i kvittoobjekt? eller lägga till i produktobjekt?

                                

                                //TILLDELA EGENSKAPERNA TILL KVITTOOBJEKT//ADDERA KVITTORAD TILL EN LISTA - UPPDATERA KVITTO FÖR VARJE NY VARA 
                                //SKriov en loop som lopar igenom reciptens receiptrows och skriver ut. 

                                //SPARAR  CURRENTPRODUCT TILL LISTAN allReceipt.ListOfSingleReceipts
                                allReceipt.AddToListOfSingleReceipts(currentProduct.ProductID, currentProduct.ProductName, 
                                    currentProduct.ProductUnit, currentProduct.ProductPrice, currentProduct.TotalPrice, numberOfProducts);

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
        //METHODS
        private void Commands()
        {
            Console.WriteLine("Kommandon:");
            Console.WriteLine("<productid> <antal>");
            Console.WriteLine("Kommando PAY");
            Console.WriteLine("Kommando:");
        }
        public int Menu()
        {
            while (true)
            {
                Console.WriteLine("KASSA");
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
        } //KOLLAR SÅ ANTAL ÄR SIFFRA
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
                var product = new Products(parts[0], parts[1], parts[2], Convert.ToDecimal(parts[3]), 0); 
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