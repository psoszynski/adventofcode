using Spectre.Console;

var lines = File.ReadAllLines("input1.txt");

var maxY = lines.Length;
var maxX = lines[0].Length;
var grid = new bool[maxY, maxX];

for (int y = 0; y < maxY; y++)
    for (int x = 0; x < maxX; x++)
        grid[y, x] = lines[y][x] == '@';

int totalAccessibleCount = 0;

AnsiConsole.Live(new Canvas(maxX, maxY))
    .Start(ctx =>
    {
        while (true)
        {
            (int accessibleCount, char[,] resultGrid) = FindAccessibleToiletpapers(maxY, maxX, grid);

            var canvas = new Canvas(maxX, maxY);
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    if (grid[y, x])
                    {
                        if (resultGrid[y, x] == 'x')
                            canvas.SetPixel(x, y, Color.Red);
                        else
                            canvas.SetPixel(x, y, Color.Green);
                    }
                    else
                    {
                        canvas.SetPixel(x, y, Color.Grey);
                    }
                }
            }
            ctx.UpdateTarget(canvas);
            Thread.Sleep(50);

            if (accessibleCount == 0) break;

            totalAccessibleCount += accessibleCount;
            RemoveToiletPapers(grid, resultGrid);
        }
    });

Console.WriteLine(totalAccessibleCount);

//for (int y = 0; y < maxY; y++)
//{
//    for (int x = 0; x < maxX; x++)
//    {
//        Console.Write(grid2[y, x]);
//    }
//    Console.WriteLine();
//}

// ...
// .x.
// ...

int Check(int y, int x)
{
    var count = 0;
    if (y > 0 && x > 0 && grid[y - 1, x - 1]) count++;
    if (y > 0 && grid[y - 1, x]) count++;
    if (y > 0 && x < maxX - 1 && grid[y - 1, x + 1]) count++;

    if (x > 0 && grid[y, x - 1]) count++;
    if (x < maxX - 1 && grid[y, x + 1]) count++;

    if (y < maxY - 1 && x > 0 && grid[y + 1, x - 1]) count++;
    if (y < maxY - 1 && grid[y + 1, x]) count++;
    if (y < maxY - 1 && x < maxX - 1 && grid[y + 1, x + 1]) count++;
    return count;
}

(int, char[,]) FindAccessibleToiletpapers(int maxY, int maxX, bool[,] grid)
{
    var total = 0;
    var grid2 = new char[maxY, maxX];
    for (int y = 0; y < maxY; y++)
        for (int x = 0; x < maxX; x++)
        {
            if (grid[y, x])
            {
                var count = Check(y, x);
                //grid2[y, x] = (char)('0' + count);
                if (count < 4)
                {
                    total++;
                    grid2[y, x] = 'x';
                }
                else
                {
                    grid2[y, x] = '.';
                }
            }
            else
            {
                grid2[y, x] = '.';
            }
        }
    return (total, grid2);
}

void RemoveToiletPapers(bool[,] grid, char[,] resultGrid)
{
    int maxY = grid.GetLength(0);
    int maxX = grid.GetLength(1);
    for (int y = 0; y < maxY; y++)
        for (int x = 0; x < maxX; x++)
            if (resultGrid[y, x] == 'x')
                grid[y, x] = false;
}