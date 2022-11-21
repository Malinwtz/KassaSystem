namespace KassaSystem;

public class SingleReceipt
{
    public SingleReceipt(string productID, string productName, string productUnit, decimal price,
        decimal totalPrice, int count)
    {
        ProductID = productID;
        ProductName = productName;
        ProductUnit = productUnit;
        Price = price;
        TotalPrice = totalPrice; //räkna varje gång ist för att undvika dubbellagring
        Count = count;
    }

    public string ProductID { get; }

    public string ProductName { get; }

    public string ProductUnit { get; }

    public decimal Price { get; }

    public decimal TotalPrice { get; set; }

    public int Count { get; set; }
}