using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystem
{
    public class ReceiptRow
    {
        private string _productID;
        private string _productName;
        private string _productUnit;
        private decimal _price;
        private int _count;

        public ReceiptRow(string productID, string productName, string productUnit, decimal price, int count)
        {
            _productID = productID;
            _productName = productName;
            _productUnit = productUnit;
            _price = price;
            _count = count;
        }
        public string ProductID
        { get { return _productID; } }  
    }
    internal class Receipt
    {
        private List<ReceiptRow> _receiptRows= new List<ReceiptRow>();
        private int _serialNumber;
        public void AddToReceipt(string productID, string productName, string productUnit, decimal price, int count)
        {
            foreach(ReceiptRow row in _receiptRows)
            {
                if (row.ProductID == productID ) 
                        count++;
                else
                    _receiptRows.Add(new ReceiptRow(productID, productName, productUnit, price, count));
            }
        }
        
        public Receipt()
        {
            
        }
        
    }
}
