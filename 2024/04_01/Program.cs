
char[,] array = ReadFileToArray("testinput.txt");
int rows = array.GetLength(0)-3;
int cols = array.GetLength(1)-3;

Console.Clear();
Console.SetCursorPosition(0,0);

for (var i = 0; i < rows; i++)
{
    for (var j = 0; j < cols; j++)
    {
        Console.SetCursorPosition(j, i);
        Console.Write(array[i, j]);
    }
    Console.WriteLine();
}

var count = 0;
for (var i = 0; i < rows; i++)
{
    for (var j = 0; j < cols; j++)
    {
        var consoleColors = VerifyXmas(array, i, j);
        if(consoleColors.Count > 0)
        {
            count+=consoleColors.Count();
            Highlight(array, i, j, consoleColors.First());
        }
    }
    Console.WriteLine();
}
Console.WriteLine(count);

List<ConsoleColor> VerifyXmas(char[,] a, int i, int j)
{
    var listOfColors = new List<ConsoleColor>();
    for (int d = 1; d <= 4; d++)
    {
        switch (d)
        {
            case 1: // Horizontal - left to right
                if ((a[i, j] == 'X' && a[i, j + 1] == 'M' && a[i, j + 2] == 'A' && a[i, j + 3] == 'S') ||
                    (a[i, j] == 'S' && a[i, j + 1] == 'A' && a[i, j + 2] == 'M' && a[i, j + 3] == 'X'))
                {
                    listOfColors.Add(ConsoleColor.Red);
                }
                continue;

            case 2: // Diagonal down-right
                if ((a[i, j] == 'X' && a[i + 1, j + 1] == 'M' && a[i + 2, j + 2] == 'A' && a[i + 3, j + 3] == 'S') ||
                    (a[i, j] == 'S' && a[i + 1, j + 1] == 'A' && a[i + 2, j + 2] == 'M' && a[i + 3, j + 3] == 'X'))
                {
                    listOfColors.Add(ConsoleColor.Green);
                }
                continue;

            case 3: // Vertical - top to bottom
                if ((a[i, j] == 'X' && a[i + 1, j] == 'M' && a[i + 2, j] == 'A' && a[i + 3, j] == 'S') ||
                    (a[i, j] == 'S' && a[i + 1, j] == 'A' && a[i + 2, j] == 'M' && a[i + 3, j] == 'X'))
                {
                    listOfColors.Add(ConsoleColor.Blue);
                }
                continue;

            case 4: // Diagonal down-left
                if (j < 3) continue;
                if ((a[i, j] == 'X' && a[i + 1, j - 1] == 'M' && a[i + 2, j - 2] == 'A' && a[i + 3, j - 3] == 'S') ||
                    (a[i, j] == 'S' && a[i + 1, j - 1] == 'A' && a[i + 2, j - 2] == 'M' && a[i + 3, j - 3] == 'X'))
                {
                    listOfColors.Add(ConsoleColor.Yellow);
                }
                continue;
        }
    }

    return listOfColors;
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
    int originalRows = lines.Length;
    int originalCols = lines[0].Length;
    
    // Calculate new dimensions
    int newRows = originalRows + 3;
    int newCols = originalCols + 3;

    // Pad existing lines with dots
    for (int i = 0; i < originalRows; i++)
    {
        lines[i] = lines[i] + "...";
    }

    // Create new lines with all dots
    var paddedLines = lines.ToList();
    for (int i = 0; i < 3; i++)
    {
        paddedLines.Add(new string('.', newCols));
    }

    // Create and fill the array
    char[,] array = new char[newRows, newCols];
    for (int i = 0; i < newRows; i++)
    {
        for (int j = 0; j < newCols; j++)
        {
            array[i, j] = paddedLines[i][j];
        }
    }

    return array;
}

