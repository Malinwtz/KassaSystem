using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystem
{
    public class SingleReceipt
    {
        private string _productID;
        private string _productName;
        private string _productUnit;
        private decimal _price;
        private decimal _totalPrice;
        private int _count; //   private int _serialNumber;

        public SingleReceipt(string productID, string productName, string productUnit, decimal price, 
            decimal totalPrice, int count)
        {
            _productID = productID;
            _productName = productName;
            _productUnit = productUnit;
            _price = price;
            _totalPrice = totalPrice;
            _count = count;
        }
        public string ProductID
        { get { return _productID; } }  
        public string ProductName
            { get { return _productName; } }
        public string ProductUnit
            { get { return _productUnit; } }
        public decimal Price
            { get { return _price; } }
        public decimal TotalPrice
        { get { return _totalPrice; } }
        private int Count
            { get { return _count; } }

    }
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

        public void AddToListOfSingleReceipts(string productID, string productName, string productUnit, 
            decimal price, decimal totalPrice, int count)
        {
            //---LÄGG TILL BANAN * FLERA ISTÄLLET FÖR PÅ FLER OLIKA RADER - SKRIV OM KVITTOT
            //---MÅSTE SPARAS EN COUNT PÅ KVITTORADEN. OM PRODUKTEN FINNS I LISTAN, ÄNDRA COUNT+1 I KVITTORADEN. 
            //---ÄNDRA VALD RAD I FILEN

            if (_listOfSingleReceipts.Count < 1)
            {
                _listOfSingleReceipts.Add(new SingleReceipt(productID, productName, productUnit,
                      price, totalPrice, count));
            }
            else 
            {
                foreach (var row in _listOfSingleReceipts.ToList())
                {
                    //OM PRODUCTID FINNS I LISTAN, RÄKNA +1
                    if (row.ProductID == productID)
                        count++;
                    //ANNARS ADDERA NY SINGLERECEIPT TILL LISTAN 
                    else
                        _listOfSingleReceipts.Add(new SingleReceipt(productID, productName, productUnit,
                            price, totalPrice, count));
                }
            }
        }
    }
}
