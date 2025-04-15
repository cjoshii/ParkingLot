namespace ParkingLot;

public interface ISpace
{
    int Id { get; }

    SpaceType SpaceType { get; }

    bool Occupied { get; set; }

    string? LicensePlate { get; set; }

    event EventHandler<SpaceEventArgs> SpaceEvent;

    void ToggleSpace(string plate, bool Occupy);
}

public class Space : ISpace
{
    public int Id { get; private set; }

    public SpaceType SpaceType { get; private set; }

    public bool Occupied { get; set; }

    public string? LicensePlate { get; set; }

    public event EventHandler<SpaceEventArgs>? SpaceEvent;

    public Space(int id, SpaceType type)
    {
        this.Id = id;
        this.SpaceType = type;
    }

    public void ToggleSpace(string plate, bool Occupy)
    {
        if (Occupy)
        {
            OnSpaceEvent(new SpaceEventArgs(SpaceEventType.Occupied, plate));
        }
        else
        {
            OnSpaceEvent(new SpaceEventArgs(SpaceEventType.Vacated, plate));
        }
    }

    private void OnSpaceEvent(SpaceEventArgs args)
    {
        SpaceEvent?.Invoke(this, args);
    }
}

public class SpaceEventArgs : EventArgs
{
    public SpaceEventType SpaceEventType { get; private set; }
    public string LicensePlate { get; set; }

    public SpaceEventArgs(SpaceEventType eventType, string plate)
    {
        this.SpaceEventType = eventType;
        this.LicensePlate = plate;
    }
}

public enum SpaceEventType
{
    Occupied,
    Vacated
}

public enum SpaceType
{
    Standard,
    Reserved
}