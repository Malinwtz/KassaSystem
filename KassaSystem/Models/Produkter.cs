using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KassaSystem.Models
{  
    public class Products
    {
        private string _productID;
        private string _productName;
        private string _productUnit;
        private decimal _productPrice;
        private decimal _totalPrice;
        private int _count;
        //CONSTRUCTORS
        public Products() 
        {

        }
        public Products(string productID, string productName, string productUnit, decimal productPrice, decimal totalPrice /*, int count*/) 
        {
            _productID = productID;
            _productName = productName;
            _productUnit = productUnit;
            _productPrice = productPrice;
            _totalPrice = totalPrice;
      //      _count = count; 
        }
        //PROPERTIES
        public string ProductID 
            { get { return _productID; } set { _productID = value; } }
        public string ProductName
        {
           get { return _productName; }
            set 
            {
                if (ProductName.Length < 2 || string.IsNullOrEmpty(ProductName) == true)
                    throw new ArgumentException("Felaktigt inskrivet namn");

                _productName = value; 
            }
        }
        public string ProductUnit 
            { get { return _productUnit; } set { _productUnit = value; } }
        public decimal ProductPrice
        {
            get { return _productPrice; }
            set { _productPrice = value;}
        }
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; } 
        }
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        //METHODS
        public Products FindProductFromProductID(List<Products> allProducts, string prod)
        {
            foreach (var product in allProducts)
            {
                if (product.ProductID.ToLower() == prod.ToLower())
                    return product;
            }
            return null;
        }
    }
}

