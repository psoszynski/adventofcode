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

var heighest = 0;
for (int i = 1; i < input.Length - 1; i++)
{
	for (int j = 1; j < input[0].Length - 1; j++)
	{
		var t = input[i][j];
		//check top
		var topList = columns[j - 1].Take(i).Reverse().ToList();
		var sTop = FindScenicDistanace(topList, t);
		//check right
		var rightList = input[i].Substring(j + 1);
		var sRight = FindScenicDistanace(rightList.ToCharArray().ToList(), t);
		//check bottom
		var bottomList = columns[j - 1].Skip(i + 1).ToList();
		var sBottom = FindScenicDistanace(bottomList, t);
		//check left
		var leftList = input[i].Substring(0,j);
		var sLeft = FindScenicDistanace(leftList.Reverse().ToList(), t);

		var scenicValue = sTop*sLeft*sRight*sBottom;
		if(scenicValue>heighest) heighest=scenicValue;
		//Console.WriteLine($"{t} {scenicValue}");
	}
}

heighest.Dump("Heighest scenic value");
int FindScenicDistanace(List<char> list, char myHeight)
{
	var x = list.Select((value, index) => new { value, index = index + 1 })
				.Where(pair => pair.value >= myHeight)
				.Select(pair => pair.index)
				.FirstOrDefault();
	if (x == 0)
	{
		return list.Count();
	}
	return x;
}
