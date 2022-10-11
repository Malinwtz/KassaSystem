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
        private string productID;
        private string productName;
        private string productUnit;
        private decimal productPrice;
        private decimal totalPrice;
        //CONSTRUCTORS
        public Products() 
        {

        }
        public Products(string productID, string productName, string productUnit, decimal productPrice, decimal totalPrice) 
        {
            this.productID = productID;
            this.productName = productName;
            this.productUnit = productUnit;
            this.productPrice = productPrice;
            this.totalPrice = totalPrice;
        }
        //PROPERTIES
        public string ProductID 
            { get { return productID; } set { productID = value; } }
        public string ProductName
        {
           get { return productName; }
            set 
            {
                if (ProductName.Length < 2 || string.IsNullOrEmpty(ProductName) == true)
                    throw new ArgumentException("Felaktigt inskrivet namn");

                productName = value; 
            }
        }
        public string ProductUnit 
            { get { return productUnit; } set { productUnit = value; } }
        public decimal ProductPrice
        {
            get { return productPrice; }
            set { productPrice = value;}
        }
        public decimal TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; } 
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

