<Query Kind="Statements" />

var input = File.ReadAllLines(@"C:\Users\psoszyns\programming\AOC2022\09\input.txt");

var h = (x: 0, y: 0);
var t = (x: 0, y: 0);
foreach(var c in input)
{
	(h, t) = CalculateNewTailPosition(h, t, c);
}

h.Dump();
t.Dump();

((int,int),(int,int)) CalculateNewTailPosition((int x,int y) h, (int x,int y) t, string command)
{
	var mt = (x:0, y:0);
	var mh = (x:0,y:0);
	var dir = command.Split(" ").First();
	var dis = int.Parse(command.Split(" ").Last());
	
	if(dir == "R") mh.x = dis;
	if (dir == "L") mh.x = -1 * dis;
	if (dir == "U") mh.y = dis;
	if (dir == "D") mh.y = -1 * dis;

	if (mh.x > 0)
	{
		if (h.x > t.x) mt.x = mh.x;
		else if (h.x == t.x && mh.x > 1) mt.x = mh.x - 1;
		else if (h.x < t.x && mh.x > 2) mt.x = mh.x - 2;
	}
	if (mh.x < 0)
	{
		if (h.x < t.x) mt.x = mh.x;
		else if (h.x == t.x && mh.x < -1) mt.x = mh.x + 1;
		else if (h.x > t.x && mh.x < -2) mt.x = mh.x + 2;
	}
	if (mh.y > 0)
	{
		if (h.y > t.y) mt.y = mh.y;
		else if (h.y == t.y && mh.y > 1) mt.y = mh.y - 1;
		else if (h.y < t.y && mh.y > 2) mt.y = mh.y - 2;
	}
	if (mh.y < 0)
	{
		if (h.y < t.y) mt.y = mh.y;
		else if (h.y == t.y && mh.y < -1) mt.y = mh.y + 1;
		else if (h.y > t.y && mh.y < -2) mt.y = mh.y + 2;
	}

	t.x += mt.x;
	t.y += mt.y;
	if (mt.x != 0)
	{
		t.y = h.y;
	}
	if (mt.y != 0)
	{
		t.x = h.x;
	}
	
	h.x+=mh.x;
	h.y+=mh.y;
	return (h,t);
}













