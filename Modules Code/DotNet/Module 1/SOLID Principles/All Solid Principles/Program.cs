using System;
using System.Collections.Generic;
using System.Linq;

//SRP: Product class only manages product data
public class Product
{
    public string Name { get; set; }
    public double Price { get; set; }
}

// SRP: Cart class only manages cart operations
public class Cart
{
    private readonly List<Product> _products = new List<Product>();

    public void AddProduct(Product product) => _products.Add(product);

    public double GetTotalPrice()
    {
        return _products.Sum(p => p.Price);
    }

    public List<Product> GetProducts() => _products;
}

// OCP: Payment is open for extension
public interface IPayment
{
    void Pay(double amount);
}

public class CreditCardPayment : IPayment
{
    public void Pay(double amount) => Console.WriteLine($"Paid {amount} using Credit Card.");
}

public class PaypalPayment : IPayment
{
    public void Pay(double amount) => Console.WriteLine($"Paid {amount} using Paypal.");
}

// LSP: Any IPayment method can be substituted safely
public class Checkout
{
    private readonly IPayment _payment;

    public Checkout(IPayment payment)
    {
        _payment = payment;
    }

    public void ProcessOrder(Cart cart)
    {
        double total = cart.GetTotalPrice();
        _payment.Pay(total);
    }
}

// ISP: Separate small interfaces instead of one fat interface
public interface IReadable
{
    void ReadData();
}

public interface IWriteable
{
    void WriteData(string data);
}

// MySQL supports both read and write
public class MySqlDb : IReadable, IWriteable
{
    public void ReadData() => Console.WriteLine("Reading data from MySQL");

    public void WriteData(string data) => Console.WriteLine($"Writing '{data}' into MySQL");
}

// MongoDB only supports Write for simplicity
public class MongoDb : IWriteable
{
    public void WriteData(string data) => Console.WriteLine($"Writing '{data}' into MongoDB");
}

// DIP: DataService depends on abstraction, not concrete class
public class DataService
{
    private readonly IWriteable _database;

    public DataService(IWriteable database)
    {
        _database = database;
    }

    public void SaveOrder(string orderData)
    {
        _database.WriteData(orderData);
    }
}

// Putting It All Together
class Program
{
    static void Main()
    {
        // SRP: Create products
        var p1 = new Product { Name = "Laptop", Price = 50000 };
        var p2 = new Product { Name = "Mouse", Price = 1500 };

        // SRP: Add products to cart
        var cart = new Cart();
        cart.AddProduct(p1);

        // OCP + LSP: Choose payment method
        var checkout = new Checkout(new PaypalPayment());
        checkout.ProcessOrder(cart);

        // ISP + DIP: Save order into MySQL
        var dataService = new DataService(new MySqlDb());
        dataService.SaveOrder("Order saved successfully!");
    }
}