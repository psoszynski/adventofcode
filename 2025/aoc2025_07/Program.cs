var lines = await File.ReadAllLinesAsync("input1.txt");

char[][] manifold = lines
    .Select(line => line.ToCharArray())
    .ToArray();

var color = Console.ForegroundColor;
var count = 0;

manifold[1][manifold[0].Length / 2] = '|';

for (var i = 2; i < manifold.Length;i+=2)
{
    var o = 0;
    foreach(var c in manifold[i])
    {
        if (manifold[i - 1][o] == '|')
        {
            if(manifold[i][o] == '^')
            {
                Console.ForegroundColor = ConsoleColor.Green;
                manifold[i + 1][o - 1] = '|';
                manifold[i + 1][o + 1] = '|';
                count++;
            }
            else
                manifold[i + 1][o] = '|';
        }
        else
            Console.ForegroundColor = color;
        Console.Write(c);
        o++;
    }
    Console.WriteLine();
}
Console.WriteLine($"Total cound: {count}");