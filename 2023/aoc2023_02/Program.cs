using System;
using System.IO;

class Program
{
    static void Main()
    {
        var input = File.ReadAllLines("real_input.txt");
        // var red = 12;
        // var green = 13;
        // var blue = 14;

        static string RemoveGameNumber(string line)
        {
            line = line.Substring(line.IndexOf(":") + 2);
            return line;
        }

        var linenumber = 0;
        //var sumoflinenumbers = 0;
        var sumpofpowers = 0;
        foreach (var gameline in input)
        {
            linenumber++;
            var game = RemoveGameNumber(gameline);
            var cubesets = game.Split(';');

            var maxred = 0;
            var maxgreen = 0;
            var maxblue = 0;

            //var gamePossible = true;
            foreach (var round in cubesets)
            {
                var cubes = round.Split(',');
                foreach (var cube in cubes)
                {
                    var c = cube.Trim();
                    var number = int.Parse(c.Split(" ")[0]);
                    var color = c.Split(" ")[1];

                    switch (color)
                    {
                        case "red":
                            if (number > maxred)
                            {
                                maxred = number;
                            }
                            // if(number > red)
                            // {
                            //     gamePossible = false;
                            // }
                            break;
                        case "green":
                            if (number > maxgreen)
                            {
                                maxgreen = number;
                            }
                            // if(number > green)
                            // {
                            //     gamePossible = false;
                            // }
                            break;
                        case "blue":
                            if (number > maxblue)
                            {
                                maxblue = number;
                            }
                            // if(number > blue)
                            // {
                            //     gamePossible = false;
                            // }
                            break;
                    }
                }
            }

            var power = maxred * maxgreen * maxblue;
            sumpofpowers += power;
            // if (gamePossible)
            // {
            //     sumoflinenumbers += linenumber;
            // }
        }
        Console.WriteLine(sumpofpowers);
    }
}

