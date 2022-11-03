﻿using System;
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
        private decimal _discountPrice;
        private string _discountStartDate;
        private string _discountEndDate;
        private int _saldo;  
        public Products() 
        {

        }
        public Products(string productID, string productName, string productUnit, decimal productPrice,
            decimal totalPrice = 0, decimal discountPrice = 0, string discountStartDate = null, 
            string discountEndDate = null, int saldo = 0) 
        {
            _productID = productID;
            _productName = productName;
            _productUnit = productUnit;
            _productPrice = productPrice;
            _totalPrice = totalPrice;
            _discountPrice = discountPrice;
            _discountStartDate = discountStartDate;
            _discountEndDate = discountEndDate;
            _saldo = saldo;
        }
        public string ProductID 
            { get { return _productID; } set { _productID = value; } }
        public string ProductName
        {
            get { return _productName; }
            set 
            {
                if (ProductName.Length < 3 || string.IsNullOrEmpty(ProductName) == true)
                    throw new ArgumentException("Felaktigt produktnamn");

                _productName = value; 
            }
        }
        public string ProductUnit 
        { 
            get { return _productUnit; } 
            set 
            {
                if (ProductUnit.Length < 2 || string.IsNullOrEmpty(ProductUnit) == true)
                    throw new ArgumentException("Felaktig produktenhet");
                _productUnit = value; 
            } 
        }
        public decimal ProductPrice
        {
            get { return _productPrice; } 
            set 
            { 
                if (ProductPrice < 0)
                    throw new ArgumentException("Felaktigt pris");  
                _productPrice = value;
            }
        }
        public decimal TotalPrice
        {
            get { return _totalPrice; } set { _totalPrice = value; } 
        }
        public int Count
        {
            get { return _count; } set { _count = value; }
        }
        public string DiscountStartDate
        {
            get { return _discountStartDate; } 
            set { _discountStartDate = value; }
        }
        public string DiscountEndDate
        {
            get { return _discountEndDate; } 
            set { _discountEndDate = value; }
        }
        public decimal DiscountPrice
        {
            get { return _discountPrice; } 
            set 
            {
                if (DiscountPrice < 0)
                    throw new ArgumentException("Felaktigt pris");
                _discountPrice = value; 
            }
        }
        public int Saldo
        { get { return _saldo; } set { _saldo = value; } }

        public Products FindProductFromProductID(List<Products> allProducts, string prod)
        {
            foreach (var product in allProducts)
            {
                if (product.ProductID.ToLower() == prod.ToLower())
                    return product;
            }
            return null;
        }
        
        public Products CheckIfDiscount(string id)
        {
            Admin admin = new();

            var product = admin.FindProductWithId(id);
            if (product != null)
            {
                var endDate = Convert.ToDateTime(product.DiscountEndDate); //object reference not set to an instance of an object/product was null
                var startDate = Convert.ToDateTime(product.DiscountStartDate);
                var days = Convert.ToInt32((endDate - startDate).TotalDays);
                for (var i = 0; i <= days; i++)
                {
                    var addedDays = startDate.AddDays(i);
                    if (addedDays.ToString("yy-MM-dd") == DateTime.Today.ToString("yy-MM-dd"))
                    {
                        product.ProductPrice = product.DiscountPrice;

                        return product;
                    }
                }
            }
            return product;
        }
        public decimal CalculateTotalPriceSingleProduct(int numberOfProducts, decimal price)
        {
            var totalPrice = price * numberOfProducts;
            return totalPrice;
        }
    }
}

