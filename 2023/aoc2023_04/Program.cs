var lines = File.ReadAllLines("real_input.txt");

//var sum = 0;
var scratchCards = new List<int>();

int i = 1;
foreach (var line in lines)
{
    var card = line.Substring(line.IndexOf(":")+2);
    var winningNumbers = card.Split("|")[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var yourNumbers = card.Split("|")[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var myWinningNumbers = winningNumbers.Intersect(yourNumbers);
    var myWinningNumbersCount = myWinningNumbers.Count();

    scratchCards.Add(i);
    var numberOfCopies = scratchCards.Where(n => n == i).Count();
    for(var j = 0; j < numberOfCopies; j++)
    {
        Enumerable.Range(i+1, myWinningNumbersCount).ToList().ForEach(x => scratchCards.Add(x));
    }

    i++;
    // var points = myWinningNumbersCount > 0 ? (int)Math.Pow(2, myWinningNumbersCount - 1) : 0;
    // sum+=points;
}

// foreach(var n in scratchCards)
// {
//     Console.Write(n + " ");
// }

Console.WriteLine();
Console.WriteLine(scratchCards.Count());
