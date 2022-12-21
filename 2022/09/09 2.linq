<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\09\input.txt");

var list = new List<(int,int)>();
var tails = new (int,int)[9];
for(int i=0;i<9;i++) tails[i]=(0,0);
var h = (x: 0, y: 0);

foreach(var c in input)
{
	var dir = c.Split(" ").First();
	var dis = int.Parse(c.Split(" ").Last());
	if (dir == "R")
	{
		for (var i = 0; i < dis; i++)
		{
			h.x++;
			tails[0] = UpdateTail(h, tails[0]);
			for (int j = 1; j < 9; j++)
			{
				tails[j] = UpdateTail(tails[j-1], tails[j]);
			}
			list.Add(tails[8]);
		}
	}
	if (dir == "L")
	{
		for (var i = 0; i < dis; i++)
		{
			h.x--;
			tails[0] = UpdateTail(h, tails[0]);
			for (int j = 1; j < 9; j++)
			{
				tails[j] = UpdateTail(tails[j - 1], tails[j]);
			}
			list.Add(tails[8]);
		}
	}
	if (dir == "U")
	{
		for (var i = 0; i < dis; i++)
		{
			h.y++;
			tails[0] = UpdateTail(h, tails[0]);
			for (int j = 1; j < 9; j++)
			{
				tails[j] = UpdateTail(tails[j - 1], tails[j]);
			}
			list.Add(tails[8]);
		}
	}
	if (dir == "D")
	{
		for (var i = 0; i < dis; i++)
		{
			h.y--;
			tails[0] = UpdateTail(h, tails[0]);
			for (int j = 1; j < 9; j++)
			{
				tails[j] = UpdateTail(tails[j - 1], tails[j]);
			}
			list.Add(tails[8]);
		}
	}
}

var listDistinct = list.Distinct();
var count = listDistinct.Count();
count.Dump();

var minmax = (0,0,0,0);
foreach (var l in listDistinct)
{
	if (l.Item1 > minmax.Item1) minmax.Item1 = l.Item1;
	if (l.Item2 > minmax.Item2) minmax.Item2 = l.Item2;
	if (l.Item1 < minmax.Item3) minmax.Item3 = l.Item1;
	if (l.Item2 < minmax.Item4) minmax.Item4 = l.Item2;
}

var map = new char[minmax.Item2-minmax.Item4+1, minmax.Item1-minmax.Item3+1];

for(var i=0;i<minmax.Item2-minmax.Item4+1;i++)
	for(var j=0;j<minmax.Item1-minmax.Item3+1;j++)
		map[i,j]=' ';

foreach (var x in listDistinct)
{
	map[minmax.Item2-minmax.Item4-x.Item2+minmax.Item4, x.Item1-minmax.Item3] = 'X';
}

map.Dump();


(int,int) UpdateTail((int x,int y) h, (int x,int y) t)
{
	if (h.x > t.x + 1)
	{
		t.x++;
		t.y=h.y;
	}
	if (h.x < t.x - 1)
	{
		t.x--;
		t.y=h.y;
	}
	if (h.y > t.y + 1)
	{
		t.y++;
		t.x=h.x;
	}
	if (h.y < t.y - 1)
	{
		t.y--;
		t.x=h.x;
	}
	return t;
}













