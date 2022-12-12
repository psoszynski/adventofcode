<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\08\input.txt");

var columns = new List<List<char>>();
for (int j = 1; j < input[0].Length - 1; j++)
{
	var c = new List<char>();
	for (int i = 0; i < input.Length; i++)
	{
		c.Add(input[i][j]);
	}
	columns.Add(c);
}

var visibleCount=0;
for (int i = 1; i < input.Length - 1; i++)
{
	for (int j = 1; j < input[0].Length - 1; j++)
	{
		var t = input[i][j];
		//check left
		var left = input[i].Substring(0,j);
		var visibleLeft = left.Max()<t;
		//check right
		var right = input[i].Substring(j+1);
		var visibleRight = right.Max() < t;
		//check top
		var top = columns[j-1].Take(i);
		var visibleTop = top.Max() < t;
		//check bottom
		var bottom = columns[j - 1].Skip(i+1);
		var visibleBottom = bottom.Max() < t;
		
		//Console.Write(t);
		//if (visibleLeft) Console.Write("+"); else Console.Write("-");
		//if (visibleRight) Console.Write("+"); else Console.Write("-");
		//if (visibleTop) Console.Write("+"); else Console.Write("-");
		//if (visibleBottom) Console.Write("+"); else Console.Write("-");
		var visible = visibleLeft || visibleRight || visibleTop || visibleBottom;
		//if (visible) Console.Write("+"); else Console.Write("-");
		if(visible) visibleCount++;
	}
	//Console.WriteLine();
}

var x = input[0].Length*2;
var y = input.Length*2;
var z = visibleCount;

var result = x + y + z - 4;

result.Dump();

