var input = File.ReadAllLines("./input2.txt");
(var monkeys, var leastCommonMultiple) = ReadMonkeysFromInput(input);

//part 1 - 20 roounds
//part 2 - 1000 roundsz
for(var round = 0;round<10000;round++)
{
    ExecuteRound(ref monkeys);
}

foreach(var m in monkeys)
{
    Console.WriteLine($"{m.NumberOfInspectedItems} - {string.Join(",",m.Items)}");
}

var top2 = monkeys.Select(m => m.NumberOfInspectedItems).OrderByDescending(m => m).Take(2);
long mult = 1;
foreach(var m in top2)
{
    mult *= m;
}
Console.WriteLine(mult);

void ExecuteRound(ref List<Monkey> monkeys)
{
    foreach(var monkey in monkeys)
    {
        while(monkey.Items.TryDequeue(out long item))
        {
            var worryLevel = monkey.Operation(item);
            var newWorryLevel = worryLevel %= leastCommonMultiple;

            //if (newWorryLevel != worryLevel) Console.WriteLine($"{worryLevel} {newWorryLevel} {monkey.DivisibleBy} {leastCommonMultiple}");

            var isDivisible = (newWorryLevel % monkey.DivisibleBy) == 0;
            if (isDivisible)
            {
                monkeys[monkey.TrueMonkey].Items.Enqueue(newWorryLevel);
            }
            else
            {
                monkeys[monkey.FalseMonkey].Items.Enqueue(newWorryLevel);
            }
            monkey.NumberOfInspectedItems++;
        }
    }
}

static (List<Monkey>, long) ReadMonkeysFromInput(string[] input)
{
    var monkeys = new List<Monkey>();
    Monkey currentMonkey = null!;
    foreach (var line in input)
    {
        var t = "";
        int indexOf = 0;
        if (line.Contains("Monkey"))
        {
            if (currentMonkey != null)
            {
                monkeys.Add(currentMonkey);
            }
            currentMonkey = new Monkey();
        }
        t = "Starting items:";
        indexOf = line.IndexOf(t);
        if (indexOf>0)
        {
            var listOfItems = line.Substring(indexOf+t.Length+1).Split(',').Select(i => long.Parse(i));
            foreach (var item in listOfItems)
            {
                currentMonkey.Items.Enqueue(item);
            }
        }
        t = "Operation: new = ";
        indexOf = line.IndexOf(t);
        if (indexOf > 0)
        {
            var operation = line.Substring(indexOf+t.Length+4).Split(" ").First();
            var value = line.Split(" ").Last();
            if (long.TryParse(value, out long o))
            {
                if (operation == "+") currentMonkey.Operation = x => x + o;
                if (operation == "*") currentMonkey.Operation = x => x * o;
                if (operation == "-") currentMonkey.Operation = x => x - o;
                if (operation == "/") currentMonkey.Operation = x => x / o;
            }
            else
            {
                if (operation == "+") currentMonkey.Operation = x => x + x;
                if (operation == "*") currentMonkey.Operation = x => x * x;
                if (operation == "-") currentMonkey.Operation = x => 0;
                if (operation == "/") currentMonkey.Operation = x => 1;
            }
        }
        t = "divisible by";
        indexOf = line.IndexOf(t);
        if (indexOf > 0)
        {
            var s = line.Substring(indexOf + t.Length);
            var divOp = long.Parse(s);
            currentMonkey.DivisibleBy = divOp;
        }
        t = "If true: throw to monkey";
        indexOf = line.IndexOf(t);
        if (indexOf>0)
        {
            var s = line.Substring(indexOf + t.Length);
            var monkeyNr = int.Parse(s);
            currentMonkey.TrueMonkey = monkeyNr;
        }
        t = "If false: throw to monkey";
        indexOf = line.IndexOf(t);
        if (indexOf > 0)
        {
            var s = line.Substring(indexOf + t.Length);
            var monkeyNr = int.Parse(s);
            currentMonkey.FalseMonkey = monkeyNr;
        }
    }
    monkeys.Add(currentMonkey);

    var leastCommonMultiple = 1L;
    foreach (var monkey in monkeys)
        leastCommonMultiple *= monkey.DivisibleBy;

    return (monkeys, leastCommonMultiple);
}

public class Monkey
{
    public Queue<long> Items { get; set; } = new Queue<long>();
    public Func<long, long> Operation { get; set; }
    public long DivisibleBy { get; set; }
    public int TrueMonkey { get; set; }
    public int FalseMonkey { get; set; }
    public long NumberOfInspectedItems { get; set; }
}