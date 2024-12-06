var lines = File.ReadAllLines("input.txt");
var lines1 = lines.TakeUntil(l => l == "").ToArray();
var lines2 = lines.SkipWhile(l => l != "").Skip(1).ToArray();

var rules = lines1.Select(l => (int.Parse(l.Split("|")[0]), int.Parse(l.Split("|")[1]))).ToArray();
var updates = lines2.Select(l => l.Split(",").Select(int.Parse).ToList()).ToList();

var sum=0;
foreach(var update in updates)
{
    Console.WriteLine(string.Join(",", update));
    var ok = true;
    for (var n=0; n<update.Count; n++)
    {
        var ok_inner = true;
        for(var n2=n+1; n2<update.Count; n2++)
        {
            var number1 = update[n];
            var number2 = update[n2];

            //first lets find breakinng rules
            var breakingRule = rules.FirstOrDefault(r => r.Item1 == number2 && r.Item2 == number1);
            if(breakingRule != default)
            {
                Console.WriteLine($"Breaking rule: {number2} -> {number1}");
                ok_inner = false;
                break;
            }

            // var rule = rules.FirstOrDefault(r => r.Item1 == number1 && r.Item2 == number2);
            // if(rule == default)
            // {
            //     Console.WriteLine($"Rule not found: {update[n]} -> {update[n2]}");
            //     break;
            // }
        }
        if (!ok_inner)
        {
            ok = false;
        }
    }
    if(ok)
    {
        var middleIndex = update.Count / 2;
        var middleNumber = update[middleIndex];
        sum+=middleNumber;
    }
}

Console.WriteLine(sum);



public static class EnumerableExtensions
{
    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
             if (predicate(item))
                yield break;
            yield return item;

        }
    }
}