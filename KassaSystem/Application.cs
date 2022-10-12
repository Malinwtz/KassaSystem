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
                    var userInput = new string[2];
                    var numberOfProducts = 0;

                        while (true)
                        {
                            //SKriov en loop som lopar igeniom reciptens receiptrows och skriver
                            //ut. skriv alltså om kvittot för varje ny vara
                            Commands();
                            //GÖR OM INPUT TILL PRODUKTKOD OCH ANTAL
                            string input = Console.ReadLine().Trim();
                            userInput = input.Split(' ');
                            //LETAR OCH HÄMTAR PRODUKTKOD I LISTAN ALLAPRODUKTER
                            var currentProduct = product1.FindProductFromProductID(allProducts, userInput[0]);

                            //SKriov en loop som lopar igenom listan och skriver ut. 
                            if (input.ToUpper() == "PAY")
                            {
                            //VG:kvitto ska ha löpnr. Kanske counter i ++; vid varje pay och lägg till som
                            //variabel när kvittot skickas till filen
                            //FUNKTION SOM LÄSER SENASTE RAD I FILEN? KAN ISF SKRIVA LÖPNR SIST PÅ KVITTOT
                            //OCH LÄSA INNAN BÖRJAN AV NYTT KVITTO

                            Console.Clear();   
                            Console.WriteLine("KASSA");
                            Console.WriteLine($"KVITTO {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                                //----SKRIV UT SINGLERECEIPT I KONSOLEN----
                                //----LISTAN SKRIVS INTE UT -----------------
                            foreach (var row in allReceipt.ListOfSingleReceipts)
                                { 
                                //---HOPPAR ÖVER ATT GÅ IN I FOREACH??
                                    Console.WriteLine($"{row.ProductName} {numberOfProducts} * {row.Price} " +
                                        $"= {row.Price * numberOfProducts}kr");
                                }
                                Console.WriteLine($"Total: {singleReceiptTotalAmount}" + Environment.NewLine);
                            
                                
                                //----DE PRODUKTER SOM SKRIVS IN UNDER LOOPEN SKA SPARAS HÄR I LOOPEN NEDAN

                                //----KVITTOAVSKILJARE MÅSTE FINNAS-------------räcker med mellanrum?
                                var fileName = DateTime.Now.ToString("RECEIPT_yyy-MM-dd") + ".txt"; //all info lägger vi in på samma rad i filen //relativ sökväg skickas in i filename

                                //VG: LÄS SPECIFIK RAD I FÖRRA KVITTOT FÖR ATT FÅ NÄSTA NR PÅ KVITTOT---------
                                var i = 0;
                                i++;
                                var numberLine = $"KVITTO NR {i}";
                                File.AppendAllText(fileName, Environment.NewLine); //SPARAR TOM RAD I KVITTO
                                File.AppendAllText(fileName, numberLine + Environment.NewLine); //SPARAR KVITTONR
                                //-----FÖR ATT FÅ NÄSTA NR I ORDNINGEN MÅSTE SENASTE NR LÄSAS FRÅN KVITTOFIL?-------------

                                //FÖR VARJE RAD I LISTAN RECEIPTROWS
                                foreach (SingleReceipt row in allReceipt.ListOfSingleReceipts)
                                {   
                                    var line = ""; 
                                    line = $"{row.ProductName} {numberOfProducts} * {row.Price} = {row.Price * numberOfProducts}kr"; 
                                    File.AppendAllText(fileName, line + Environment.NewLine);     //lägger till all inskriven data i EN rad sist i filen //environment.newline för att få en ny rad i filen. 
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
                                //KONVERTERAR ANTAL PRODUKTER TILL INT 
                                numberOfProducts = Convert.ToInt32(userInput[1]);
                                //RÄKNAR UT TOTALSUMMA AV PRODUKTEN
                                currentProduct.TotalPrice = CalculateTotalPriceSingleProduct(numberOfProducts,
                                    Convert.ToDecimal(currentProduct.ProductPrice));
                                //LÄGGER TILL TOTALSUMMA AV PRODUKTEN TILL HELA KVITTOSUMMAN
                                singleReceiptTotalAmount += currentProduct.TotalPrice;

                                //SKRIVER UT PRODUKTENS EGENSKAPER
                                Console.Clear();
                                Console.WriteLine($"{currentProduct.ProductName} {numberOfProducts} * " +
                                    $"{currentProduct.ProductPrice} = {currentProduct.TotalPrice}");
                                Console.WriteLine($"Total: {singleReceiptTotalAmount}kr" + Environment.NewLine);
                               
                           
                                //totalsumma för singlereceipt = för varje rad i  single.receipt finns ett totalprice. loopa igenom 
                    
                                //SPARAR  CURRENTPRODUCT TILL LISTAN allReceipt.ListOfSingleReceipts
                                allReceipt.AddToListOfSingleReceipts(currentProduct.ProductID, currentProduct.ProductName, 
                                    currentProduct.ProductUnit, currentProduct.ProductPrice, currentProduct.TotalPrice, 
                                    numberOfProducts);

                            //---LÄGG TILL BANAN * FLERA ISTÄLLET FÖR PÅ FLER OLIKA RADER
                            //---MÅSTE SPARAS EN COUNT PÅ KVITTORADEN. OM PRODUKTEN FINNS I LISTAN, ÄNDRA COUNT+1 I KVITTORADEN. 
                            //---ÄNDRA VALD RAD I FIL i addtolistofsinglereceipt
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