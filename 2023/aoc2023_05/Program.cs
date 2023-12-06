using System.Data;
using System.Diagnostics;

var input = File.ReadAllLines("real_input.txt");
var seedsPart1 = input[0].Substring(7).Split(" ").Select(long.Parse).ToList();

var seedsPart2 = new List<long>();
for (var i=0; i<seedsPart1.Count; i+=2)
{
    Console.WriteLine(seedsPart1[i + 1]);
    for (var j=0; j < seedsPart1[i+1]; j++)
    {
        var seed = seedsPart1[i] + j;
        seedsPart2.Add(seed);
    }
}

var max = seedsPart2.Max();
Console.WriteLine($"max={max}");

var almanac = ReadAlmanac(input);

Console.WriteLine("creating matrix...");
var s = new Stopwatch();
s.Start();
var matrix = CreateMatrix(almanac, max);
s.Stop();
Console.WriteLine($"Matrix created in {s.ElapsedMilliseconds}ms");

//matrix approach
var lowestLocation = long.MaxValue;
foreach(var value in seedsPart2)
{
    var current = matrix[value];
    if (current < lowestLocation)
    {
        lowestLocation = current;
    }
}

//mapping for each value approach
//foreach(var value in seedsPart2)
//{
//    var current = value;
//    foreach (var m in almanac)
//    {
//        current = Map(m.Key, current);
//    }
//    if (current < lowestLocation)
//    {
//        lowestLocation = current;
//    }
//}

Console.WriteLine(lowestLocation);

long Map(string mapping, long input)
{
    var records = almanac[mapping];

    foreach(var record in records)
    {
        if(input >= record.Item2 && input < record.Item2 + record.Item3)
        {
            var ret = record.Item1 + input - record.Item2;
            return ret;
        }
    }
    return input;
}

Dictionary<string, List<(long,long,long)>> ReadAlmanac(string[] input)
{
    var almanac = new Dictionary<string, List<(long, long, long)>>();
    var mappings = new[]
    {
        "seed-to-soil",
        "soil-to-fertilizer",
        "fertilizer-to-water",
        "water-to-light",
        "light-to-temperature",
        "temperature-to-humidity",
        "humidity-to-location"
    };

    foreach(var m in mappings)
    {
        var indexStartForMapping = input
            .Select((row, index) => new { row, index })
            .FirstOrDefault(i => i.row.Contains(m))
            ?.index;
        if(indexStartForMapping == null)
        {
            Console.WriteLine($"Mapping {m} not found");
            continue;
        }
        var records = input.Skip(indexStartForMapping.Value + 1)
            .TakeWhile(i => !string.IsNullOrWhiteSpace(i))
            .Select(i => i.Split(" "))
            .Select(i => (long.Parse(i[0]), long.Parse(i[1]), long.Parse(i[2])))
            .ToList();

        almanac.Add(m, records);
    }
    return almanac;
}

Dictionary<long,long> CreateMatrix(Dictionary<string, List<(long, long, long)>> almanac, long max)
{
    var matrix = new Dictionary<long, long>();
    for(long i=0; i<=max; i++)
    {
        var current = i;
        foreach (var m in almanac)
        {
            current = Map(m.Key, current);
        }
        matrix.Add(i, current);
    }
    return matrix;
}
