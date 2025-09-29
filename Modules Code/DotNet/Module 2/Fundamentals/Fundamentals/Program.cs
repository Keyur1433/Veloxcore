//Create a Car class with properties Brand, Model, and Year. Then create an object of the class and display its details.

class Car
{
    private string? _brand;
    private string? _model;
    private int _year;

    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
}

class Program
{
    public static void Main()
    {
        Car car = new()
        {
            Brand = "Tata",
            Model = "Nexon",
            Year = 2000
        };

        Console.WriteLine(car.Brand);
    }
}

//Create a Book class with fields like Title, Author, Price. Add a method DiscountPrice() that applies a 10% discount and returns the new price.
class Book
{
    private string? _title;
    private double _price;

    public string? Title { get; set; }
    public double Price { get; set; }

    public double DiscountPrice(double price)
    {
        return _price - (price * 0.1);
    }
}