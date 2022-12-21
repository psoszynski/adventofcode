using Spectre.Console;

var input = File.ReadAllText(@"C:\Users\psosz\programming\AdventOfCode2022\adventofcode\2022\Inputs\13\tinput.txt");
var pairs = input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
            .Select((e,i) => (i,e.Split($"{Environment.NewLine}")));

var sumOfIndices = 0;
foreach(var pair in pairs)
{
    Console.WriteLine(pair.Item1+1);
    var first = pair.Item2[0];
    var second = pair.Item2[1];

    var t1 = new Tree("A");
    var topNode1 = t1.AddNode(new Text(first));
    var parsed1 = ParseELement(first, topNode1);

    var t2 = new Tree("B");
    var topNode2 = t1.AddNode(new Text(second));
    var parsed2 = ParseELement(second, topNode2);

    var compareResult = Compare(parsed1, parsed2);
    if (!compareResult.HasValue) compareResult = true;
    Console.WriteLine(compareResult);
    if(compareResult==true)
    {
        sumOfIndices += pair.Item1+1;
    }
    Console.WriteLine(sumOfIndices);
    Console.WriteLine();
}

Console.WriteLine(sumOfIndices);

bool? Compare(Element e1, Element e2)
{
    Console.WriteLine($"Compare {e1.Content} vs {e2.Content}");

    if (e1.Content == "[]" && e2.Content == "[]")
    {
        e1.Content = $"[0]";
        e1.Children.Add(new Element { Content = "0" });

        e2.Content = $"[0]";
        e2.Children.Add(new Element { Content = "0" });
    }

    if (e1.Children.Count == 0 && (e2.Children.Count > 0 || e2.Content == "[]") && int.TryParse(e1.Content, out _))
    {
        var c = e1.Content;
        e1.Content = $"[{c}]";
        e1.Children.Add(new Element { Content = c });
    }

    if ((e1.Children.Count > 0 || e1.Content == "[]") && e2.Children.Count == 0 && int.TryParse(e2.Content, out _))
    {
        var c = e2.Content;
        e2.Content = $"[{c}]";
        e2.Children.Add(new Element { Content = c });
    }

    if (e1.Children.Count == 0 && e2.Children.Count == 0)
    {
        if(int.Parse(e1.Content) > int.Parse(e2.Content))
        {
            return false;
        }
        else if (int.Parse(e1.Content) < int.Parse(e2.Content))
        {
            return true;
        }
    }

    if (e1.Content == "[]" && e2.Content != "[]")
    {
        return true;
    }

    if (e2.Content == "[]" && e1.Content != "[]")
    {
        return false;
    }

    var i = 0;
    bool? childrenBool = null;
    foreach (var child in e1.Children)
    {
        if (i >= e2.Children.Count) return false;
        childrenBool =  Compare(child, e2.Children[i++]);
        if(childrenBool.HasValue) return childrenBool;
    }
    return childrenBool;
}

Element ParseELement(string element, TreeNode node)
{
    var elementTop = new Element();
    elementTop.Content = element;
    elementTop = elementTop.GetElementFrom(0);
    PopulateElements(elementTop, node );

    void PopulateElements(Element element, TreeNode tin)
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
            var node = tin.AddNode(new Text(subelement.Content));
            if (subelement.Content.Length == 0) break;
            start = start + subelement.Content.Length;
            if (element.Content[start] == ',') start++;
            PopulateElements(subelement, node);
        }
    }
    return elementTop;
}

public class Element
{
    public string Content { get; set; }
    public List<Element> Children { get; set; } = new List<Element>();
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
