using Spectre.Console;
AnsiConsole.ResetColors();

Thread.Sleep(2);
var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\2022\12\input.txt");
var length = input[0].Length; //8
#pragma warning disable CA1416 // Validate platform compatibility
Console.BufferWidth = Console.LargestWindowWidth;
Console.BufferHeight = Console.LargestWindowHeight;
#pragma warning restore CA1416 // Validate platform compatibility

var y = 0;
foreach (var line in input)
{
    var x = 0;
    foreach (var c in line)
    {
        int c1 = Convert.ToInt32(22 * ((float)c - 97) / 25) + 232;
        Console.CursorLeft = x;
        Console.CursorTop = y;
        AnsiConsole.Background = Spectre.Console.Color.FromInt32(c1);
        AnsiConsole.Write(c);
        x++;
    }
    y++;
}

(var start, var stop, var rows) = FindStartStop(input);

var points = new HashSet<(int, int)>();
var newPoints = new HashSet<(int, int)>();
var visitedPoints = new HashSet<(int, int)>();

points.Add((start));
var i = 1;
while (true)
{
    (var foundDestination, newPoints) = NextIteration(points);
    PrintPoints(newPoints, Spectre.Console.Color.Red);
    Thread.Sleep(20);
    PrintPoints(points);
    if (foundDestination || newPoints.Count() == 0) break;
    points = newPoints;
    i++;
}

Console.CursorTop = input.Count()-1;

(bool, HashSet<(int, int)>) NextIteration(HashSet<(int, int)> points)
{
    var newPoints = new HashSet<(int, int)>();
    foreach (var p in points)
    {
        int xnew, ynew, heightOfNew;
        var heightOfCurrent = input[p.Item2][p.Item1];
        if (heightOfCurrent == 'S') heightOfCurrent = 'a';
        if (heightOfCurrent == 'E') //heightOfCurrent = 'z';
        {
            return (true, newPoints);
        }

        xnew = p.Item1 + 1;
        if (xnew < length)
        {
            ynew = p.Item2;
            if (!visitedPoints.Contains((xnew, ynew)))
            {
                heightOfNew = input[ynew][xnew];
                if (heightOfNew <= heightOfCurrent || heightOfNew - heightOfCurrent == 1)
                {
                    newPoints.Add((xnew, ynew));
                    visitedPoints.Add((xnew, ynew));
                }
            }
        }
        xnew = p.Item1;
        ynew = p.Item2 + 1;
        if (ynew < rows && !visitedPoints.Contains((xnew, ynew)))
        {
            heightOfNew = input[ynew][xnew];
            if (heightOfNew <= heightOfCurrent || heightOfNew - heightOfCurrent == 1)
            {
                newPoints.Add((xnew, ynew));
                visitedPoints.Add((xnew, ynew));
            }
        }
        xnew = p.Item1 - 1;
        if (xnew > -1)
        {
            ynew = p.Item2;
            if (!visitedPoints.Contains((xnew, ynew)))
            {
                heightOfNew = input[ynew][xnew];
                if (heightOfNew <= heightOfCurrent || heightOfNew - heightOfCurrent == 1)
                {
                    newPoints.Add((xnew, ynew));
                    visitedPoints.Add((xnew, ynew));
                }
            }
        }
        xnew = p.Item1;
        ynew = p.Item2 - 1;
        if (ynew > -1 && !visitedPoints.Contains((xnew, ynew)))
        {
            heightOfNew = input[ynew][xnew];
            if (heightOfNew <= heightOfCurrent || heightOfNew - heightOfCurrent == 1)
            {
                newPoints.Add((xnew, ynew));
                visitedPoints.Add((xnew, ynew));
            }
        }
    }
    return (false, newPoints);
}

((int, int), (int, int), int) FindStartStop(string[] input)
{
    var row = 0;
    (int x, int y) start = (-1, -1);
    (int x, int y) end = (-1, -1);
    foreach (var l in input)
    {
        var lChar = l.ToCharArray();
        var sElement = lChar.Select((x, i) => (x, i)).Where(i => i.Item1 == 'S');
        if (sElement.Count() > 0)
        {
            start = (sElement.First().Item2, row);
        }
        var eElement = lChar.Select((x, i) => (x, i)).Where(i => i.Item1 == 'E');
        if (eElement.Count() > 0)
        {
            end = (eElement.First().Item2, row);
        }
        row++;
    }
    return (start, end, row);
}

void PrintPoints(HashSet<(int,int)> points, Spectre.Console.Color? color = null)
{
    if (color.HasValue)
    {
        foreach (var p in points)
        {
            Console.CursorLeft = p.Item1;
            Console.CursorTop = p.Item2;
            AnsiConsole.Background = color.Value;
            AnsiConsole.Write(input[p.Item2][p.Item1]);
        }
    }
    else
    {
        foreach (var p in points)
        {
            Console.CursorLeft = p.Item1;
            Console.CursorTop = p.Item2;
            var c = input[p.Item2][p.Item1];
            AnsiConsole.Background = Convert.ToInt32(22 * ((float)c - 97) / 25) + 232;
            AnsiConsole.Write(input[p.Item2][p.Item1]);
        }
    }
}