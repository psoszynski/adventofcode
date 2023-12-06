var input = File.ReadAllLines("./input.txt");

// var sum = 0;
// foreach(var line in input)
// {
//     var prunedList = line.Where(l => (int)l < 58);
//     var x = int.Parse(prunedList.FirstOrDefault().ToString());
//     var y = int.Parse(prunedList.LastOrDefault().ToString());
//     var calibrationValue = 10 * x + y;
//     sum += calibrationValue;
// }

string[] numbers = {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

var sum = 0;
foreach(var line in input)
{
    (var x, var y) = FindFirstAndLastNumber(line);
    if (x.HasValue)
    {
        var calibrationValue = 10 * x.Value + y.Value;
        sum += calibrationValue;
    }
    
    Console.WriteLine($"{line} => {x} {y}");
}

(int? x, int? y) FindFirstAndLastNumber(string s)
{
    var i= 0;
    int? firstNumber = null;
    int? lastNumber = null;

    while (i < s.Length)
    {
        var currentNumber = s[i] < 58 ? int.Parse(s[i].ToString()) : GetNumberFromStringAtPosition(s, i);
        
        if (currentNumber != null)
        {
            if (firstNumber == null)
            {
                firstNumber = currentNumber;
                lastNumber = currentNumber;
            }
            else
            {
                lastNumber = currentNumber;
            }
        }
        i ++;
    }
    return (firstNumber, lastNumber);
}

int? GetNumberFromStringAtPosition(string s, int i)
{
    foreach(var n in numbers)
    {
        if(i+n.Length > s.Length)
        {
            continue;
        }
        var substring = s[i..(i + n.Length)];
        if (n == substring)
        {
            var index = Array.FindIndex(numbers, e => e == n);
            return index + 1;
        }
    }
    return null;
}

Console.WriteLine(sum);