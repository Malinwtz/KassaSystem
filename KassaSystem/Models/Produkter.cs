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
        private DateTime _discountStartDate;
        private DateTime _discountEndDate;
        private decimal _discountPrice;
        //CONSTRUCTORS
        public Products() 
        {

        }
        public Products(string productID, string productName, string productUnit, decimal productPrice, 
            decimal totalPrice) 
        {
            _productID = productID;
            _productName = productName;
            _productUnit = productUnit;
            _productPrice = productPrice;
            _totalPrice = totalPrice;
            _discountStartDate = DateTime.MinValue;
            _discountEndDate = DateTime.MinValue;
            _discountPrice = 0;
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
        { 
            get { return _productUnit; } set { _productUnit = value; } 
        }
        public decimal ProductPrice
        {
            get { return _productPrice; } set { _productPrice = value;}
        }
        public decimal TotalPrice
        {
            get { return _totalPrice; } set { _totalPrice = value; } 
        }
        public int Count
        {
            get { return _count; } set { _count = value; }
        }
        public DateTime DiscountStartDate
        {
            get { return _discountStartDate; } set { _discountStartDate = value; }
        }
        public DateTime DiscountEndDate
        {
            get { return _discountEndDate; } set { _discountEndDate = value; }
        }
        public decimal DiscountPrice
        {
            get { return _discountPrice; } set { _discountPrice = value; }
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
        public void CheckIfDiscount(Products product )
        {
            var days = Convert.ToInt32((product.DiscountEndDate - product.DiscountStartDate).TotalDays);
            for (var i = 0; i < days; i++)
            {
                var addedDays = product.DiscountStartDate.AddDays(i);
                if (addedDays.ToString("yy-MM-dd") == DateTime.Today.ToString("yy-MM-dd"))
                {
                    Console.WriteLine("Ändrar pris");
                    product.ProductPrice = product.DiscountPrice;
                    break;
                }
            }
        }
    }
}

