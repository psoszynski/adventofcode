using System.Data;
using Microsoft.VisualBasic;

var input = File.ReadAllLines("input.txt");
//var input = File.ReadAllLines("input.txt");

var count = 0;
// foreach(var line in input)
// {
//     var lineAsInts = line.Split(' ').Select(int.Parse).ToArray();
//     if(IsSafe(lineAsInts))
//     {
//         Console.WriteLine("true");
//         count++;
//     }
//     else
//     {
//         Console.WriteLine("false");
//     }
// }
// Console.WriteLine(count);

//part 2
count=0;
foreach(var line in input)
{
    var lineAsInts = line.Split(' ').Select(int.Parse).ToList();
    if(IsIncreasingOrDecreasingWithTolerance(lineAsInts))
    {
        Console.WriteLine($"{line}: true");
        count++;
    }
    else
    {
        Console.WriteLine($"{line}: false");
    }
}
Console.WriteLine(count);

bool IsSafe(List<int> row)
{
    if (IsIncreasing(row) || IsDecreasing(row))
    {
        return true;
    }
    return false;
}

bool IsIncreasing(List<int> row)
{
    for (int i = 0; i < row.Count-1; i++)
    {
        if (row[i+1]-row[i] > 3 || row[i+1]-row[i] < 1)
        {
            return false;
        }

    }
    return true;
}

bool IsDecreasing(List<int> row)
{
    for (int i = 0; i < row.Count-1; i++)
    {
        if (row[i]-row[i+1] > 3 || row[i]-row[i+1] < 1)
        {
            return false;
        }
    }
    return true;
}

bool IsIncreasingOrDecreasingWithTolerance(List<int> row)
{
    if(IsIncreasing(row) || IsDecreasing(row))
    {
        return true;
    }

    for (int i = 0; i < row.Count; i++)
    {
        var rowWithoutOneBadLevel = row.Take(i).Concat(row.Skip(i + 1)).ToList();
        if (IsIncreasing(rowWithoutOneBadLevel) || IsDecreasing(rowWithoutOneBadLevel))
        {
            return true;
        }
    }
    return false;
}