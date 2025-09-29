public interface IDiscount
{
    double Calculate(double discount);
}

public class regularDisc : IDiscount
{
    public double Calculate(double amount) => amount * 0.1;
}

public class PremiumDisc : IDiscount
{
    public double Calculate(double amount) => amount * 0.2;
}

public class DiscCalculator
{
    private readonly IDiscount _discount;

    public DiscCalculator(IDiscount discount)
    {
        _discount = discount;
    }

    public double GetDisc(double amount) => _discount.Calculate(amount);
}

class Program
{
    static void Main()
    {
        DiscCalculator calc = new DiscCalculator(new PremiumDisc());
        calc.GetDisc(10000);

        calc = new DiscCalculator(new regularDisc());
        calc.GetDisc();
    }
}