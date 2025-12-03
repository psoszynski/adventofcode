using System.Numerics;
using System.Text;
using Xunit.Abstractions;

namespace aoc2025_02_prep;

public class SolutuionTests
{
    readonly ITestOutputHelper _output;

    public SolutuionTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task AoC2025_02_1()
    {
        const string fileName = "input1.txt";
        string path = Path.Combine(AppContext.BaseDirectory, fileName);
        if (!File.Exists(path)) path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        if (!File.Exists(path)) throw new FileNotFoundException($"Could not find {fileName}");

        var input = await File.ReadAllTextAsync(path);
        var allLines = input.Split(',');

        BigInteger grandTotalInvalids = BigInteger.Zero;
        BigInteger grandTotalInvalidsSum = BigInteger.Zero;

        foreach (var line in allLines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split('-', 2);
            if (parts.Length != 2)
            {
                _output.WriteLine($"Skipping malformed range: '{line}'");
                continue;
            }

            var start = parts[0];
            var end = parts[1];

            bool inputsOk = ProductIDVerifier.TryValidateSequentialRange(start, end,
                out List<string> invalids, out long checkedCount, out string errorMessage);

            if (!inputsOk)
            {
                _output.WriteLine($"{start}-{end} InputsOk:False Error:{errorMessage}");
                continue;
            }

            string firstInvalid = invalids.Count > 0 ? invalids[0] : "(none)";
            _output.WriteLine($"{start}-{end} InputsOk:True FirstInvalid:{firstInvalid} InvalidCount:{invalids.Count}");
            grandTotalInvalids += invalids.Count;
            grandTotalInvalidsSum += invalids.Sum(i => Int64.Parse(i));
        }

        _output.WriteLine($"GrandTotalInvalids: {grandTotalInvalids}");
        _output.WriteLine($"GrandTotalInvalidsSum: {grandTotalInvalidsSum}");
        Assert.True(true);
    }

    [Fact]
    public async Task AoC2025_02_2()
    {
        const string fileName = "input1.txt";
        string path = Path.Combine(AppContext.BaseDirectory, fileName);
        if (!File.Exists(path)) path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        if (!File.Exists(path)) throw new FileNotFoundException($"Could not find {fileName}");

        var input = await File.ReadAllTextAsync(path);
        var allLines = input.Split(',');

        BigInteger grandTotalInvalids = BigInteger.Zero;
        BigInteger grandTotalInvalidsSum = BigInteger.Zero;

        foreach (var line in allLines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split('-', 2);
            if (parts.Length != 2)
            {
                _output.WriteLine($"Skipping malformed range: '{line}'");
                continue;
            }

            var start = parts[0];
            var end = parts[1];

            bool inputsOk = ProductIDVerifier.TryValidateSequentialRangePart2(start, end,
                out List<string> invalids, out long checkedCount, out string errorMessage);

            if (!inputsOk)
            {
                _output.WriteLine($"{start}-{end} InputsOk:False Error:{errorMessage}");
                continue;
            }

            string firstInvalid = invalids.Count > 0 ? invalids[0] : "(none)";
            _output.WriteLine($"{start}-{end} InputsOk:True FirstInvalid:{firstInvalid} InvalidCount:{invalids.Count}");
            grandTotalInvalids += invalids.Count;
            grandTotalInvalidsSum += invalids.Sum(i => Int64.Parse(i));
        }

        _output.WriteLine($"GrandTotalInvalids: {grandTotalInvalids}");
        _output.WriteLine($"GrandTotalInvalidsSum: {grandTotalInvalidsSum}");
        Assert.True(true);
    }

    [Theory]
    [InlineData("123", true)]
    [InlineData("122122", false)]
    [InlineData("12141214", false)]
    [InlineData("123233", true)]
    [InlineData("123456123456", false)]
    [InlineData("1188511885", false)]
    [InlineData("11885131885", true)]
    public void IsValidProductIDPart1(string productId, bool expectedIsValid)
    {
        Assert.Equal(expectedIsValid, ProductIDVerifier.IsValidProductID(productId));
    }


    [Theory]
    [InlineData("123123123", false)]
    [InlineData("111", false)]
    [InlineData("999", false)]
    [InlineData("1010", false)]
    [InlineData("1188511885", false)]
    [InlineData("222222", false)]
    [InlineData("1698522", true)]
    public void IsValidProductIDPart2(string productId, bool expectedIsValid)
    {
        Assert.Equal(expectedIsValid, ProductIDVerifier.IsValidProductIDPart2(productId));
    }

    [Theory]
    [InlineData(10000000, 12, 8)]
    public void GenerateNumericStringsReport(int count, int maxLength, int minLength)
    {
        const int absoluteMin = 2;
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
        if (minLength < absoluteMin) throw new ArgumentOutOfRangeException(nameof(minLength), $"minLength must be >= {absoluteMin}");
        if (maxLength < minLength) throw new ArgumentOutOfRangeException(nameof(maxLength), "maxLength must be >= minLength");

        var rnd = new Random(0); // deterministic seed for repeatable reports
        var sb = new StringBuilder();
        sb.AppendLine($"Generating {count} numeric strings (length {minLength}..{maxLength})");
        sb.AppendLine("Reporting only strings that failed validation (IsValid == false):");

        int invalidCount = 0;
        for (int i = 0; i < count; i++)
        {
            int len = rnd.Next(minLength, maxLength + 1);
            var chars = new char[len];
            for (int j = 0; j < len; j++)
            {
                chars[j] = (char)('0' + rnd.Next(10));
            }

            string s = new string(chars);
            bool isValid = ProductIDVerifier.IsValidProductID(s);
            if (!isValid)
            {
                invalidCount++;
                sb.AppendLine($"{i + 1:0000}: {s} => {isValid}");
            }
        }

        sb.AppendLine($"Total generated: {count}");
        sb.AppendLine($"Total invalid (reported): {invalidCount}");

        _output.WriteLine(sb.ToString());

        Assert.True(true);
    }

    [Theory]
    [InlineData("11", "22", false)]
    [InlineData("95", "115", false)]
    [InlineData("1698522", "1698528", true)]
    [InlineData("016985220", "016985280", false)]
    public void ValidateSequentialRange(string start, string end, bool expectedAllValid)
    {
        bool inputsOk = ProductIDVerifier.TryValidateSequentialRange(start, end,
            out List<string> invalids, out long checkedCount, out string errorMessage);

        if (!inputsOk)
        {
            // input validation failed (non-digit or leading-zero start/end) - consider that an invalid found
            _output.WriteLine($"Input validation failed: {errorMessage}");
            bool foundInvalid = true;
            _output.WriteLine($"Checked 0 sequential numeric strings from '{start}' to '{end}'. FoundInvalid={foundInvalid}");
            Assert.Equal(expectedAllValid, !foundInvalid);
            return;
        }

        bool foundInvalidInRange = invalids.Count > 0;
        string firstInvalid = foundInvalidInRange ? invalids[0] : null;

        if (foundInvalidInRange)
        {
            _output.WriteLine($"Validation found invalids; first: '{firstInvalid}' total:{invalids.Count}");
        }
        else
        {
            _output.WriteLine($"Successfully validated {checkedCount} sequential numeric strings from '{start}' to '{end}'.");
        }

        Assert.Equal(expectedAllValid, !foundInvalidInRange);
    }
}


static class ProductIDVerifier
{
    internal static bool IsValidProductID(string productID)
    {
        if (productID == null) return false;
        return IsValidProductID(productID.AsSpan());
    }

    // - empty -> false
    // - leading zero (multi-digit) -> false
    // - odd length -> true (per requirement)
    // - even length -> must be digits and halves compared (invalid when halves equal)
    internal static bool IsValidProductID(ReadOnlySpan<char> productSpan)
    {
        if (productSpan.IsEmpty) return false;

        int len = productSpan.Length;

        if (len > 1 && productSpan[0] == '0') return false;

        if ((len & 1) != 0) return true;

        for (int i = 0; i < len; i++)
        {
            if ((uint)(productSpan[i] - '0') > 9) return false;
        }

        int half = len >> 1;

        var left = productSpan.Slice(0, half);
        var right = productSpan.Slice(half, half);

        return !left.SequenceEqual(right);
    }

    internal static bool IsValidProductIDPart2(ReadOnlySpan<char> productSpan)
    {
        var lengthOfFragment = 1;
        while (true)
        {
            var hasOnlyRepetitions = HasProductIDOnlyRepetitionsPerFragmentLength(productSpan, lengthOfFragment);
            if(hasOnlyRepetitions)
            {
                return false;
            }
            lengthOfFragment++;
            if(lengthOfFragment > productSpan.Length / 2)
            {
                break;
            }
        }
        return true;
    }
    


    internal static bool HasProductIDOnlyRepetitionsPerFragmentLength(ReadOnlySpan<char> productSpan, int lengthOfFragment)
    {
        if (productSpan.IsEmpty) return false;

        int totalLength = productSpan.Length;

        if (totalLength > 1 && productSpan[0] == '0') return false;

        if (totalLength <= lengthOfFragment) return false;

        if (totalLength % lengthOfFragment != 0) return false;

        for (int i = 0; i < totalLength; i++)
        {
            if ((uint)(productSpan[i] - '0') > 9) return false;
        }

        var i2 = 1;
        var previousSlice = productSpan.Slice(0, lengthOfFragment);
        var hasOnlyRepetitions = true;
        while(lengthOfFragment * i2 < totalLength)
        {
            var currentSlice = productSpan.Slice(lengthOfFragment * i2, lengthOfFragment);
            if (!currentSlice.SequenceEqual(previousSlice))
            {
                hasOnlyRepetitions = false;
                break;
            }
            currentSlice = previousSlice;
            i2++;
        }

        return hasOnlyRepetitions;
    }

    internal static bool TryValidateSequentialRange(string start, string end,
        out List<string> invalidProductIds, out long checkedCount, out string errorMessage)
    {
        invalidProductIds = new List<string>();
        checkedCount = 0;
        errorMessage = null;

        if (start is null)
        {
            errorMessage = "start is null";
            return false;
        }
        if (end is null)
        {
            errorMessage = "end is null";
            return false;
        }

        if (!IsAllDigitsStrict(start))
        {
            errorMessage = $"Start value contains non-digit characters or leading zeros: '{start}'";
            return false;
        }
        if (!IsAllDigitsStrict(end))
        {
            errorMessage = $"End value contains non-digit characters or leading zeros: '{end}'";
            return false;
        }

        int cmp = CompareNumericStrings(start, end);
        if (cmp >= 0)
        {
            errorMessage = $"End value must be strictly greater than start. start='{start}' end='{end}'";
            return false;
        }

        string current = start;
        while (true)
        {
            checkedCount++;
            bool isValid = IsValidProductID(current);
            if (!isValid)
            {
                invalidProductIds.Add(current);
            }

            if (CompareNumericStrings(current, end) == 0)
            {
                break;
            }

            current = IncrementNumericString(current);
        }

        return true;
    }

    internal static bool TryValidateSequentialRangePart2(string start, string end,
    out List<string> invalidProductIds, out long checkedCount, out string errorMessage)
    {
        invalidProductIds = new List<string>();
        checkedCount = 0;
        errorMessage = null;

        if (start is null)
        {
            errorMessage = "start is null";
            return false;
        }
        if (end is null)
        {
            errorMessage = "end is null";
            return false;
        }

        if (!IsAllDigitsStrict(start))
        {
            errorMessage = $"Start value contains non-digit characters or leading zeros: '{start}'";
            return false;
        }
        if (!IsAllDigitsStrict(end))
        {
            errorMessage = $"End value contains non-digit characters or leading zeros: '{end}'";
            return false;
        }

        int cmp = CompareNumericStrings(start, end);
        if (cmp >= 0)
        {
            errorMessage = $"End value must be strictly greater than start. start='{start}' end='{end}'";
            return false;
        }

        string current = start;
        while (true)
        {
            checkedCount++;
            bool isValid = IsValidProductIDPart2(current);
            if (!isValid)
            {
                invalidProductIds.Add(current);
            }

            if (CompareNumericStrings(current, end) == 0)
            {
                break;
            }

            current = IncrementNumericString(current);
        }

        return true;
    }

    internal static bool IsAllDigitsStrict(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        if (s.Length > 1 && s[0] == '0') return false;
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if ((uint)(c - '0') > 9) return false;
        }
        return true;
    }

    internal static int CompareNumericStrings(string a, string b)
    {
        int ia = 0;
        while (ia < a.Length && a[ia] == '0') ia++;
        int ib = 0;
        while (ib < b.Length && b[ib] == '0') ib++;

        int lena = a.Length - ia;
        int lenb = b.Length - ib;

        if (lena == 0 && lenb == 0) return 0;
        if (lena != lenb) return lena < lenb ? -1 : 1;

        for (int i = 0; i < lena; i++)
        {
            char ca = a[ia + i];
            char cb = b[ib + i];
            if (ca != cb) return ca < cb ? -1 : 1;
        }

        return 0;
    }

    internal static bool TryIncrementNumber(Span<char> digits)
    {
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            char c = digits[i];
            int d = c - '0';
            if ((uint)d > 9) return false;

            d++;
            if (d < 10)
            {
                digits[i] = (char)('0' + d);
                return true;
            }

            // carry
            digits[i] = '0';
        }

        return false;
    }

    internal static string IncrementNumericString(string numeric)
    {
        if (numeric == null) throw new ArgumentNullException(nameof(numeric));
        var chars = numeric.ToCharArray();
        if (TryIncrementNumber(chars))
        {
            return new string(chars);
        }

        return '1' + new string('0', numeric.Length);
    }

    internal static bool IsValidProductID(char[] productChars)
    {
        if (productChars == null) return false;
        return IsValidProductID(new ReadOnlySpan<char>(productChars));
    }
}