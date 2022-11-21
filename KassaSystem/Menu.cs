namespace KassaSystem;

internal class Menu
{
    public static int CashMenu()
    {
        Console.Clear();
        Console.WriteLine("KASSA" + Environment.NewLine);
        Console.WriteLine("1.Ny kund");
        Console.WriteLine("2.Administreringsverktyg");
        Console.WriteLine("0.Avsluta" + Environment.NewLine);
        var sel = ReturnFromMenu(0, 2);
        return sel;
    }

    public static int AdminMenu()
    {
        Console.WriteLine("ADMINISTRERINGSMENY" + Environment.NewLine);
        Console.WriteLine("1. Skapa ny produkt");
        Console.WriteLine("2. Ta bort eller ändra produkt");
        Console.WriteLine("3. Visa produkter och kampanjpriser");
        Console.WriteLine("4. Visa försäljningsstatistik");
        Console.WriteLine("0. Tillbaka till huvudmenyn" + Environment.NewLine);
        var sel = ReturnFromMenu(0, 4);
        return sel;
    }

    public static int ChangeProductMenu()
    {
        Console.Clear();
        Console.WriteLine("ÄNDRA PRODUKT" + Environment.NewLine);
        Console.WriteLine("1. Ta bort");
        Console.WriteLine("2. Ändra namn");
        Console.WriteLine("3. Ändra ID");
        Console.WriteLine("4. Ändra enhet");
        Console.WriteLine("5. Ändra pris");
        Console.WriteLine("0. Tillbaka till huvudmenyn" + Environment.NewLine);
        var sel = ReturnFromMenu(0, 5);
        return sel;
    }

    public static int ChangePriceMenu()
    {
        Console.Clear();
        Console.WriteLine("ÄNDRA PRIS" + Environment.NewLine);
        Console.WriteLine("1. Skriv in ett nytt pris");
        Console.WriteLine("2. Skriv in ett kampanjpris");
        Console.WriteLine("0. Tillbaka till huvudmenyn" + Environment.NewLine);
        var sel = ReturnFromMenu(0, 2);
        return sel;
    }

    public static int ReturnFromMenu(int min, int max)
    {
        var sel = -1;
        while (true)
        {
            try
            {
                sel = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Felaktig input");
            }

            if (sel >= min && sel <= max) return sel;
            Console.WriteLine("Felaktig input");
        }
    }
}