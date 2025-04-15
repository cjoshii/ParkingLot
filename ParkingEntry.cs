namespace ParkingLot;

public class ParkingEntry
{
    public DateTimeOffset EntryTime { get; private set; }
    public int? ParkingSpaceId { get; set; }

    public ParkingEntry()
    {
        this.EntryTime = DateTimeOffset.UtcNow;
    }
}