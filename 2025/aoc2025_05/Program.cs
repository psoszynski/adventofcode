
using System.Diagnostics.SymbolStore;

var lines = await File.ReadAllLinesAsync("input1.txt");

var validRanges = new List<(long, long)>();
var ids = new List<long>();


long min = long.MaxValue;
long max = long.MinValue;

var ranges = true;
foreach(var line in lines)
{
    if (line == "") ranges = false;
    if (ranges)
    {
        var r = line.Split('-');
        var r1 = long.Parse(r[0]);
        var r2 = long.Parse(r[1]);
        if (r1 < min) min = r1;
        if (r2 > max) max = r2;
        validRanges.Add((r1, r2));
    }
    else
    {
        try
        {
            ids.Add(long.Parse(line));
        }
        catch (Exception) {}

    }
}

Console.WriteLine(min);
Console.WriteLine(max);

var diff = max - min;
Console.WriteLine(diff);

//part 1
//var validIds = new List<long>();
//foreach(var id in ids)
//{
//    for(var i=0; i<validRanges.Count; i++)
//    {
//        if (id >= validRanges[i].Item1 && id <= validRanges[i].Item2)
//        {
//            validIds.Add(id);
//            break;
//        }
//    }
//}

//foreach (var id in validIds)
//    Console.WriteLine(id);

//Console.WriteLine(validIds.Count);


//-------
   //-------------
   //                   -----
   //                     ---------------------  ----