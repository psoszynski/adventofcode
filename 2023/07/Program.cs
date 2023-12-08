var input = File.ReadAllLines("real_input.txt");

var hands = input.Select((el) => (el.Split(" ")[0], el.Split(" ")[1])).ToList();
var orderedHands = hands.OrderBy(h => h.Item1, new HandComparer());

try
{
    checked
    {
        var sum = orderedHands.Select((e, i) => int.Parse(e.Item2) * (i+1)).Sum();
        Console.WriteLine(sum);
    }
}
catch (OverflowException e)
{
    Console.WriteLine(e.Message);
}

public class HandComparer : IComparer<string>
{ 
    public int Compare(string h1, string h2)
    {
        if (string.Equals(h1, h2)) return 0;
        if (GetStrengthJokerVersion(h1) > GetStrengthJokerVersion(h2)) return 1;
        if (GetStrengthJokerVersion(h1) < GetStrengthJokerVersion(h2)) return -1;
        for (int i = 0; i < h1.Length; i++)
        {
            if (f2(h1[i]) == f2(h2[i])) continue;
            if (f2(h1[i]) > f2(h2[i])) return 1;
            if (f2(h1[i]) < f2(h2[i])) return -1;
        }
        return 0;
    }
    
    bool IsFiveOfAKind(string hand) => hand.Distinct().Count()==1;
    bool IsFourOfAKind(string hand) => 
        hand.Count(i => i == hand[0])==4 || hand.Count(i => i == hand[1])==4;
    bool IsFullHouse(string hand) =>
        hand.Distinct().Count() == 2 &&
        (hand.Count(i => i == hand[0])==2 || hand.Count(i => i == hand[0])==3);
    bool IsThreeOfKind(string hand) =>
        hand.Distinct().Count() == 3 &&
        (hand.Count(i => i == hand[0])==3 || hand.Count(i => i == hand[1])==3 || hand.Count(i => i == hand[3])==3);
    bool IsTwoPair(string hand) =>
        hand.Distinct().Count() == 3 &&
        (hand.Count(i => i == hand[0])==2 || hand.Count(i => i == hand[1])==2 || hand.Count(i => i == hand[3])==2);
    bool IsOnePair(string hand) =>
        hand.Distinct().Count() == 4;
    bool IsHighCard(string hand) =>
        hand.Distinct().Count() == 5;
    
    // part 2 with Jokers
    bool ChangeToJokerVersion(Func<string, bool> func, string hand)
    {
        if(func(hand)) return true;
        foreach(var c in "AKQT98765432")
        {
            if(func(hand.Replace('J',c))) return true;
        }
        return false;
    }
    
    int GetStrength(string hand)
    {
        if (IsFiveOfAKind(hand)) return 7;
        if (IsFourOfAKind(hand)) return 6;
        if (IsFullHouse(hand)) return 5;
        if (IsThreeOfKind(hand)) return 4;
        if (IsTwoPair(hand)) return 3;
        if (IsOnePair(hand)) return 2;
        if (IsHighCard(hand)) return 1;
        return 0;
    }

    int GetStrengthJokerVersion(string hand)
    {
        if(ChangeToJokerVersion(IsFiveOfAKind,hand)) return 7;
        if(ChangeToJokerVersion(IsFourOfAKind,hand)) return 6;
        if(ChangeToJokerVersion(IsFullHouse,hand)) return 5;
        if(ChangeToJokerVersion(IsThreeOfKind,hand)) return 4;
        if(ChangeToJokerVersion(IsTwoPair,hand)) return 3;
        if(ChangeToJokerVersion(IsOnePair,hand)) return 2;
        if(ChangeToJokerVersion(IsHighCard,hand)) return 1;
        return 0;
    }
    
    int f(char c) => c switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        < 'a' => c - 48,	
        _ => 0
    };
    
    int f2(char c) => c switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 1,
        'T' => 10,
        < 'a' => c - 48,	
        _ => 0
    };
    
}