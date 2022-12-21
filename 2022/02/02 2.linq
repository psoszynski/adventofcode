<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\02\input.txt");

// X - you need to lose, Y - you need to draw, Z - you need to win

var sum = 0;
foreach(var r in input)
{
	var s = r switch
	{
		"A X" => 3+0,
		"A Y" => 1+3,
		"A Z" => 2+6,
		"B X" => 1+0,
		"B Y" => 2+3,
		"B Z" => 3+6,
		"C X" => 2+0,
		"C Y" => 3+3,
		"C Z" => 1+6,
		_ => 0
	};
	sum+=s;
	//Console.WriteLine($"{r} {s}");
}

sum.Dump();





