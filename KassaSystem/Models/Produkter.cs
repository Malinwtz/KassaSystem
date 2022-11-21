namespace KassaSystem.Models;

public class Products
{
    private decimal _discountPrice;
    private string _productName;
    private decimal _productPrice;
    private string _productUnit;


    public Products()
    {
    }

    public Products(string productID, string productName, string productUnit, decimal productPrice,
        decimal totalPrice = 0, decimal discountPrice = 0, string discountStartDate = null,
        string discountEndDate = null, int saldo = 0)
    {
        ProductID = productID;
        _productName = productName;
        _productUnit = productUnit;
        _productPrice = productPrice;
        TotalPrice = totalPrice;
        _discountPrice = discountPrice;
        DiscountStartDate = discountStartDate;
        DiscountEndDate = discountEndDate;
        Saldo = saldo;
    }

    public string ProductID { get; set; }

    public string ProductName
    {
        get => _productName;
        set
        {
            if (ProductName.Length < 1 || string.IsNullOrEmpty(ProductName))
                throw new ArgumentException("Felaktigt produktnamn");

            _productName = value;
        }
    }

    public string ProductUnit
    {
        get => _productUnit;
        set
        {
            if (ProductUnit.Length < 2 || string.IsNullOrEmpty(ProductUnit))
                throw new ArgumentException("Felaktig produktenhet");
            _productUnit = value;
        }
    }

    public decimal ProductPrice
    {
        get => _productPrice;
        set
        {
            if (ProductPrice < 0)
                throw new ArgumentException("Felaktigt pris");
            _productPrice = value;
        }
    }

    public decimal TotalPrice { get; set; }

    public int Count { get; set; }

    public string DiscountStartDate { get; set; }

    public string DiscountEndDate { get; set; }

    public decimal DiscountPrice
    {
        get => _discountPrice;
        set
        {
            if (DiscountPrice < 0)
                throw new ArgumentException("Felaktigt pris");
            _discountPrice = value;
        }
    }

    public int Saldo { get; set; }

    public Products CheckIfDiscount(string id)
    {
        Admin admin = new();
        var product = admin.FindProductWithId(id);
        if (product != null)
        {
            var endDate = Convert.ToDateTime(product.DiscountEndDate);
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