<Query Kind="Statements" />

var inventory = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\01\input.txt");
var elves = new List<List<int>>();
var totals = new List<int>();
var currentElv = new List<int>();
var total = 0;

elves.Add(currentElv);
foreach(var inv in inventory)
{
	if(inv == "")
	{
		currentElv = new List<int>();
		elves.Add(currentElv);
		totals.Add(total);
		total = 0;
		continue;
	}
	var invValue = int.Parse(inv);
	total += invValue;
	currentElv.Add(invValue);
}
totals.Add(total);

totals.OrderByDescending(t=>t).Take(3).Sum().Dump();