using System.Text;

var input = File.ReadAllLines(@"./input.txt");

double sum = 0;
foreach (var i in input)
{
    var value = ConvertFromSnafu(i);
    sum += value;
}

var snafu = ConvertToSnafu((Int64)sum);
Console.WriteLine(snafu);

//var converted = ConvertFromSnafu(snafu);
//Console.Write(converted);

double ConvertFromSnafu(string snafu)
{
    double result = 0;
    var i = 0;
    foreach (var s in snafu.Reverse())
    {
        if (s == '-')
        {
            result -= Math.Pow(5, i);
        }
        else if (s == '=')
        {
            result -= 2 * Math.Pow(5, i);
        }
        else if (s == '1')
        {
            result += Math.Pow(5, i);
        }
        else if (s == '2')
        {
            result += 2 * Math.Pow(5, i);
        }
        i++;
    }
    return result;
}

string ConvertToSnafu(Int64 n)
{
    var baseNum = 5;
    var s = new Stack<char>();
    do
    {
        var x = n % baseNum;
        if (x == 4)
        {
            s.Push('-');
            n += 5;
        }
        else if (x == 3)
        {
            s.Push('=');
            n += 5;
        }
        else
        {
            s.Push(x.ToString()[0]);
        }
        n /= baseNum;

    } while (n != 0);

    var sb = new StringBuilder();
    while (true)
    {
        if (s.Count == 0) break;
        var i = s.Pop();
        sb.Append(i);
    }
    return sb.ToString();
}
