<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\03\input.txt");

var allSacks = input
.Select((i,index) => (i,index/3));

var groupsOf3 = from i in allSacks
		group i.Item1 by i.Item2 into g
		select (g.First(),g.Skip(1).Take(1).First(),g.Last());

var sum = groupsOf3
.Select(g => g.Item1.ToCharArray()
	.Intersect(g.Item2.ToCharArray())
	.Intersect(g.Item3.ToCharArray()).First())
.Select(i => (i, i>96?i-96:i-38))
.Sum(i => i.Item2);

sum.Dump();
