<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\05\input.txt");
var e = input.Select((e,i)=>(e,i)).Where(i => i.e==string.Empty).First().Item2-1;
var stacks = new List<Stack>();
int numberOfStacks = (input.First().Length/4)+1;
stacks.AddRange(Enumerable.Range(1,numberOfStacks).Select(en => new Stack()));

foreach(var element in input.Take(e).Reverse())
{
	for(var i=0;i<numberOfStacks;i++)
	{
		var letter = element[i*4+1];
		if (char.IsWhiteSpace(letter)) continue;
		stacks[i].Push(letter);
	}
}

foreach(var command in input.Skip(e+2))
{
	var groups = Regex.Match(command, @"[^0-9]+([0-9]+)[^0-9]+([0-9]+)[^0-9]+([0-9]+)").Groups;
	var a = int.Parse(groups[1].Value);
	var b = int.Parse(groups[2].Value);
	var c = int.Parse(groups[3].Value);
	MoveFromTo(stacks,a,b,c);
}

stacks.Dump();

void MoveFromTo(List<Stack> s, int n, int f, int t)
{
	var temps = new List<object>();
	for(var i=0;i<n;i++)
	{
		temps.Add(s[f-1].Pop());
	}
	for(var i=temps.Count();i>0;i--)
	{
		s[t-1].Push(temps[i-1]);
	}
}