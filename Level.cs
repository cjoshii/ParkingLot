namespace ParkingLot;

public interface ILevel
{
    int Id { get; }
    IList<ISpace> Spaces { get; }
    IList<ILane> Lanes { get; }
    IPaymentProcessor? PaymentProcessor { get; set; }
    IDictionary<string, ParkingEntry> Entries { get; }
    void AddSpace(ISpace space);
    void AddLane(ILane lane);
}

public class Level : ILevel
{
    public IList<ISpace> Spaces { get; private set; }

    public IList<ILane> Lanes { get; private set; }

    public IDictionary<string, ParkingEntry> Entries { get; private set; }

    public int Id { get; private set; }

    public IPaymentProcessor? PaymentProcessor { get; set; }

    public Level(int id)
    {
        this.Id = id;
        Spaces = [];
        Lanes = [];
        Entries = new Dictionary<string, ParkingEntry>();
    }

    public void AddSpace(ISpace space)
    {
        Spaces.Add(space);
        space.SpaceEvent += ProcessSpaceEvent;
    }

    public void AddLane(ILane lane)
    {
        Lanes.Add(lane);
        lane.LaneEvent += ProcessLaneEvent;
    }

    private void ProcessSpaceEvent(Object? sender, SpaceEventArgs args)
    {
        if (sender is ISpace space)
        {
            if (args.SpaceEventType.Equals(SpaceEventType.Occupied))
            {
                space.Occupied = true;
                space.LicensePlate = args.LicensePlate;
                if (Entries.TryGetValue(args.LicensePlate, out var parkingEntry))
                    parkingEntry.ParkingSpaceId = space.Id;
            }
            else
            {
                space.Occupied = false;
                space.LicensePlate = null;
                if (Entries.TryGetValue(args.LicensePlate, out var parkingEntry))
                    parkingEntry.ParkingSpaceId = null;
            }
        }
    }

    private void ProcessLaneEvent(Object? sender, LaneEventArgs args)
    {
        if (sender is ILane lane)
        {
            if (args.LaneEventType.Equals(LaneEventType.Validation))
            {
                if (string.IsNullOrWhiteSpace(args.LicensePlate))
                {
                    args.IsValid = false;
                    args.Message = "License plate is required";
                }
                else
                {
                    if (lane.LaneType.Equals(LaneType.Entry))
                    {
                        if (Entries.ContainsKey(args.LicensePlate))
                        {
                            args.IsValid = false;
                            args.Message = "License plate is already entered";
                            return;
                        }

                        if (Entries.Count.Equals(Spaces.Count()))
                        {
                            args.IsValid = false;
                            args.Message = $"Parking level-{this.Id} is full";
                        }
                    }
                    else
                    {
                        if (Entries.TryGetValue(args.LicensePlate, out var entry))
                        {
                            if (!PaymentProcessor.ProcessPayment(args.LicensePlate, DateTimeOffset.UtcNow.Subtract(entry.EntryTime)))
                            {
                                args.IsValid = false;
                                args.Message = $"Payment failed";
                            }
                        }
                        else
                        {
                            args.IsValid = false;
                            args.Message = "License plate not found";
                        }
                    }
                }
            }
            else
            {
                if (lane.LaneType.Equals(LaneType.Entry))
                {
                    this.Entries[args.LicensePlate] = new ParkingEntry();
                }
                else
                {
                    this.Entries.Remove(args.LicensePlate);
                }
            }

        }
    }
}