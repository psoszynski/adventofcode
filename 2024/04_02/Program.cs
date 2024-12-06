char[,] array = ReadFileToArray("input.txt");
int rows = array.GetLength(0);
int cols = array.GetLength(1);

// Console.Clear();
// Console.SetCursorPosition(0,0);

// for (var i = 0; i < rows; i++)
// {
//     for (var j = 0; j < cols; j++)
//     {
//         Console.SetCursorPosition(j, i);
//         Console.Write(array[i, j]);
//     }
//     Console.WriteLine();
// }

var count = 0;
for (var i = 0; i < rows - 2; i++)
{
    for (var j = 0; j < cols - 2; j++)
    {
        count += VerifyXmas(array, i, j);
    }
    //Console.WriteLine();
}
Console.WriteLine();
Console.WriteLine(count);

// Variations:
// M.S  S.S  M.M  S.M
// .A.  .A.  .A.  .A.
// M.S  M.M  S.S  S.M

int VerifyXmas(char[,] a, int i, int j)
{
    var c = 0;
    if (a[i + 1, j + 1] == 'A')
    {
        if(a[i, j] == 'M' && a[i, j + 2] == 'S' && a[i + 2, j] == 'M' && a[i + 2, j + 2] == 'S')
        {
            c++;
        }
        if(a[i, j] == 'S' && a[i, j + 2] == 'S' && a[i + 2, j] == 'M' && a[i + 2, j + 2] == 'M')
        {
            c++;
        }
        if (a[i, j] == 'M' && a[i, j + 2] == 'M' && a[i + 2, j] == 'S' && a[i + 2, j + 2] == 'S')
        {
            c++;
        }
        if(a[i, j] == 'S' && a[i, j + 2] == 'M' && a[i + 2, j] == 'S' && a[i + 2, j + 2] == 'M')
        {
            c++;
        }
    }
    return c;
}

void Highlight(char[,] a, int i, int j, ConsoleColor c)
{
    Console.SetCursorPosition(j, i);
    Console.ForegroundColor = c;
    Console.Write(a[i, j]);
    Console.ResetColor();
    Thread.Sleep(50);
}

char[,] ReadFileToArray(string filePath)
{
    // Read original lines
    string[] lines = File.ReadAllLines(filePath);
    int rows = lines.Length;
    int cols = lines[0].Length;

    // Create and fill the array
    char[,] array = new char[rows, cols];
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            array[i, j] = lines[i][j];
        }
    }

    return array;
}