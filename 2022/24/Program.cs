var input = File.ReadAllLines("./input.txt");

var searchPositions = new List<(int, int)>();
searchPositions.Add((0,-1));

var i = 1;
var matrix = ReadBoard(input);
var ymax = matrix.GetLength(0);
var xmax = matrix.GetLength(1);
while (true)
{
    ProceedOneMinute(matrix);
    searchPositions = FindNewSearchPositions(matrix, searchPositions);
    Console.SetCursorPosition(0, 0);
    Print(matrix, searchPositions);
    i++;
    //Console.WriteLine(i);
    if(searchPositions.Any(searchPosition => searchPosition.Item1 == xmax - 1 && searchPosition.Item2 == ymax - 1))
    {
        Console.WriteLine($"Found exit 1 at minute {i}");
        break;
    }
    //Thread.Sleep(200);
}

searchPositions.Clear();
searchPositions.Add((xmax-1, ymax));
while (true)
{
    ProceedOneMinute(matrix);
    searchPositions = FindNewSearchPositions(matrix, searchPositions);
    Console.SetCursorPosition(0, 0);
    Print(matrix, searchPositions);
    i++;
    //Console.WriteLine(i);
    if (searchPositions.Any(searchPosition => searchPosition.Item1 == 0 && searchPosition.Item2 == 0))
    {
        Console.WriteLine($"Found exit 2 at minute {i}");
        break;
    }
    //Thread.Sleep(200);
}

searchPositions.Clear();
searchPositions.Add((0, -1));
while (true)
{
    ProceedOneMinute(matrix);
    searchPositions = FindNewSearchPositions(matrix, searchPositions);
    Console.SetCursorPosition(0, 0);
    Print(matrix, searchPositions);
    i++;
    //Console.WriteLine(i);
    if (searchPositions.Any(searchPosition => searchPosition.Item1 == xmax - 1 && searchPosition.Item2 == ymax - 1))
    {
        Console.WriteLine($"Found exit 3 at minute {i}");
        break;
    }
    //Thread.Sleep(200);
}



List<(int, int)> FindNewSearchPositions((List<char>, List<char>)[,] matrix, List<(int, int)> searchPositions)
{
    var ymax = matrix.GetLength(0);
    var xmax = matrix.GetLength(1);

    var newSearchPositions = new List<(int, int)>();
    foreach(var searchPosition in searchPositions)
    {
        var foundMove = false;
        //check left
        var xnew = searchPosition.Item1 - 1;
        var ynew = searchPosition.Item2;
        foundMove = UpdateNewSearchPositions(matrix, ymax, xmax, newSearchPositions, xnew, ynew);
        //check up
        xnew = searchPosition.Item1;
        ynew = searchPosition.Item2 - 1;
        foundMove = UpdateNewSearchPositions(matrix, ymax, xmax, newSearchPositions, xnew, ynew);
        //check right
        xnew = searchPosition.Item1 + 1;
        ynew = searchPosition.Item2;
        foundMove = UpdateNewSearchPositions(matrix, ymax, xmax, newSearchPositions, xnew, ynew);
        //check down
        xnew = searchPosition.Item1;
        ynew = searchPosition.Item2 + 1;
        foundMove = UpdateNewSearchPositions(matrix, ymax, xmax, newSearchPositions, xnew, ynew);
        if (!foundMove)
        {
            xnew = searchPosition.Item1;
            ynew = searchPosition.Item2;
            if (ynew == -1 || ynew >= ymax || (matrix[ynew, xnew].Item1.Count() == 0
                && !newSearchPositions.Any(newSearchPosition => newSearchPosition.Item1 == xnew && newSearchPosition.Item2 == ynew)))
            {
                newSearchPositions.Add((xnew, ynew));
            }
        }
    }

    return newSearchPositions;

    static bool UpdateNewSearchPositions((List<char>, List<char>)[,] matrix, int ymax, int xmax, List<(int, int)> newSearchPositions, int xnew, int ynew)
    {
        var foundMove = false;

        if(xnew == 0 && ynew == -1 && !newSearchPositions.Any(newSearchPosition => newSearchPosition.Item1 == xnew && newSearchPosition.Item2 == ynew))
        {
            newSearchPositions.Add((xnew, ynew));
            foundMove = true;
        }

        if (xnew >= 0 && ynew >= 0 && xnew < xmax && ynew < ymax
            && matrix[ynew, xnew].Item1.Count() == 0
            && !newSearchPositions.Any(newSearchPosition => newSearchPosition.Item1 == xnew && newSearchPosition.Item2 == ynew))
        {
            newSearchPositions.Add((xnew, ynew));
            foundMove = true;
        }

        return foundMove;
    }
}

void ProceedOneMinute((List<char>,List<char>)[,] matrix)
{
    var ymax = matrix.GetLength(0);
    var xmax = matrix.GetLength(1);

    for (var y = 0; y < ymax; y++)
    {
        for (var x = 0; x < xmax; x++)
        {
            foreach (char c in matrix[y, x].Item1)
            {
                switch (c)
                {
                    case '>':
                        if (x < xmax - 1)
                            matrix[y, x + 1].Item2.Add(c);
                        else
                            matrix[y, 0].Item2.Add(c);
                        break;
                    case 'v':
                        if ( y < ymax - 1)
                            matrix[y + 1, x].Item2.Add(c);
                        else
                            matrix[0, x].Item2.Add(c);
                        break;
                    case '<':
                        if (x > 0)
                            matrix[y, x - 1].Item2.Add(c);
                        else
                            matrix[y, xmax - 1].Item2.Add(c);
                        break;
                    case '^':
                        if (y > 0)
                            matrix[y - 1, x].Item2.Add(c);
                        else
                            matrix[ymax - 1, x].Item2.Add(c);
                        break;
                }
            }
        }
    }

    for (var y = 0; y < ymax; y++)
    {
        for (var x = 0; x < xmax; x++)
        {
            matrix[y, x].Item1.Clear();
            foreach (var e in matrix[y, x].Item2)
                matrix[y, x].Item1.Add(e);
            matrix[y, x].Item2.Clear();
        }
    }
}

void Print((List<char>,List<char>)[,] matrix, List<(int, int)> listOfPossibleMoves)
{
    var ymax = matrix.GetLength(0);
    var xmax = matrix.GetLength(1);

    for (var y = 0; y < ymax; y++)
    {
        for (var x = 0; x < xmax; x++)
        {
            var chars = matrix[y, x].Item1;
            if (chars.Count() > 1)
            {
                Console.Write(chars.Count());
            }
            else if (chars.Count() == 1)
            {
                var foregroundColor = Console.ForegroundColor;
                var c = matrix[y, x].Item1[0];
                switch (c)
                {
                    case '>':
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case '<':
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case 'v':
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case '^':
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    default:
                        break;
                }
                Console.Write(c);
                Console.ForegroundColor = foregroundColor;
            }
            else if (listOfPossibleMoves.Any(possibleMove => possibleMove.Item1 == x && possibleMove.Item2 == y))
            {
                var backgroundColor = Console.BackgroundColor;
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(' ');
                Console.BackgroundColor = backgroundColor;
            }
            else
            {
                Console.Write('.');
            }
        }
        Console.WriteLine();
    }
}

(List<char>,List<char>)[,] ReadBoard(string[] input)
{
    var matrix = new (List<char>, List<char>)[input.Length - 2, input[0].Length - 2];
    for(var y  = 1; y < input.Length - 1; y++)
    {
        for(var x = 1; x < input[0].Length - 1; x++)
        {
            matrix[y - 1, x - 1].Item1 = new List<char>();
            matrix[y - 1, x - 1].Item2 = new List<char>();
            matrix[y - 1, x - 1].Item1.Add(input[y][x]);
        }
    }
    return matrix;
}
