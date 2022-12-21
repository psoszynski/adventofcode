<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\03\input.txt");

var x = input
.Select(i => (i.Substring(0,i.Length/2),i.Substring(i.Length/2)))
.Select(i => i.Item1.Intersect(i.Item2).First())
.Select(i => (i, i>96?i-96:i-38))
.Sum(i => i.Item2);
x.Dump();