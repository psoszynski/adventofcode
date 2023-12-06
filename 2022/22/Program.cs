var input = File.ReadAllLines("./tinput.txt");
var mapinput = input[0..^2].ToList();
var dirs = input[^1];
var maxlength = mapinput.Max(x => x.Length);

var map = CreateMap(mapinput);
var startPosition = input[0].Select((x, i) => (x, i)).FirstOrDefault(x => x.x == '.' || x.x == '#').i;
var stages = ParseStages(dirs);

var currentPosition = (0, startPosition, Facing.Right);
foreach(var stage in stages)
{
    currentPosition = ExecuteStage(stage, currentPosition, map);
}

(int y, int x, Facing facing) ExecuteStage(
    (int steps, char turn) stage,
    (int y, int x, Facing facing) position,
    (List<(int xmin, int xmax)> hLimits,
     List<(int ymin, int ymax)> vLimits,
     List<(int y, int x)> walls) map)
{
    var xmin = map.hLimits[position.y].xmin;
    var xmax = map.hLimits[position.y].xmax;
    var ymin = map.vLimits[position.x].ymin;
    var ymax = map.vLimits[position.x].ymax;
    var width = xmax - xmin + 1;
    var height = ymax - ymin + 1;
    var newFacing = position.facing.Turn(stage.turn);
    var allhWalls = map.walls.Where(w => w.y == position.y);
    var allvWalls = map.walls.Where(w => w.x == position.x);

    switch (position.facing)
    {
        case Facing.Right:
            var allhWallsOrdered = allhWalls.OrderBy(w => w.x);
            var newx = position.x + stage.steps;
            if (position.x + stage.steps <= xmax)
            {
                var wallsToRightInRange = allhWallsOrdered.Where(w => w.x > position.x && w.x < newx);
                if (wallsToRightInRange.Count() > 0)
                {
                    return (position.y, wallsToRightInRange.First().x - 1, newFacing);
                }
                else
                {
                    return (position.y, position.x + stage.steps, newFacing);
                }
            }
            return (position.y, allhWallsOrdered.First().x == xmin ? xmax : Math.Min(allhWallsOrdered.First().x - 1, newx - width), newFacing);
        case Facing.Down:
            var allvWallsOrdered = allvWalls.OrderBy(w => w.y);
            if (position.y + stage.steps <= ymax)
            {
                var wallsDownInRange = allvWallsOrdered.Where(w => w.y > position.y && w.y < position.y + stage.steps);
                if (wallsDownInRange.Count() > 0)
                {
                    return (wallsDownInRange.First().y - 1, position.x, newFacing);
                }
                else
                {
                    return (position.y + stage.steps, position.x, newFacing);
                }
            }
            return (allvWallsOrdered.First().y == ymin ? ymax : allvWallsOrdered.First().y - 1, position.x, newFacing);
        case Facing.Left:
            var allhWallsOrderedDescending = allhWalls.OrderByDescending(w => w.x);
            if (position.x - stage.steps >= xmin)
            {
                var wallsToLeftInRange = allhWallsOrderedDescending.Where(w => w.x < position.x && w.x > position.x - stage.steps);
                if (wallsToLeftInRange.Count() > 0)
                {
                    return (position.y, wallsToLeftInRange.First().x + 1, newFacing);
                }
                else
                {
                    return (position.y, position.x - stage.steps, newFacing);
                }
            }
            return (position.y, allhWallsOrderedDescending.First().x == xmax ? xmin : allhWallsOrderedDescending.First().x + 1, newFacing);
        case Facing.Up:
            var allvWallsOrderedDescending = allvWalls.OrderByDescending(w => w.x);
            if (position.y - stage.steps >= ymin)
            {
                var wallsUpInRange = allvWallsOrderedDescending.Where(w => w.y < position.y && w.y > position.y - stage.steps);
                if (wallsUpInRange.Count() > 0)
                {
                    return (wallsUpInRange.First().y + 1, position.x, newFacing);
                }
                else
                {
                    return (position.y - stage.steps, position.x, newFacing);
                }
            }
            return (allvWallsOrderedDescending.First().y == xmax ? xmin : allvWallsOrderedDescending.First().y + 1, position.x, newFacing);
        default:
            break;


    }

    return (1, 1, Facing.Right);
}

static List<(int, char)> ParseStages(string dirs)
{
    var stages = new List<(int, char)>();
    while (dirs.Length > 0)
    {
        var numberOfStepsList = dirs.TakeWhile(l => l != 'R' && l != 'L');
        var numberOfStepsString = string.Join("", numberOfStepsList);
        var numberOfSteps = int.Parse(numberOfStepsString);
        var l = numberOfStepsList.Count();
        if (l >= dirs.Length)
        {
            stages.Add((numberOfSteps, 'x'));
            break;
        }
        var stage = dirs[l++];
        stages.Add((numberOfSteps, stage));
        dirs = dirs[l..];
    }

    return stages;
}

(List<(int,int)> hLimits, List<(int, int)> vLimits, List<(int,int)> walls) CreateMap(List<string> mapinput)
{
    var hLimits = new List<(int, int)>();
    var vLimits = new List<(int, int)>();
    var walls = new List<(int, int)>();
    var maxlength = mapinput.Max(x => x.Length);
    var xmin = new int[maxlength];
    var xmax = new int[maxlength];
    for (int i = 0; i < maxlength; i++)
    {
        xmin[i] = -1;
        xmax[i] = -1;
    }
    var rownumber = 0;
    foreach (var row in mapinput)
    {
        var firstHashIndex = row.IndexOf('#');
        if (firstHashIndex < 0) firstHashIndex = int.MaxValue;
        var left = Math.Min(row.IndexOf('.'), firstHashIndex);
        var right = Math.Max(row.LastIndexOf('.'), row.LastIndexOf('#'));
        hLimits.Add((left, right));

        for (int j = 0; j < row.Length; j++)
        {
            if ((row[j] == '.' || row[j] == '#') && xmin[j]==-1)
            {
                xmin[j] = rownumber;
            }
            if (row[j] == '.' || row[j] == '#')
            {
                xmax[j] = rownumber;
            }
        }
        
        var hashes = row.Select((r, i) => (r, i)).Where(x => x.r == '#').Select(x => (rownumber, x.i));
        walls.AddRange(hashes);
        rownumber++;

    }
    for (int i = 0; i < xmin.Length; i++)
    {
        vLimits.Add((xmin[i], xmax[i]));
    }
    return (hLimits, vLimits, walls);
}

enum Facing
{
    Right,
    Down,
    Left,
    Up
}

static class Extensions
{
    public static Facing Turn(this Facing facing, char turn)
    {
        switch (facing,turn)
        {
            case (Facing.Right,'R'):
                return Facing.Down;
            case (Facing.Down, 'R'):
                return Facing.Left;
            case (Facing.Left,'R'):
                return Facing.Up;
            case (Facing.Up,'R'):
                return Facing.Right;

            case (Facing.Right,'L'):
                return Facing.Up;
            case (Facing.Down,'L'):
                return Facing.Right;
            case (Facing.Left,'L'):
                return Facing.Down;
            case (Facing.Up,'L'):
                return Facing.Left;

            default:
                return Facing.Right;
        }
    }
}