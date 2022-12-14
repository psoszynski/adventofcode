var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\2022\12\input.txt");

var length = input[0].Length; //8
(var start, var stop, var rows) = FindStartStop(input);

var points = new HashSet<(int, int)>();
var newPoints = new HashSet<(int, int)>();
var visitedPoints = new HashSet<(int, int)>();


points.Add((start));
var i = 1;
while (true)
{
    newPoints = NextIteration(points);
    if (newPoints.Count() == 0) break;
    points = newPoints;
    i++;
    PrintPoints(points);
}
Console.WriteLine(i);

void PrintPoints(HashSet<(int, int)> points)
{
    foreach (var p in points)
    {
        Console.Write($"({p.Item1},{p.Item2},{input[p.Item2][p.Item1]}) ");
    }
    Console.WriteLine();
}

HashSet<(int, int)> NextIteration(HashSet<(int, int)> points)
{
    var newPoints = new HashSet<(int, int)>();
    foreach (var p in points)
    {
        int xnew, ynew, heightOfNew;
        var heightOfCurrent = input[p.Item2][p.Item1];
        if (heightOfCurrent == 'S') heightOfCurrent = 'a';
        if (heightOfCurrent == 'E') heightOfCurrent = 'z';

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
    return newPoints;
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
            start = (row, sElement.First().Item2);
        }
        var eElement = lChar.Select((x, i) => (x, i)).Where(i => i.Item1 == 'E');
        if (eElement.Count() > 0)
        {
            end = (row, eElement.First().Item2);
        }
        row++;
    }
    return (start, end, row);
}

