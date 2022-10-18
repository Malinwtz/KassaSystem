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
        //public void AddToListOfSingleReceipts(string productID, string productName, string productUnit, 
        //    decimal price, decimal totalPrice, int count)
        //{   
        //        _listOfSingleReceipts.Add(new SingleReceipt(productID, productName, productUnit,
        //              price, totalPrice, count));
        //}
        public void AddToListOfSingleReceipts(Products product)
        {   
            _listOfSingleReceipts.Add(new SingleReceipt(product.ProductID, product.ProductName, 
                product.ProductUnit, product.ProductPrice, product.TotalPrice, product.Count));
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
        public void WriteTotalAmount()
        {
            Console.WriteLine($"Total: {CalculateTotal()}kr" + Environment.NewLine);
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
