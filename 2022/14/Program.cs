using Spectre.Console;

AnsiConsole.ResetColors();

#pragma warning disable CA1416 // Validate platform compatibility
//Console.BufferWidth = Console.LargestWindowWidth;
Console.BufferWidth = 400;
Console.BufferHeight = 200;
#pragma warning restore CA1416 // Validate platform compatibility

var input = File.ReadAllLines("./input.txt");
var matrix = PrintWalls(input, skipPrint:false);

var maxy = 0;
for (var x = 0; x < 600; x++)
{
    for (var y = 0; y < 200; y++)
    {
        if (matrix[x, y] != 0 && y > maxy)
            maxy = y;
    }
}

PrintLine(0, maxy+2, 999, maxy+2, '*', matrix, skipPrint:false);

long s = -1;
while (true)
{
    //Console.ReadKey();
    s++;
    var sand = ((500, 0), (500, 0));
    var faltOrFinished = false;
    while (true)
    {
        (var status, sand) = CalculateNext(sand, matrix);
        if (status == -1 || status == 3)
        {
            faltOrFinished = true;
            break;
        }
        if (status == 2 || status == 3) break;
        Print(sand.Item2.Item1, sand.Item2.Item2, '.', Spectre.Console.Color.Black);
        Print(sand.Item1.Item1, sand.Item1.Item2, 'o', Spectre.Console.Color.Green);
        Thread.Sleep(5);
        //Debug.WriteLine($"({s} {sand.Item1.Item1},{sand.Item1.Item2})");
    }
    if (faltOrFinished) break;
}

if (maxy + 4 < 62)
    Console.CursorTop = maxy + 4;
else
    Console.CursorTop = 62;
Console.WriteLine(s+1);

(int, ((int, int), (int, int))) CalculateNext(((int, int), (int, int)) sand, char[,] matrix)
{
    // (current),(previous)
    // status: -1 falt, 1 - ok, 2 - cannot move, 3 - finished
    sand.Item2 = sand.Item1;
    var below = matrix[sand.Item1.Item1, sand.Item1.Item2 + 1];
    var belowLeft = matrix[sand.Item1.Item1-1, sand.Item1.Item2 + 1];
    var belowRight = matrix[sand.Item1.Item1+1, sand.Item1.Item2 + 1];
    if (below == 0)
    {
        sand.Item1.Item2++;
    }
    else if (belowLeft == 0)
    {
        sand.Item1.Item1--;
        sand.Item1.Item2++;
    }
    else if(belowRight == 0)
    {
        sand.Item1.Item1++;
        sand.Item1.Item2++;
    }
    else if(sand.Item1.Item1 == 500 && sand.Item1.Item2 == 0)
    {
        return (3, (sand.Item1, sand.Item2));
    } else
    {
        matrix[sand.Item1.Item1, sand.Item1.Item2] = 'o';
        return (2, (sand.Item1, sand.Item2));
    }

    if (sand.Item1.Item2 > 165)
    {
        return (-1, (sand.Item1, sand.Item2));
    }
    return (1, (sand.Item1, sand.Item2));
}

char[,] PrintWalls(string[] input, bool skipPrint=false)
{
    var matrix = new char[1000, 200];

    foreach (var i in input)
    {
        var line = i.Split(" -> ").Select(s => s.Split(",")).ToList();
        for (var p=0; p<line.Count()-1; p++)
        {
            PrintLine(int.Parse(line[p][0]), int.Parse(line[p][1]), int.Parse(line[p+1][0]), int.Parse(line[p+1][1]), 'X', matrix, skipPrint);
        }
    }

    return matrix;
}

//498,4 -> 498,6 -> 496,6
//503,4 -> 502,4 -> 502,9 -> 494,9
void PrintLine(int x1, int y1, int x2, int y2, char c, char[,] matrix, bool skipPrint=false)
{
    if(x1 == x2)
    {
        if(y2 >= y1)
        {
            // DOWN
            for(var j=y1; j <= y2; j++)
            {
                matrix[x1,j] = c;
                if(!skipPrint) Print(x1, j, c);
            }
        }
        else
        {
            // UP
            for (var j = y1; j >= y2; j--)
            {
                matrix[x1, j] = c;
                if (!skipPrint) Print(x1, j, c);
            }
        }
    }
    else
    {
        if (x2 >= x1)
        {
            // RIGHT
            for (var j = x1; j <= x2; j++)
            {
                matrix[j, y1] = c;
                if(!skipPrint) Print(j, y1, c);
            }
        }
        else
        {
            // LEFT
            for (var j = x1; j >= x2; j--)
            {
                matrix[j, y1] = c;
                if (!skipPrint) Print(j, y1, c);
            }
        }
    }

}

void Print(int x, int y, char c, Spectre.Console.Color? color = null)
{
    //3 columns
    //1: 0-99 (50), 2: 101-200 (151), 3: 202-301 (252)

    x -= 450;

    var ym = 60;
    if (y >= ym)
    {
        y -= ym;
        x += 101;
    }
    if (y >= ym)
    {
        y -= ym;
        x += 101;
    }

    if (x < 0 || x > 300) return;
    if (color == null) color = Spectre.Console.Color.Red;
    Console.CursorLeft = x;
    Console.CursorTop = y;
    AnsiConsole.Foreground = color.Value;
    AnsiConsole.Write(c);
}