List<Element> elements = File.ReadAllLines(@"C:\Users\psosz\programming\AdventOfCode2022\adventofcode\2022\Inputs\13\input.txt")
    .Where(i => i != "")
    .Select(ParseELement)
    .ToList();

elements.Sort();
elements.Reverse();

foreach (var e in elements)
{
    Console.WriteLine(e.Content);
}

var i1 = elements
    .Select((e, i) => (e, i))
    .FirstOrDefault(e => e.e.Content == "[[2]]")
    .i+1;
var i2 = elements
    .Select((e, i) => (e, i))
    .FirstOrDefault(e => e.e.Content == "[[6]]")
    .i+1;
Console.WriteLine($"{i1} {i2} {i1*i2}");

Element ParseELement(string element)
{
    var elementTop = new Element();
    elementTop.Content = element;
    elementTop = elementTop.GetElementFrom(0);
    PopulateElements(elementTop);

    void PopulateElements(Element element)
    {
        if (element.Content[0] != '[')
        {
            return;
        }
        var start = 1;
        while (true)
        {
            if (element.Content[start] == ']' || start > element.Content.Length) return;
            var subelement = element.GetElementFrom(start);
            //Console.WriteLine(subelement.Content);
            element.Children.Add(subelement);
            if (subelement.Content.Length == 0) break;
            start = start + subelement.Content.Length;
            if (element.Content[start] == ',') start++;
            PopulateElements(subelement);
        }
    }
    return elementTop;
}

public class Element : IComparable<Element>
{
    public string Content { get; set; }
    public List<Element> Children { get; set; } = new List<Element>();

    public int CompareTo(Element? other)
    {
        if (other == null) return 1;

        //Console.WriteLine($"Compare {Content} vs {other.Content}");

        if (Content == other.Content)
        {
            return 0;
        }

        if (Children.Count == 0 && (other.Children.Count > 0 || other.Content == "[]") && int.TryParse(this.Content, out _))
        {
            var c = Content;
            Content = $"[{c}]";
            Children.Add(new Element { Content = c });
        }

        if ((Children.Count > 0 || Content == "[]") && other.Children.Count == 0 && int.TryParse(other.Content, out _))
        {
            var c = other.Content;
            other.Content = $"[{c}]";
            other.Children.Add(new Element { Content = c });
        }

        if (Children.Count == 0 && other.Children.Count == 0)
        {
            if (int.Parse(Content) > int.Parse(other.Content))
            {
                return -1;
            }
            else if (int.Parse(Content) < int.Parse(other.Content))
            {
                return 1;
            }
            else { return 0; }
        }

        if (Content == "[]" && other.Content != "[]")
        {
            return 1;
        }

        if (other.Content == "[]" && Content != "[]")
        {
            return -1;
        }

        var i = 0;
        foreach (var child in this.Children)
        {
            if (i >= other.Children.Count) return -1;
            var childrenCompare = child.CompareTo(other.Children[i++]);
            if(childrenCompare != 0)
            {
                return childrenCompare;
            }
        }

        if(i<other.Children.Count)
        {
            return 1;
        }
        return 0;
    }

    public Element GetElementFrom(int start)
    {
        if (Content[start] > 47 && Content[start] < 58)
        {
            var number = Content[start].ToString();
            while (true)
            {
                var nextchar = Content[++start];
                if (nextchar > 47 && nextchar < 58)
                {
                    number += nextchar.ToString();
                }
                else
                {
                    var element = new Element();
                    element.Content = number;
                    return element;
                }

            }
        }
        if (Content[start] != '[') throw new Exception("Not an element");
        var i = start;
        var c = 0;
        while (true)
        {
            if (Content[i] == ']') c++;
            if (Content[i] == '[') c--;
            if (c == 0)
            {
                var element = new Element();
                element.Content = Content.Substring(start, i - start + 1);
                return element;
            }
            i++;
        }
    }
}
