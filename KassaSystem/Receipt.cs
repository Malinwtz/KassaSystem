using KassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        
        public void AddToListOfSingleReceipts(Products product)
        {
            _listOfSingleReceipts.Add(new SingleReceipt(product.ProductID, product.ProductName,
                product.ProductUnit, product.ProductPrice, product.TotalPrice, product.Count));
        }
        private int GetNumberOfReceipt()
        {
            var fileName = "Receipt number.txt";
            if (File.Exists(fileName))
            {
                var lastLine = File.ReadLines(fileName).Last();
                var number = Convert.ToInt32(lastLine);
                number++;
                File.AppendAllText(fileName, number + Environment.NewLine);
                return number;
            }
            else
            {
                File.AppendAllText(fileName, 1 + Environment.NewLine);
                return 1;
            }
        }
        public void SaveToReceipt()
        {   
            var fileName = DateTime.Now.ToString("RECEIPT_yyy-MM-dd") + ".txt";
            var lastNumber = GetNumberOfReceipt();
            var numberLine = $"KVITTO NR {lastNumber}";
            File.AppendAllText(fileName, Environment.NewLine);
            File.AppendAllText(fileName, numberLine + Environment.NewLine); 

            foreach (var row in ListOfSingleReceipts)
            {
                var line = $"{row.ProductName} {row.Count} * {row.Price} = {row.Price * row.Count}kr";
                File.AppendAllText(fileName, line + Environment.NewLine);     
            }
            var total = $"Total: {Convert.ToString(CalculateTotal())}kr";
            File.AppendAllText(fileName, total + Environment.NewLine);
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
        public void ShowTotalAmount()
        {
            Console.WriteLine($"Total: {(CalculateTotal()).ToString("F", CultureInfo.InvariantCulture)}kr" 
                + Environment.NewLine);
        }
        public bool IsListContaining(Products product)
        {
            var result = _listOfSingleReceipts.FirstOrDefault(p => p.ProductID == product.ProductID);
            if (result != null) return true;
            else return false;
        }
        public void ShowListOfProducts()
        { 
            foreach (var row in _listOfSingleReceipts)
            {   
                Console.WriteLine($"{row.ProductName} {row.Count} * " +
                    $"{row.Price.ToString("F", CultureInfo.InvariantCulture)} " +
                    $"= {(row.Price * row.Count).ToString("F", CultureInfo.InvariantCulture)}kr");
            }
        }
        public void SaveDateToProductNameFile()
        {
            foreach (var row in ListOfSingleReceipts)
            {
                var filename = row.ProductName + ".txt";
                for (int i = 0; i < row.Count; i++)
                {
                    File.AppendAllText(filename, $"{DateTime.Now:yyyy-MM-dd}" + Environment.NewLine);
                }
            }
        }
        public void ChangeCountToProductsTextFile()
        {
            Admin admin = new();
            AllReceipts allReceipts = new();
            File.Delete("Products.txt");

            foreach (var row in admin.ListOfProducts) 
            {
                int count;  
                var result = _listOfSingleReceipts.FirstOrDefault(p => p.ProductID == row.ProductID);
                
                if (result != null) count = result.Count;
                else count = 0;

                var product = admin.FindProductWithId(row.ProductID);
                var line = $"{row.ProductID};{row.ProductName};{row.ProductUnit};{row.ProductPrice};" +
                    $"{row.DiscountPrice};{row.DiscountStartDate};{row.DiscountEndDate};" +
                    $"{product.Saldo - count}";
                File.AppendAllText("Products.txt", line + Environment.NewLine);
            }
        }
        
    }
}
/*
public void WriteProductsWithDiscount()
{
foreach (var product in _listOfProducts)
            {
                Console.WriteLine($"{product.ProductID};{product.ProductName};{product.ProductUnit};" +
                    $"{product.ProductPrice}kr");
                if (product.DiscountPrice > 0) //---läs från lista
                {
                    Console.WriteLine($"  *KAMPANJPRIS: {product.DiscountPrice}kr; {product.DiscountStartDate:yyyy-MM-dd}" +
                        $" - {product.DiscountEndDate:yyyy-MM-dd}");
                }
            }
            Console.WriteLine(Environment.NewLine);
 }
 */
