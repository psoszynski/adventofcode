using System.Data;
using System.Diagnostics;

var input = File.ReadAllLines("real_input.txt");
var seedsPart1 = input[0].Substring(7).Split(" ").Select(long.Parse).ToList();

var almanac = ReadAlmanac(input);

var s = new Stopwatch();
s.Start();

long location = -1;
while (true)
{
    var current = ++location;
    foreach (var m in almanac.Reverse())
    {
        current = ReverseMap(m.Key, current);
    }

    if (IsSeedPart2(seedsPart1, current))
    {
        break;
    }
}

s.Stop();
Console.WriteLine($"Lowest location: {location} time: {s.Elapsed.Seconds}");


#region old
// Console.WriteLine("creating matrix...");
// var s = new Stopwatch();
// s.Start();
// var matrix = CreateMatrix(almanac, max);
// s.Stop();
// Console.WriteLine($"Matrix created in {s.ElapsedMilliseconds}ms");

//matrix approach
// var lowestLocation = long.MaxValue;
// foreach(var value in seedsPart2)
// {
//     var current = matrix[value];
//     if (current < lowestLocation)
//     {
//         lowestLocation = current;
//     }
// }

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

//Console.WriteLine(lowestLocation);
#endregion

long Map(string mapping, long source)
{
    var records = almanac[mapping];
    foreach(var record in records)
    {
        // rocord: 49 53 4 -> source 53,54,55,56 is mapped to destination 49,50,51,52
        // if source is in the source range
        // source >= 53 AND source < 57 (53+4)
        if(source >= record.Item2 && source < record.Item2 + record.Item3)
        {
            var destination = record.Item1 + source - record.Item2;
            return destination;
        }
    }
    return source;
}

long ReverseMap(string mapping, long destination)
{
    var records = almanac[mapping];
    foreach(var record in records)
    {
        // rocord: 49 53 4 -> source 53,54,55,56 is mapped to destination 49,50,51,52
        // if destiantion is in the destination range
        // destination >= 49 AND destination < 53 (49+4)
        if(destination >= record.Item1 && destination < record.Item1 + record.Item3)
        {
            var source = record.Item2 + destination - record.Item1;
            return source;
        }
    }
    return destination;
}

bool IsSeedPart2(List<long> seedsPart1, long value)
{
    for (var i=0; i<seedsPart1.Count; i+=2)
    {
        if (value >= seedsPart1[i] && value < seedsPart1[i] + seedsPart1[i + 1])
        {
            return true;
        }
    }
    return false;
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
