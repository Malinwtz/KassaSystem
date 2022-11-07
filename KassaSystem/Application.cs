using KassaSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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
            while (true)
            {
                var sel = Menu.CashMenu();

                if (sel == 1)
                {
                    AllReceipts allReceipt = new ();
                    Admin admin = new Admin();
                    Console.Clear();

                    while (true)
                    {   
                        ShowCommands();
                        var input = Console.ReadLine().Trim();
                        var userInput = input.Split(' ');
                        var currentProduct = admin.FindProductWithId(userInput[0]);
                           
                        if (input.ToUpper() == "PAY")
                        {  
                            ShowReceiptHead();
                            allReceipt.ShowListOfProducts(); 
                            allReceipt.ShowTotalAmount();
                            allReceipt.SaveToReceipt();
                            allReceipt.SaveDateToProductNameFile();
                            allReceipt.ChangeCountToProductsTextFile();
                            Console.ReadKey(); 
                            break;
                        }

                        if (userInput.Length != 2 || currentProduct == null || userInput[1] == null 
                            || TryUserInputNumbers(userInput[1]) == false) 
                        {
                            Console.WriteLine("Felaktig input");
                                continue;
                        }

                        else if (currentProduct != null && TryUserInputNumbers(userInput[1]) == true)
                        {
                            var id = currentProduct.ProductID.ToString();

                            var numberOfProducts = Convert.ToInt32(userInput[1]);
                            if (allReceipt.IsListContaining(currentProduct) == true)
                            {
                                SaveProductIfAlreadyInList(allReceipt, currentProduct, numberOfProducts);
                                allReceipt.ShowTotalAmount();
                            }
                            else if (!allReceipt.IsListContaining(currentProduct))
                            {
                                currentProduct = currentProduct.CheckIfDiscount(currentProduct.ProductID);
                                SaveNewProductToLIst(currentProduct, allReceipt, numberOfProducts);
                                Console.Clear();
                                Console.WriteLine($"{currentProduct.ProductName} {currentProduct.Count} * " +
                                        $"{currentProduct.ProductPrice.ToString("F", CultureInfo.InvariantCulture)}" +
                                        $" = {currentProduct.TotalPrice.ToString("F", CultureInfo.InvariantCulture)}");
                                allReceipt.ShowTotalAmount();
                            }
                        }
                    }
                }

                else if (sel == 2)
                {
                    Admin admin = new Admin();  
                    Console.Clear();
                    var sel2 = Menu.AdminMenu();

                    if (sel2 == 1)
                        admin.CreateNewProduct();
                    else if (sel2 == 2)
                        admin.ChangeProduct(); 
                    else if (sel2 == 3)
                    {
                        admin.ShowProductsWithDiscount(); 
                        Console.ReadKey();
                    }
                    else if (sel2 == 4)
                    {
                        admin.SalesStatistics();
                    }
                    else if (sel2 == 0)
                        Console.Clear();
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
                    row.TotalPrice = currentProduct.CalculateTotalPriceSingleProduct(row.Count, Convert.ToDecimal(row.Price));

                    Console.Clear();
                    Console.WriteLine($"{row.ProductName} {row.Count} * " + $"{row.Price} = {row.TotalPrice}");
                }
            }
        }
        private void SaveNewProductToLIst(Products currentProduct, AllReceipts allReceipt, int numberOfProducts)
        {
            currentProduct.Count = numberOfProducts;
            currentProduct.TotalPrice = currentProduct.CalculateTotalPriceSingleProduct(currentProduct.Count,
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
            Console.WriteLine("PAY");
            Console.Write("Kommando:");
        }
        public bool TryUserInputNumbers(string uInput)
        {
            if (uInput == null || Convert.ToInt32(uInput) < 0)
            {
                return false;
            }
            else return true;
        }
    }
}
