
var input = await File.ReadAllLinesAsync("input1.txt");

Int64 sumOfJoltages = 0;
foreach (var line in input)
{
    var inputSpan = line.AsSpan();
    var largestJoltage = GetLargestJoltagePart2(inputSpan);
    sumOfJoltages += largestJoltage;
    Console.WriteLine($"{inputSpan}: {largestJoltage}");
}
Console.WriteLine($"Sum of joltages: {sumOfJoltages}");

Int64 GetLargestJoltagePart2(ReadOnlySpan<char> inputSpan)
{
    const int targetLength = 12;
    if (inputSpan.Length < targetLength)
        throw new ArgumentException($"Input must contain at least {targetLength} digits.", nameof(inputSpan));

    // Number of digits we must remove
    int toRemove = inputSpan.Length - targetLength;

    // Greedy stack approach: build the maximal subsequence of length targetLength preserving order.
    // Use a heap-backed buffer to avoid returning a stack pointer.
    var buffer = new char[inputSpan.Length];
    int top = 0;

    for (int i = 0; i < inputSpan.Length; i++)
    {
        char c = inputSpan[i];
        if (!char.IsDigit(c))
            throw new FormatException($"Non-digit character encountered at position {i}: '{c}'.");

        while (top > 0 && toRemove > 0 && buffer[top - 1] < c)
        {
            top--;
            toRemove--;
        }

        buffer[top++] = c;
    }

    // If we still need to remove more (monotonic non-increasing), trim from the end
    top -= toRemove;
    if (top < targetLength) top = targetLength; // defensive; shouldn't normally happen

    // Take the first targetLength characters to form the maximal number
    var resultChars = new char[targetLength];
    Array.Copy(buffer, 0, resultChars, 0, targetLength);
    var resultString = new string(resultChars);

    if (!long.TryParse(resultString, out var parsed))
        throw new FormatException("Failed to parse selected digits into a number.");

    if (parsed > Int64.MaxValue)
        throw new OverflowException($"Result {parsed} exceeds {nameof(Int32)}. Change return type to long if needed.");

    return (Int64)parsed;
}

int GetLargestJoltage(ReadOnlySpan<char> inputSpan)
{
    // c - '0'
    var indexesOfLargest = FindIndexesOfLargest(inputSpan);

    // If there are multiple largest digits, return that digit twice as a two-digit number
    if (indexesOfLargest.Length > 1)
    {
        var value = inputSpan[indexesOfLargest[0]] - '0';
        return value * 10 + value;
    }

    var indexOfLargest = indexesOfLargest[0];

    // Otherwise, find the second largest digit
    // Remove the largest digit from consideration
    var clonedSpan = CloneWithZeros(inputSpan, indexesOfLargest);

    // Find the indexes of largest in the modified spam, effectively the second largest in the original
    var secondLargestIndexes = FindIndexesOfLargest(clonedSpan);
    if (secondLargestIndexes.Length > 0 && clonedSpan[secondLargestIndexes[0]] != '0')
    {
        // if largest is the last digit we return the largest value * 10 + second largest value
        if (indexOfLargest == inputSpan.Length - 1)
        {
            var largestValue = inputSpan[indexOfLargest] - '0';
            var secondLargestValue = inputSpan[secondLargestIndexes[0]] - '0';
            return secondLargestValue * 10 + largestValue;
        }

        // if any of the secondlargest indexes is higher than the index of the largest than we return a value largest value * 10 + second largest value
        var thereAreSecondLargestToTheRightOfTheLargest = secondLargestIndexes.Any(i => i > indexOfLargest);
        if (thereAreSecondLargestToTheRightOfTheLargest)
        {
            var largestValue = inputSpan[indexOfLargest] - '0';
            var secondLargestValue = inputSpan[secondLargestIndexes[0]] - '0';
            return largestValue * 10 + secondLargestValue;
        }

        // find the largest digit to the right of the largest digit in the original span
        // make all digits to the left of the largest digit and the largest digit itself zero
        var indexesToLeftIncludingLargest = Enumerable.Range(0, indexOfLargest + 1).ToArray();
        var clonedSpan2 = CloneWithZeros(inputSpan, indexesToLeftIncludingLargest);
        var rightSideIndexes = FindIndexesOfLargest(clonedSpan2);
        if (rightSideIndexes.Length > 0 && clonedSpan2[rightSideIndexes[0]] != '0')
        {
            var largestValue = inputSpan[indexOfLargest] - '0';
            var secondLargestValue = inputSpan[rightSideIndexes[0]] - '0';
            return largestValue * 10 + secondLargestValue;
        }
        return 0;
    }
    return 0;
}

int[] FindIndexesOfLargest(ReadOnlySpan<char> input)
{
    var indexOfLargest = 0;
    for (int i = 1; i < input.Length; i++)
    {
        if (input[i] > input[indexOfLargest]) indexOfLargest = i;
    }
    var allIndexesOfTheLargest = new List<int>();

    for (int i = 0; i < input.Length; i++)
    {
        if (input[i] == input[indexOfLargest]) allIndexesOfTheLargest.Add(i);
    }

    return allIndexesOfTheLargest.ToArray();
}

static ReadOnlySpan<char> CloneWithZeros(ReadOnlySpan<char> src, int[] zeroIndices)
{
    var buffer = src.ToArray(); // copies the chars into a heap array
    foreach (var idx in zeroIndices)
    {
        if ((uint)idx < (uint)buffer.Length) buffer[idx] = '0';
    }
    return new ReadOnlySpan<char>(buffer);
}


string RemoveKdigits(string num, int k)
{
    var originalK = k;
    var buffer = new char[num.Length];
    int top = 0;
    for (int i = 0; i < num.Length; i++)
    {
        char c = num[i];
        while (top > 0 && k > 0 && buffer[top - 1] > c)
        {
            top--;
            k--;
        }
        buffer[top++] = c;
    }
    var targetLength = num.Length - originalK;
    var resultChars = new char[targetLength];
    Array.Copy(buffer, 0, resultChars, 0, targetLength);
    var resultString = new string(resultChars);
    resultString = resultString.TrimStart('0');
    if (resultString == "") resultString = "0";
    return resultString;
}