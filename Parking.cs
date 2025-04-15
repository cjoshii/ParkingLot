namespace ParkingLot;

public interface IParking
{
    public IList<ILevel> Levels { get; }

    public void AddLevel(ILevel level);
}

public class Parking : IParking
{
    //This is thread safe
    private static readonly Lazy<IParking> _instance = new(() => new Parking());

    private Parking()
    {
        Levels = new List<ILevel>();
    }
    public static IParking Instance => _instance.Value;

    public IList<ILevel> Levels { get; private set; }

    public void AddLevel(ILevel level)
    {
        Levels.Add(level);
    }
}