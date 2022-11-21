namespace KassaSystem;

public class Statistics
{
    public Statistics(string name, int count)
    {
        Name = name;
        Count = count;
    }

    public string Name { get; set; }
    public int Count { get; set; }

    public void SalesStatistics()
    {
        Console.Clear();
        Console.WriteLine("FÖRSÄLJNINGSSTATISTIK");
        var statProductList = new List<Statistics>();

        Console.WriteLine("Skriv in startdatum: ");
        var start = ErrorHandling.TryDate();
        Console.WriteLine("Skriv in slutdatum: ");
        var end = ErrorHandling.TryDate();

        var ts = Convert.ToInt32((end - start).TotalDays);
        Admin adm = new Admin();

        foreach (var p in adm.ListOfProducts)
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
        //Console.WriteLine(Environment.NewLine);
        Console.WriteLine($"Från och med: {start:yyyy-MM-dd} Tom:{end:yyyy-MM-dd}");
        var orderedList = statProductList.OrderByDescending(p => p.Count);
        foreach (var stat in orderedList)
            Console.WriteLine($"{stat.Name} {stat.Count}");

        Console.ReadKey();
    }
}