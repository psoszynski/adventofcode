var input = File.ReadAllLines(@"./input.txt");

var elves = new List<Elf>();
var y = 0;
foreach (var line in input)
{
    var x = 0;
    foreach (var el in line)
    {
        if(el == '#') elves.Add(new Elf { Position = (x,y) });
        x++;
    }
    y++;
}

var order = new Queue<int>();
order.Enqueue(1);
order.Enqueue(2);
order.Enqueue(3);
order.Enqueue(4);


//PrintElves(elves);

var i = 1;
while(true)
{
    Console.WriteLine(i++);

    FindProposedPositions(elves, order);
    //check if any new ones

    var anyChanged = elves.Any(e => e.ProposedPosition.x != e.Position.x || e.ProposedPosition.y != e.Position.y);
    if(!anyChanged)
    {
        break;
    }

    var accepted = AcceptPositions(elves);
    foreach(var a in accepted)
    {
        a.Position = a.ProposedPosition;
    }
    order.Enqueue(order.Dequeue());

    //Console.WriteLine(i+1);
    //PrintElves(elves);
}

var xmin = elves.Select(e => e.Position.x).Min();
var xmax = elves.Select(e => e.Position.x).Max();
var ymin = elves.Select(e => e.Position.y).Min();
var ymax = elves.Select(e => e.Position.y).Max();

var countElves = elves
.Where(e => e.Position.x >= xmin
        && e.Position.x <= xmax
        && e.Position.y >= ymin
        && e.Position.y <= ymax)
.Count();

var emptyTiles = (xmax - xmin + 1) * (ymax - ymin + 1) - countElves;

Console.WriteLine(emptyTiles);

void PrintElves(List<Elf> elves)
{
    for (var j = -5; j < 15; j++)
    {
        for (var i = -5; i < 15; i++)
        {
            if (elves.Any(e => e.Position.x == i && e.Position.y == j))
            {
                Console.Write("X");
            }
            else
            {
                Console.Write(".");
            }
        }
        Console.WriteLine();
    }
}

void FindProposedPositions(List<Elf> elves, Queue<int> order)
{
    foreach(var my in elves)
    {
        my.ProposedPosition = (my.Position.x, my.Position.y);

        var anyElvesAround = elves.Any(
            e => (e.Position.x - 1 == my.Position.x && e.Position.y == my.Position.y) ||
                    (e.Position.x - 1 == my.Position.x && e.Position.y - 1 == my.Position.y) ||
                    (e.Position.x - 1 == my.Position.x && e.Position.y + 1 == my.Position.y) ||
                    (e.Position.x == my.Position.x && e.Position.y - 1 == my.Position.y) ||
                    (e.Position.x == my.Position.x && e.Position.y + 1 == my.Position.y) ||
                    (e.Position.x + 1 == my.Position.x && e.Position.y == my.Position.y) ||
                    (e.Position.x + 1 == my.Position.x && e.Position.y - 1 == my.Position.y) ||
                    (e.Position.x + 1 == my.Position.x && e.Position.y + 1 == my.Position.y));

        if(!anyElvesAround)
        {
            continue;
        }

        var foundProposed = false;
        foreach (var dir in order)
        {
            switch (dir)
            {
                case 1:
                    var north = elves.
                        Any(his => (his.Position.x == my.Position.x - 1
                        || his.Position.x == my.Position.x
                        || his.Position.x == my.Position.x + 1) &&
                            his.Position.y == my.Position.y - 1);
                    if (!north)
                    {
                        my.ProposedPosition = (my.Position.x, my.Position.y - 1);
                        foundProposed = true;
                    }
                    break;
                case 2:
                    var south = elves.
                    Any(his => (his.Position.x == my.Position.x - 1
                    || his.Position.x == my.Position.x
                    || his.Position.x == my.Position.x + 1) &&
                        his.Position.y == my.Position.y + 1);
                    if (!south)
                    {
                        my.ProposedPosition = (my.Position.x, my.Position.y + 1);
                        foundProposed = true;
                    }
                    break;
                case 3:
                    var west = elves.
                    Any(his => (his.Position.y == my.Position.y - 1
                    || his.Position.y == my.Position.y
                    || his.Position.y == my.Position.y + 1) &&
                        his.Position.x == my.Position.x - 1);
                    if (!west)
                    {
                        my.ProposedPosition = (my.Position.x - 1, my.Position.y);
                        foundProposed = true;
                    }
                    break;
                case 4:
                    var east = elves.
                    Any(his => (his.Position.y == my.Position.y + 1
                    || his.Position.y == my.Position.y
                    || his.Position.y == my.Position.y - 1) &&
                        his.Position.x == my.Position.x + 1);
                    if (!east)
                    {
                        my.ProposedPosition = (my.Position.x + 1, my.Position.y);
                        foundProposed = true;
                    }
                    break;
                default:
                    break;
            }
            if (foundProposed) break;
        }
        if(!foundProposed)
        {
            my.ProposedPosition = my.Position;
        }
    }
}

List<Elf> AcceptPositions(List<Elf> elves)
{
    var elvesWithAMove = elves
           .Where(elf => elf.ProposedPosition.x != elf.Position.x || elf.ProposedPosition.y != elf.Position.y);


    //foreach (var elf in elvesWithAMove)
    //{
    //    var elvesWithAMoveExceptItself =
    //        elvesWithAMove.Except(new List<Elf> { elf });
        
    //    var anyotherwithsameproposed = elvesWithAMoveExceptItself.Any(
    //        other =>
    //        other.ProposedPosition.x == elf.ProposedPosition.x
    //        && other.ProposedPosition.y == elf.ProposedPosition.y);

    //}

    var accepted = elvesWithAMove
        .Where(elf => !elvesWithAMove.Except(new List<Elf> { elf }).Any(
            other =>
            other.ProposedPosition.x == elf.ProposedPosition.x && other.ProposedPosition.y == elf.ProposedPosition.y))
        .ToList();

    return accepted;
}

public class Elf
{
    public (int x,int y) Position { get; set; }
    public (int x, int y) ProposedPosition { get; set; } = (int.MaxValue, int.MaxValue);
}
