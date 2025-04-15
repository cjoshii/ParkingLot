
namespace ParkingLot;

public class ParkingBuilder
{
    private readonly IParking _parking;

    public ParkingBuilder()
    {
        _parking = Parking.Instance;
    }
    public LevelBuilder CreateLevel(int id)
    {
        return new LevelBuilder(this, id);
    }

    public void AddLevel(ILevel level)
    {
        this._parking.AddLevel(level);
    }

    public IParking Build()
    {
        return _parking;
    }
}

