namespace ParkingLot;

public interface IPaymentProcessor
{
    bool ProcessPayment(string licensePlate, TimeSpan duration);
}

public class DefaultPaymentProcessor : IPaymentProcessor
{
    private readonly decimal _hourlyRate = 5.0m;

    private decimal CalculateFee(TimeSpan duration)
    {
        return (decimal)Math.Ceiling(duration.TotalHours) * _hourlyRate;
    }

    public bool ProcessPayment(string licensePlate, TimeSpan duration)
    {
        var fee = CalculateFee(duration);
        // In real implementation, this would integrate with a payment gateway
        Console.WriteLine($"Processing payment of ${fee} for vehicle {licensePlate}");
        Thread.Sleep(1000);
        var responses = new bool[] { true, false };
        var result = responses[new Random().Next(0, 2)];
        return result;
    }
}

