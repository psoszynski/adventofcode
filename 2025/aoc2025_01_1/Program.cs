var raw = await File.ReadAllLinesAsync("input.txt");

//ex L68, R12
var rotations = raw.Select(i => new Rotation
{
    Direction = i[0],
    Size = int.Parse(i[1..])
}).ToList();

var myLock = new Lock();
long zeroPasses = 0;

foreach (var rotation in rotations)
{
    int passes = rotation.Direction == 'L'
        ? myLock.RotateLeft(rotation.Size)
        : myLock.RotateRight(rotation.Size);

    zeroPasses += passes;

    Console.WriteLine($"Dir: {rotation.Direction} Size: {rotation.Size} Current Position: {myLock.CurrentPosition} ZeroHitsThisMove: {passes}");
}
Console.WriteLine(zeroPasses);

class Rotation
{
    public char Direction { get; set; }
    public int Size { get; set; }
}

class Lock
{
    public int CurrentPosition { get; private set; } = 50;

    public int RotateRight(int amount)
    {
        int hits = (CurrentPosition + amount) / 100;
        CurrentPosition = (CurrentPosition + amount) % 100;
        return hits;
    }

    public int RotateLeft(int amount)
    {
        int hits;
        if (CurrentPosition == 0)
        {
            hits = amount / 100;
        }
        else
        {
            hits = (amount + (100 - CurrentPosition)) / 100;
        }

        CurrentPosition = ((CurrentPosition - amount) % 100 + 100) % 100;
        return hits;
    }
}