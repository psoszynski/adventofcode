<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\02\input.txt");
//A - 1, B - 2, C - 3
var sum = 0;
foreach(var r in input)
{
	var s = r switch
	{
		"A X" => 1+3, // rock rock - draw 3
		"A Y" => 2+6, // rock paper - win 6
		"A Z" => 3+0, // rock scissors - loss 0
		"B X" => 1+0, // paper rock - loss 0
		"B Y" => 2+3, // paper paper - draw 3
		"B Z" => 3+6, // paper scissors - win 6
		"C X" => 1+6, // scissors rock - win 6
		"C Y" => 2+0, // scissors paper - loss 0
		"C Z" => 3+3, // scissors scissors -3
		_ => 0
	};
	sum+=s;
	//Console.WriteLine($"{r} {s}");
}

sum.Dump();





