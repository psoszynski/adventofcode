var lines = File.ReadAllLines("input.txt");

var maxY = lines.Length;
var maxX = lines[0].Length;
var grid = new bool[maxY, maxX];

for (int y = 0; y < maxY; y++)
    for (int x = 0; x < maxX; x++)
        grid[y, x] = lines[y][x] == '@';

(int accessibleCount, char[,] resultGrid) = FindAccessibleToiletpapers(maxY, maxX, grid);

Console.WriteLine(accessibleCount);

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

(int,char[,]) FindAccessibleToiletpapers(int maxY, int maxX, bool[,] grid)
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
    return (total,grid2);
}