using System.Text.RegularExpressions;

var filePath = "input.txt";
var text = File.ReadAllText(filePath);

var dosAndDonts = FindDoAndDontIndexes(text);

var originalColor = Console.ForegroundColor;

double mulTotal = 0;
double mulTotal2 = 0;
var startIndex = 0;
while (true)
{
    (startIndex, string mulOperation) = FindNextCorrectMul(text, startIndex);
    
    if(startIndex == -1)
    {
        break;
    }

    if (mulOperation.Length > 0)
    {
        var type = dosAndDonts.FirstOrDefault(x => x.index < startIndex).type;
        if (type == 1 || type == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else if (type == 2)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        Console.Write($"{mulOperation}");

        var commaIndex = mulOperation.IndexOf(',');
        var mul1 = int.Parse(mulOperation.Substring(4, commaIndex - 4));
        var mul2 = int.Parse(mulOperation.Substring(commaIndex + 1, mulOperation.IndexOf(')') - commaIndex - 1));
        var mulresult = mul1 * mul2;
        mulTotal += mulresult;

        if (type == 1 || type == 0)
        {
            mulTotal2 += mulresult;
        }
    }

    startIndex += mulOperation.Length;
    if (startIndex >= text.Length)
    {
        break;
    }
}

Console.ForegroundColor = originalColor;
Console.WriteLine();
Console.WriteLine(mulTotal);
Console.WriteLine(mulTotal2);

(int startIndex, string mulOperation) FindNextCorrectMul(string text, int startFrom)
{
    var startIndex = FindNextMul(text, startFrom);
    if(startIndex == -1)
    {
        return (text.Length, "");
    }
    (bool mulIsCorrect, var endIndex) = IsMulCorrect(text, startIndex);
    string mulOperation;
    if (mulIsCorrect)
    {
        mulOperation = text.Substring(startIndex, endIndex - startIndex+1);
        return (startIndex, mulOperation);
    }
    return (endIndex + 1 , "");
}

(bool,int) IsMulCorrect(string text, int index)
{
    var openBrackets = 1;
    var closeBrackets = 0;
    for (int i = index + 4; i < text.Length; i++)
    {
        if (text[i] == '(')
        {
            openBrackets++;
        }
        if (text[i] == ')')
        {
            closeBrackets++;
        }
        if (openBrackets == closeBrackets)
        {
            return (true,i);
        }
        if (openBrackets > closeBrackets && !char.IsDigit(text[i]) && text[i] != ',')
        {
            return (false,i);
        }
    }
    return (false,-1); // Return -1 if brackets are not balanced
}

int FindNextMul(string text, int index)
{
    for (int i = index; i < text.Length; i++)
    {
        if (text[i] == 'm' && text[i + 1] == 'u' && text[i + 2] == 'l' && text[i + 3] == '(')
        {
            return i;
        }
    }
    return -1;
}

List<(int index, int type)> FindDoAndDontIndexes(string text)
{
    var result = new List<(int index, int type)>();
    
    // Find all do() matches - type 1
    var doMatches = Regex.Matches(text, @"do\(\)");
    foreach (Match match in doMatches)
    {
        result.Add((match.Index, 1));
    }
    
    // Find all don't() matches - type 2
    var dontMatches = Regex.Matches(text, @"don't\(\)");
    foreach (Match match in dontMatches)
    {
        result.Add((match.Index, 2));
    }

    // Sort by index
    return result.OrderByDescending(x => x.index).ToList();
}