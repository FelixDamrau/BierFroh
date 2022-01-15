namespace BierFroh.Modules.Common;
public static class StringExtensions
{
    public static int NthIndexOf(this string? s, char value, int nthOccurrence)
    {
        if (nthOccurrence <= 0)
            throw new ArgumentException($"{nameof(nthOccurrence)} must be an positive integer!");
        if (s is null)
            return -1;

        var count = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == value)
            {
                count++;
                if (count == nthOccurrence)
                    return i;
            }
        }
        return -1;
    }
}
