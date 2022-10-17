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
        private int _count;
        private decimal _totalAmoutAllProducts;

        public SingleReceipt(string productID, string productName, string productUnit, decimal price,
            decimal totalPrice, int count, decimal totalAmoutAllProducts)
        {
            _productID = productID;
            _productName = productName;
            _productUnit = productUnit;
            _price = price;
            _totalPrice = totalPrice;
            _count = count;
            _totalAmoutAllProducts = totalAmoutAllProducts; 
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
        public int Count
        { 
            get { return _count; }
            set { _count = value; }
        }
        public decimal TotalAmoutAllProducts
        { 
            get { return _totalAmoutAllProducts; }
            set { _totalAmoutAllProducts = value; }
        }

    }

}
