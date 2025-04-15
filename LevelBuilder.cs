namespace ParkingLot;

public class LevelBuilder
{
    private readonly ILevel _level;
    private readonly ParkingBuilder _parkingBuilder;

    public LevelBuilder(ParkingBuilder parkingBuilder, int id)
    {
        _level = new Level(id);
        this._parkingBuilder = parkingBuilder;
    }

    public LevelBuilder WithSpaces(int count, SpaceType type)
    {
        for (int i = 0; i < count; i++)
        {
            var spaceId = _level.Spaces.Count + 1;
            _level.AddSpace(new Space(spaceId, type));
        }
        return this;
    }

    public LevelBuilder WithLane(LaneType type)
    {
        _level.AddLane(new Lane(type));
        return this;
    }

    public LevelBuilder WithPaymentProcessor(IPaymentProcessor paymentProcessor)
    {

        _level.PaymentProcessor = paymentProcessor;
        return this;
    }

    public ParkingBuilder Add()
    {
        _parkingBuilder.AddLevel(this._level);
        return this._parkingBuilder;
    }
}