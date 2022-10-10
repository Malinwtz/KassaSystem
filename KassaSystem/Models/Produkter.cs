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
        private int productUnit;
        private decimal productPrice;
        public Products(string productID, string productName, int productUnit, decimal productPrice)
        {
            this.productID = productID;
            this.productName = productName;
            this.productUnit = productUnit;
            this.productPrice = productPrice;
        }
        public string ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
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
        public int ProductUnit
        {
            get { return productUnit; }
            set
            {
                if (productUnit < 0)
                    throw new ArgumentException("Felaktig enhet");
                productUnit = value;
            }
        }
        public decimal ProductPrice
        {
            get { return productPrice; }
            set
            {
                if (productPrice < 0)
                    throw new ArgumentException("Felaktigt pris");  
                productPrice = value;
            }
        }

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

