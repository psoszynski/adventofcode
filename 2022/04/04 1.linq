<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\04\input.txt");

input.Select(i =>
	g(i).a>=g(i).c && g(i).b<=g(i).d ||
	g(i).a<=g(i).c && g(i).b>=g(i).d ||
	g(i).a<=g(i).c && g(i).b<=g(i).d && g(i).b>=g(i).c ||
	g(i).a>=g(i).c && g(i).b>=g(i).d && g(i).a<=g(i).d )
.Count(i=>i)
.Dump();


(int a, int b, int c, int d) g(string l)
{
	var a = l.Split(",").First().Split("-").First();
	var b = l.Split(",").First().Split("-").Last();
	var c = l.Split(",").Last().Split("-").First();
	var d = l.Split(",").Last().Split("-").Last();
	return (int.Parse(a), int.Parse(b), int.Parse(c), int.Parse(d));
}






