﻿using KassaSystem.Models;

namespace KassaSystem;

internal class Admin
{
    public Admin()
    {
        ListOfProducts = MakeListOfTextProducts();
    }

    public List<Products> ListOfProducts { get; set; } = new();

    public void CreateNewProduct()
    {
        Console.Clear();
        Console.WriteLine("SKAPA NY PRODUKT" + Environment.NewLine);
        Console.WriteLine("PRODUKTLISTA:");

        foreach (var p in ListOfProducts.OrderBy(a => a.ProductID))
            Console.WriteLine($"{p.ProductID};{p.ProductName};{p.ProductUnit}; " +
                              $"{p.ProductPrice}");

        Console.WriteLine(Environment.NewLine + "SKAPA NY PRODUKT");
        Console.Write("ProduktID: ");
        var newId = CheckIfIdExists();
        Console.Write("Namn: ");
        var newName = ErrorHandling.TryName();
        Console.Write("Enhet (kilopris/styckpris) : ");
        var newUnit = ErrorHandling.TryUnit();
        Console.Write("Pris: ");
        var newPrice = ErrorHandling.TryPrice();
        Console.Write("Skriv in saldo: ");
        var newSaldo = ErrorHandling.TryInt();

        var product = new Products(newId, newName, newUnit, newPrice, 0, 0, null, null, newSaldo);
        product.DiscountPrice = 0;
        product.DiscountStartDate = "0001-01-01";
        product.DiscountEndDate = "0001-01-01";
        AddToFile(product);

        Console.WriteLine("Ny produkt sparad" + Environment.NewLine);
    }

    public string CheckIfIdExists()
    {
        while (true)
        {
            var newId = ErrorHandling.TryId();
            var id = FindProductWithId(newId);
            if (id != null) Console.WriteLine(Environment.NewLine + "ID finns redan");
            else return newId;
        }
    }

    public Products FindProductWithId(string id)
    {
        var productId = ListOfProducts
            .FirstOrDefault(p => p.ProductID == id);
        return productId;
    }

    public List<Products> MakeListOfTextProducts()
        // gör en ny Products list// för varje var i Products.txt - spara produkt till listan//RETURN List<Products> (SPARAR ÄVEN SALDO)
    {
        var result = new List<Products>();
        foreach (var line in File.ReadLines("Products.txt"))
        {
            var parts = line.Split(';');
            var prodct = new Products(parts[0], parts[1], parts[2], Convert.ToDecimal(parts[3]), 0,
                Convert.ToDecimal(parts[4]), parts[5], parts[6], Convert.ToInt32(parts[7]));

            result.Add(prodct);
        }

        return result;
    }

    public void ChangeProduct()
    {
        Console.Clear();
        Console.WriteLine("ÄNDRA PRODUKT" + Environment.NewLine);
        ShowProductsWithDiscount();

        Console.Write("Skriv in ID på den produkt du vill ändra: ");
        var selChange = "";

        while (true)
        {
            selChange = ErrorHandling.TryId();
            var id = FindProductWithId(selChange);
            if (id == null) Console.WriteLine( Environment.NewLine + "ID finns inte");
            else break;
        }

        foreach (var row in ListOfProducts.OrderBy(p => p.ProductID).ToList())
            if (row.ProductID.ToLower() == selChange.ToLower())
            {
                var sel3 = Menu.ChangeProductMenu();

                if (sel3 == 1)
                {
                    Console.WriteLine("Tar bort från listan");
                    ListOfProducts.Remove(row);
                }
                else if (sel3 == 2)
                {
                    Console.Write("Skriv in ett nytt namn på produkten: ");
                    row.ProductName = ErrorHandling.TryName();
                    Console.WriteLine("Namn sparat" + Environment.NewLine);
                }
                else if (sel3 == 3)
                {
                    Console.Write("Skriv in ett nytt ID på produkten: ");
                    var newId = CheckIfIdExists();
                    row.ProductID = newId;
                    Console.WriteLine("ID sparat" + Environment.NewLine);
                }
                else if (sel3 == 4)
                {
                    Console.Write("Skriv in en ny enhet på produkten (kilopris/styckpris) : ");
                    row.ProductUnit = ErrorHandling.TryUnit();
                    Console.WriteLine("Enhet sparad" + Environment.NewLine);
                }
                else if (sel3 == 5)
                {
                    DiscountChoice(row);
                }

                else if (sel3 == 0)
                {
                    break;
                }
            }

        File.Delete("Products.txt");

        foreach (var row in ListOfProducts)
        {
            var line = $"{row.ProductID};{row.ProductName};{row.ProductUnit};{row.ProductPrice};" +
                       $"{row.DiscountPrice};{row.DiscountStartDate};{row.DiscountEndDate};{row.Saldo}";
            File.AppendAllText("Products.txt", line + Environment.NewLine);
        }
    }

    private void DiscountChoice(Products row)
    {
        while (true)
        {
            var sel4 = Menu.ChangePriceMenu();

            if (sel4 == 1)
            {
                Console.Write("Skriv in ett nytt pris på produkten (00,00) : ");
                row.ProductPrice = ErrorHandling.TryPrice();
                Console.WriteLine("Pris sparat" + Environment.NewLine);
            }
            else if (sel4 == 2)
            {
                CreateDiscount(row);
                Console.WriteLine(Environment.NewLine + "Kampanjpris sparat");
            }
            else if (sel4 == 0)
            {
                break;
            }
        }
    }

    private void CreateDiscount(Products row)
    {
        Console.Write("Skriv in startdatum för kampanjpriset (yyyy-MM-dd) : ");
        var dateStart = ErrorHandling.TryDate();
        row.DiscountStartDate = dateStart.ToString("yyyy-MM-dd");

        Console.Write("Skriv in slutdatum för kampanjpriset (yyyy-MM-dd) : ");
        var dateEnd = ErrorHandling.TryDate();
        row.DiscountEndDate = dateEnd.ToString("yyyy-MM-dd");

        while (true)
            try
            {
                Console.Write("Skriv in kampanjpris: ");
                var discount = Convert.ToDecimal(Console.ReadLine());
                row.DiscountPrice = discount;
                break;
            }
            catch
            {
                ErrorHandling.ErrorMessage();
            }
    }

    public void ShowProductsWithDiscount()
    {
        Console.WriteLine("PRODUKTLISTA");
        //ReadProductsFromTextFile(); 

        foreach (var product in ListOfProducts.OrderBy(p => p.ProductID))
        {
            Console.WriteLine($"{product.ProductID};{product.ProductName};{product.ProductUnit};" +
                              $"{product.ProductPrice}kr;{product.Saldo}st i lager");
            if (product.DiscountPrice > 0)
                Console.WriteLine(
                    $"  *KAMPANJPRIS: {product.DiscountPrice}kr; From {product.DiscountStartDate:yyyy-MM-dd}" +
                    $" Tom {product.DiscountEndDate:yyyy-MM-dd}*");
        }

        Console.WriteLine(Environment.NewLine);
    }

    public void ReadProductsFromTextFile()
        //läser från products.txt//för varje rad i den return. listan - spara till _listofproducts// VOID (SPARAR INTE SALDO)
    {
        var productList = File.ReadAllLines("Products.txt").ToList();

        foreach (var row in productList)
        {
            row.ToString();
            var rowArray = row.Split(';');
            ListOfProducts.Add(new Products(rowArray[0], rowArray[1],
                rowArray[2], Convert.ToDecimal(rowArray[3]), 0, Convert.ToDecimal(rowArray[4]),
                rowArray[5], rowArray[6]));
        }
    }

    private void AddToFile(Products product)
    {
        var line = $"{product.ProductID};{product.ProductName};{product.ProductUnit};{product.ProductPrice}" +
                   $";{product.DiscountPrice};{product.DiscountStartDate};{product.DiscountEndDate};{product.Saldo}";
        File.AppendAllText("Products.txt", line + Environment.NewLine);
    }

    public void AdminOptions()
    {
        var admin = new Admin();
        Console.Clear();
        var sel2 = Menu.AdminMenu();

        if (sel2 == 1)
        {
            admin.CreateNewProduct();
        }
        else if (sel2 == 2)
        {
            admin.ChangeProduct();
        }
        else if (sel2 == 3)
        {
            admin.ShowProductsWithDiscount();
            Console.ReadKey();
        }
        else if (sel2 == 4)
        {
            admin.SalesStatistics();
        }
        else if (sel2 == 0)
        {
            Console.Clear();
        }
    }

    public void SalesStatistics()
    {
        Console.Clear();
        Console.WriteLine("FÖRSÄLJNINGSSTATISTIK" + Environment.NewLine);
        var statProductList = new List<Statistics>();

        Console.Write("Skriv in startdatum (yyyy-MM-dd) : ");
        var start = ErrorHandling.TryDate();
        Console.Write("Skriv in slutdatum (yyyy-MM-dd) : ");
        var end = ErrorHandling.TryDate();

        var ts = Convert.ToInt32((end - start).TotalDays);

        foreach (var p in ListOfProducts)
            if (File.Exists(p.ProductName + ".txt"))
            {
                var list = File.ReadAllLines(p.ProductName + ".txt").ToList();
                //       var dates = productList.Where(a => a.Convert.ToTimespan == ts );
                var count = 0;
                foreach (var row in list)
                    for (var i = 0; i <= ts; i++)
                    {
                        var addedDays = start.AddDays(i);
                        var parts = addedDays.ToString().Split(' ');

                        if (parts[0] == row) count++;
                    }

                statProductList.Add(new Statistics(p.ProductName, count));
            }

        Console.Clear();
        Console.WriteLine($"Från och med: {start:yyyy-MM-dd} Tom:{end:yyyy-MM-dd}");
        var orderedList = statProductList.OrderByDescending(p => p.Count);
        foreach (var stat in orderedList)
            Console.WriteLine($"{stat.Name} {stat.Count}");

        Console.ReadKey();
    }
}


//public void DeleteProduct()
//{
//    var fileName = "Products.txt";
//    File.WriteAllLines(fileName,
//        File.ReadLines(fileName).Where(l => l != "removeme").ToList());
//    //readfromfile

//    //ta bort objekt från fil
//}