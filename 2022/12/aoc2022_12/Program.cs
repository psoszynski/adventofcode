using Spectre.Console;

AnsiConsole.ResetColors();

Thread.Sleep(20);
var input = File.ReadAllLines(@"input.txt");
var length = input[0].Length; //8
#pragma warning disable CA1416 // Validate platform compatibility
Console.BufferWidth = Console.LargestWindowWidth;
Console.BufferHeight = Console.LargestWindowHeight;
#pragma warning restore CA1416 // Validate platform compatibility

//PrintStartMap(input);

(var allAPoints, var start, var rows) = FindAllAPoints(input);

HashSet<(int, int)> points;
HashSet<(int, int)> newPoints;
HashSet<(int, int)> visitedPoints;

var i = 1;

//part 1

//points = new HashSet<(int, int)>();
//newPoints = new HashSet<(int, int)>();
//visitedPoints = new HashSet<(int, int)>();
//points.Add(start);
//while (true)
//{
//    (var foundDestination, newPoints) = NextIteration(points);
//    PrintPoints(newPoints, Spectre.Console.Color.Red);
//    Thread.Sleep(20);
//    PrintPoints(points);
//    if (foundDestination || newPoints.Count() == 0) break;
//    points = newPoints;
//    i++;
//}
//Console.CursorTop = input.Count() - 1;
//Console.WriteLine(++i);

//part 2
allAPoints.Add(start);
var shortestWay = 1000;
foreach (var a in allAPoints)
{
    points = new HashSet<(int, int)>();
    newPoints = new HashSet<(int, int)>();
    visitedPoints = new HashSet<(int, int)>();

    points.Add(a);
    var foundDestination = false;
    i = 1;
    while (true)
    {
        (foundDestination, newPoints) = NextIteration(points);
        //PrintPoints(newPoints, Spectre.Console.Color.Red);
        //Thread.Sleep(20);
        //PrintPoints(points);
        if (foundDestination || newPoints.Count() == 0) break;
        points = newPoints;
        i++;
    }

    if (foundDestination && ++i < shortestWay) shortestWay = i;
    Console.WriteLine(shortestWay);
}
Console.CursorTop = input.Count() - 1;

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

(HashSet<(int, int)>, (int, int), int) FindAllAPoints(string[] input)
{
    var row = 0;
    var allAPoints = new HashSet<(int, int)>();
    (int x, int y) start = (-1, -1);
    (int x, int y) end = (-1, -1);
    foreach (var l in input)
    {
        for (var i = 0; i < l.Length; i++)
        {
            if (l[i] == 'S') start = (i, row);
            if (l[i] == 'a') allAPoints.Add((i, row));
        }
        row++;
    }
    return (allAPoints, start, row);
}

void PrintPoints(HashSet<(int, int)> points, Spectre.Console.Color? color = null)
{
    if (color.HasValue)
    {
        foreach (var p in points)
        {
            Console.CursorLeft = p.Item1;
            Console.CursorTop = p.Item2;
            AnsiConsole.Background = color.Value;
            AnsiConsole.Write(input![p.Item2][p.Item1]);
        }
    }
    else
    {
        foreach (var p in points)
        {
            Console.CursorLeft = p.Item1;
            Console.CursorTop = p.Item2;
            var c = input![p.Item2][p.Item1];
            AnsiConsole.Background = Convert.ToInt32(22 * ((float)c - 97) / 25) + 232;
            AnsiConsole.Write(input[p.Item2][p.Item1]);
        }
    }
}

static void PrintStartMap(string[] input)
{
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
}