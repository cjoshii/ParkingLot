using ParkingLot;

Console.WriteLine("Welcome to awesome parking!");

var parkingBuilder = new ParkingBuilder();

var parking = parkingBuilder
    .CreateLevel(1)
        .WithLane(LaneType.Entry)
        .WithLane(LaneType.Exit)
        .WithSpaces(100, SpaceType.Standard)
        .WithSpaces(10, SpaceType.Reserved)
        .WithPaymentProcessor(new DefaultPaymentProcessor())
        .Add()
    .CreateLevel(2)
        .WithLane(LaneType.Entry)
        .WithLane(LaneType.Exit)
        .WithSpaces(50, SpaceType.Standard)
        .WithSpaces(5, SpaceType.Reserved)
        .WithPaymentProcessor(new DefaultPaymentProcessor())
        .Add()
    .Build();


void enterIntoParking(int level, int space, string plate)
{
    var parkingLevel = parking.Levels[level];
    var entryLane = parkingLevel.Lanes.First(l => l.LaneType == LaneType.Entry);

    if (entryLane.Validate(plate))
    {
        entryLane.Transit(plate);
        parkingLevel.Spaces[space].ToggleSpace(plate, true);
    }
}

void exitFromParking(int level, int space, string plate)
{
    var parkingLevel = parking.Levels[level];
    parkingLevel.Spaces[space].ToggleSpace(plate, false);

    var exitLane = parkingLevel.Lanes.First(l => l.LaneType == LaneType.Exit);

    if (exitLane.Validate(plate))
    {
        exitLane.Transit(plate);
    }
}

for (int i = 0; i < parking.Levels.Count; i++)
{
    for (int j = 0; j < parking.Levels[i].Spaces.Count; j++)
    {
        enterIntoParking(i, j, $"plate-{i}-{j + 1}");
    }
}

for (int i = 0; i < parking.Levels.Count; i++)
{
    for (int j = 0; j < parking.Levels[i].Spaces.Count; j++)
    {
        exitFromParking(i, j, $"plate-{i}-{j + 1}");
    }
}

Console.ReadLine();