
//char[] delimiters = new char[] { ' ', '\t' };
//var lines = await File.ReadAllLinesAsync("input1.txt");
//var int1 = lines[0].Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();
//var int2 = lines[1].Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();
//var int3 = lines[2].Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();
//var int4 = lines[3].Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();
//var ops =  lines[4].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

//long total = 0;
//for(var i=0;i<1000;i++)
//{
//    long r = 0;
//    if (ops[i] == "+") r = int1[i] + int2[i] + int3[i] + int4[i];
//    else if (ops[i] == "*") r = int1[i] * int2[i] * int3[i] * int4[i];
//    total += r;
//    Console.WriteLine($"{ops[i]} {int1[i]} {int2[i]} {int3[i]} {int4[i]} {r}");
//}
//Console.WriteLine("total:");
//Console.WriteLine(total);


using System.Numerics;

var lines = await File.ReadAllLinesAsync("input1.txt");


// 1 1 - - = 11
// - 1 1 - = 11
// - - 1 1 = 11
long operationTotal = 0;
long total = 0;
char currentOp = 'x';
for (var i=0; i < lines[0].Length;i++)
{
    (var columnValue, var op) = GetColumnValue(lines, i);
    if (op == '*' || op == '+')
    {
        currentOp = op;
        operationTotal = columnValue;
    }
    else
    {
        if(columnValue == 0)
        {
            currentOp = 'x';
        }
        if (currentOp == '+')
        {
            operationTotal += columnValue;
        }
        else if (currentOp == '*')
        {
            operationTotal *= columnValue;
        }
    }
    Console.WriteLine($"{columnValue} {op}");
    if (columnValue == 0)
    {
        Console.WriteLine(operationTotal);
        Console.WriteLine("----");
        total += operationTotal;
    }
}
total+= operationTotal;
Console.WriteLine("Final Total:");
Console.WriteLine(total);

static (int, char) GetColumnValue(string[] lines, int i)
{
    var columnValue = 0;
    if (lines[3][i] != ' ')
    {
        columnValue += lines[3][i] - 48;
        if (lines[2][i] != ' ')
        {
            columnValue += (lines[2][i] - 48) * 10;
            if (lines[1][i] != ' ')
            {
                columnValue += (lines[1][i] - 48) * 100;
                if (lines[0][i] != ' ')
                {
                    columnValue += (lines[0][i] - 48) * 1000;
                }
            }
        }
    }
    else if (lines[2][i] != ' ')
    {
        columnValue += lines[2][i] - 48;
        if (lines[1][i] != ' ')
        {
            columnValue += (lines[1][i] - 48) * 10;
            if (lines[0][i] != ' ')
            {
                columnValue += (lines[0][i] - 48) * 100;
            }
        }
    }
    else if (lines[1][i] != ' ')
    {
        columnValue += (lines[1][i] - 48);
        if (lines[0][i] != ' ')
        {
            columnValue += (lines[0][i] - 48) * 10;
        }
    }
    else if (lines[0][i] != ' ')
    {
        columnValue += (lines[0][i] - 48);
    }
    return (columnValue, lines[4][i]);
}