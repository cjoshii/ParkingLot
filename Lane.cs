namespace ParkingLot;

public interface ILane
{
    LaneType LaneType { get; }

    event EventHandler<LaneEventArgs>? LaneEvent;

    void Transit(string plate);
    bool Validate(string plate);
}

public class Lane : ILane
{
    public LaneType LaneType { get; private set; }

    public event EventHandler<LaneEventArgs>? LaneEvent;

    public Lane(LaneType type)
    {
        this.LaneType = type;
    }

    public void Transit(string plate)
    {
        OnLaneEvent(new LaneEventArgs(plate, LaneEventType.Transit));
    }

    public bool Validate(string plate)
    {

        var eventArgs = new LaneEventArgs(plate, LaneEventType.Validation);
        OnLaneEvent(eventArgs);
        if (!eventArgs.IsValid)
        {
            Console.WriteLine(eventArgs.Message);
        }
        return eventArgs.IsValid;
    }

    private void OnLaneEvent(LaneEventArgs args)
    {
        LaneEvent?.Invoke(this, args);
    }
}

public class LaneEventArgs : EventArgs
{
    public string LicensePlate { get; private set; }
    public LaneEventType LaneEventType { get; private set; }
    public bool IsValid { get; set; }
    public string? Message { get; set; }

    public LaneEventArgs(string licensePlate, LaneEventType eventType)
    {
        this.LicensePlate = licensePlate;
        this.LaneEventType = eventType;
        this.IsValid = true;
    }
}

public enum LaneType
{
    Entry,
    Exit
}

public enum LaneEventType
{
    Validation,
    Transit
}