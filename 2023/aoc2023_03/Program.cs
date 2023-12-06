using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

var engineSchematic = File.ReadAllLines("real_input.txt");
var foregroundColor = Console.ForegroundColor;

var height = engineSchematic.Count();
var length = engineSchematic[0].Length;
var engine = new char[length+2, height+2];

for (var y = 1; y <= height; y++)
{
    for (var x = 1; x <= length; x++)
    {
        engine[y, x] = engineSchematic[y-1][x-1];
    }
}

var currentNumber = new List<int>();
var sumOfNumbers = 0;
var symbol = false;
var gears = new Dictionary<(int a,int b), List<(int,int,int)>>();
var currentStars = new List<(int,int)>();

for (var y = 1; y <= height; y++)
{
    for (var x = 1; x <= length; x++)
    {
        var c = engine[y, x];
        if(IsCiffre(c))
        {
            currentNumber.Add(c-48);
            (var isSymbolAround, var stars) = IsThereASymbolAround(engine, y, x);
            if (isSymbolAround)
            {
                foreach (var star in stars)
                {
                    if (!currentStars.Contains(star))
                    {
                        currentStars.Add(star);
                    }
                }
                symbol=true;
            }
        }
        else if(currentNumber.Count>0)
        {
            sumOfNumbers = ProcessWholeNumberReturnNewSum(currentNumber, currentStars, symbol, sumOfNumbers, x, y);
            symbol = false;
        }
        if (!IsCiffre(c))
        {
            //Console.ForegroundColor = foregroundColor;
            //Console.Write(c);
        }
    }
    if(currentNumber.Count>0)
    {
        sumOfNumbers = ProcessWholeNumberReturnNewSum(currentNumber, currentStars, symbol, sumOfNumbers, length+1, y);
        symbol = false;
    }
    //Console.WriteLine();
}

//Console.ForegroundColor = foregroundColor;
//Console.WriteLine(sumOfNumbers);

var sumOfMultipliedNumbers = 0;
foreach (var gear in gears.Where(g => g.Value.Count() == 2))
{
    var value1 = gear.Value[0].Item3;
    var value2 = gear.Value[1].Item3;
    var multi = value1 * value2;
    sumOfMultipliedNumbers += multi;
    Console.WriteLine($"{gear.Key} {value1} {value2} {multi}");
}
Console.WriteLine(sumOfMultipliedNumbers);

int ProcessWholeNumberReturnNewSum(List<int> ints, List<(int,int)> currentStars, bool isSymbolAround, int currentSum, int x, int y)
{
    var number = ints.Aggregate((a, b) => a * 10 + b);
    ints.Clear();
    if (isSymbolAround)
    {
        currentSum += number;
        //Console.ForegroundColor = ConsoleColor.Green;
        
        if(currentStars.Count>0)
        {
            foreach (var star in currentStars)
            {
                if(!gears.ContainsKey(star))
                {
                    var newListOfStars = new List<(int,int,int)>();
                    newListOfStars.Add((x,y,number));
                    gears.Add(star, newListOfStars);
                }
                else
                {
                    gears[star].Add((x,y,number));
                }
            }
        }
    }
    else
    {
        //Console.ForegroundColor = ConsoleColor.Red;
    }
    currentStars.Clear();
    //Console.Write(number);
    return currentSum;
}

(bool, List<(int,int)>) IsThereASymbolAround(char[,] chars, int i, int j)
{
    static bool IsSymbol(char c)
    {
        var isSymbol = c != '.' && c != '?' && c!= 0 && (c<48 || c > 58);
        //Console.WriteLine($"{c} {(int)c} {isSymbol}");
        return isSymbol;
    }
    var isSymbolAround = (IsSymbol(chars[i - 1, j - 1])
                          || IsSymbol(chars[i - 1, j])
                          || IsSymbol(chars[i - 1, j + 1])
                          || IsSymbol(chars[i, j - 1])
                          || IsSymbol(chars[i, j + 1])
                          || IsSymbol(chars[i + 1, j - 1])
                          || IsSymbol(chars[i + 1, j])
                          || IsSymbol(chars[i + 1, j + 1]));
    
    var stars = new List<(int,int)>();
    if (chars[i - 1, j - 1] == '*') stars.Add((i - 1, j - 1));
    if (chars[i - 1, j] == '*') stars.Add((i - 1, j));
    if (chars[i - 1, j + 1] == '*') stars.Add((i - 1, j + 1));
    if (chars[i, j - 1] == '*') stars.Add((i, j - 1));
    if (chars[i, j + 1] == '*') stars.Add((i, j + 1));
    if (chars[i + 1, j - 1] == '*') stars.Add((i + 1, j - 1));
    if (chars[i + 1, j] == '*') stars.Add((i + 1, j));
    if (chars[i + 1, j + 1] == '*') stars.Add((i + 1, j + 1));

    return (isSymbolAround, stars);
}

bool IsCiffre(char c1)
{
    return c1>47 && c1<58;
}
