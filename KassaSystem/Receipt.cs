using KassaSystem.Models;
using System;
using System.Collections.Generic;
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
        //metod getTotal returnerar pris * antal
        public void AddToListOfSingleReceipts(string productID, string productName, string productUnit, 
            decimal price, decimal totalPrice, int count, decimal allTotal)
        {
            //---LÄGG TILL BANAN * FLERA ISTÄLLET FÖR PÅ FLER OLIKA RADER - SKRIV OM KVITTOT
             
           //---DET BLIR TVÅ RADER MED PRODUKTER I SINGLERECEIPT

            if (_listOfSingleReceipts.Count < 1)
            {
                _listOfSingleReceipts.Add(new SingleReceipt(productID, productName, productUnit,
                      price, totalPrice, count, allTotal));
            }
            else 
            {
                foreach (var product in _listOfSingleReceipts.ToList())
                {
                    //OM PRODUCTID FINNS I LISTAN, RÄKNA +1 OCH ÄNDRA I LISTAN
                    if (product.ProductID == productID)
                        break;
                    //    product.Count++; //---ÄNDRA befintlig produkt I LISTAN

                    //ANNARS ADDERA NY SINGLERECEIPT TILL LISTAN 
                    else
                        _listOfSingleReceipts.Add(new SingleReceipt(productID, productName, productUnit,
                            price, totalPrice, count, allTotal));
                }
            }
        }
    }
}
