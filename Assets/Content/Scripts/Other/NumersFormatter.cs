
public class NumersFormatter
{
    private static readonly string[] suffixes = { "", "K", "M", "B", "T" };

    public static string FormatNumber(float number)
    {
        if (number < 1000) return number.ToString("0");

        int suffixIndex = 0;
        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000f;
            suffixIndex++;
        }

        return number.ToString("0.#") + suffixes[suffixIndex];
    }
}
