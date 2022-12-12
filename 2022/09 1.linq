<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\09\input.txt");

var list = new List<(int,int)>();
var tails = new (int,int)[9];
for(int i=0;i<9;i++) tails[i]=(0,0);
//var map = new char[500,500];
var h = (x: 0, y: 0);
var t = (x: 0, y: 0);
//map[h.x,h.y]='x';
foreach(var c in input)
{
	var dir = c.Split(" ").First();
	var dis = int.Parse(c.Split(" ").Last());
	if (dir == "R")
	{
		for (var i = 0; i < dis; i++)
		{
			h.x++;
			t = UpdateTail(h, t);
			list.Add(t);
			//map[t.x,t.y]='x';
		}
	}
	if (dir == "L")
	{
		for (var i = 0; i < dis; i++)
		{
			h.x--;
			t = UpdateTail(h, t);
			list.Add(t);
			//map[t.x,t.y]='x';
		}
	}
	if (dir == "U")
	{
		for (var i = 0; i < dis; i++)
		{
			h.y++;
			t = UpdateTail(h, t);
			list.Add(t);
			//map[t.x,t.y]='x';
		}
	}
	if (dir == "D")
	{
		for (var i = 0; i < dis; i++)
		{
			h.y--;
			t = UpdateTail(h, t);
			list.Add(t);
			//map[t.x,t.y]='x';
		}
	}
}

//var count = 0;
//for(var i=0;i<500;i++)
//	for(var j=0;j<500;j++)
//		if(map[i,j]=='x') count++;


var count = list.Distinct().Count();
count.Dump();
//map.Dump();

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













