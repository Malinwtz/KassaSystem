using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystem
{
    public class AllReceipts
    {
        private List<SingleReceipt> _listOfSingleReceipts = new List<SingleReceipt>();
        
        public AllReceipts()
        {

        }

        public List<SingleReceipt> ListOfSingleReceipts
        { 
            get { return _listOfSingleReceipts; } 
            set { _listOfSingleReceipts = value; }        
        }
        //metod getTotal returnerar pris * antal
        public void AddToListOfSingleReceipts(string productID, string productName, string productUnit, 
            decimal price, decimal totalPrice, int count)
        {
            //---LÄGG TILL BANAN * FLERA ISTÄLLET FÖR PÅ FLER OLIKA RADER - SKRIV OM KVITTOT
             
           //---DET BLIR TVÅ RADER MED PRODUKTER I SINGLERECEIPT

            if (_listOfSingleReceipts.Count < 1)
            {
                _listOfSingleReceipts.Add(new SingleReceipt(productID, productName, productUnit,
                      price, totalPrice, count));
            }
            else 
            {
                foreach (var product in _listOfSingleReceipts.ToList())
                {
                    //OM PRODUCTID FINNS I LISTAN, RÄKNA +1 OCH ÄNDRA I LISTAN
                    if (product.ProductID == productID)
                        break;
                    //    product.Count++; //---ÄNDRA befintlig produkt I LISTAN

                    //ANNARS ADDERA NY SINGLERECEIPT TILL LISTAN 
                    else
                        _listOfSingleReceipts.Add(new SingleReceipt(productID, productName, productUnit,
                            price, totalPrice, count));
                }
            }
        }

        public decimal CalculateTotal()
        {   
            decimal total = 0;
            foreach (var row in _listOfSingleReceipts)
            {
                total += row.TotalPrice;   
            }
            return total;
        }
        public bool IsListContaining(Products product)
        {
            foreach (var row in _listOfSingleReceipts.ToList())
            {
                if (row.ProductID == product.ProductID)
                {
                    return true;
                }
            }
            return false;
        }
        public void ShowListOfProducts()
        {
            foreach (var row in _listOfSingleReceipts)
            {
                Console.WriteLine($"{row.ProductName} {row.Count} * {row.Price} " +
                    $"= {row.Price * row.Count}kr");
            }
        }
    }
}
