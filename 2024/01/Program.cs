
//var input = File.ReadAllLines("testinput.txt");
var input = File.ReadAllLines("input.txt");

var fc = new List<int>();
var sc = new List<int>();
foreach (var line in input)
{
    fc.Add(int.Parse(line.Split("   ")[0]));
    sc.Add(int.Parse(line.Split("   ")[1]));
}

var fcsorted = fc.OrderBy(x => x).ToList();
var scsorted = sc.OrderBy(x => x).ToList();


var sum = 0;
for (int i = 0; i < fcsorted.Count; i++)
{
    var currentDifference = Math.Abs(fcsorted[i] - scsorted[i]);
    sum += currentDifference;
    //Console.WriteLine($"{fc[i]},{sc[i]}, {Math.Abs(fc[i] - sc[i])}");
}

Console.WriteLine(sum);

//part 2

sum = 0;
for (int i = 0; i < fcsorted.Count; i++)
{
    var c = sc.Count(x => x == fc[i]);
    var m = fc[i] * c;
    sum += m;
    //Console.WriteLine($"{fc[i]}, {c}, {m}");
}

Console.WriteLine(sum);