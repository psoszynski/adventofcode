var input = File.ReadAllLines("real_input2.txt");
var times = input[0].Substring(11).Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();
var distances = input[1].Substring(11).Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();

for(var race=0; race<times.Count; race++)
{
    long winningPossibilities = 0;
    for (var index = 1; index < times[race]; index++)
    {
        if (ÏsWinning(times[race], distances[race], index))
        {
            winningPossibilities++;
        }
    }
    Console.WriteLine(winningPossibilities);
    
bool ÏsWinning(long raceTime, long record, long timeTry)
    => (raceTime - timeTry) * timeTry > record;
