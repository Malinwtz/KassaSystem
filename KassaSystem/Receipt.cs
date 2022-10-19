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
