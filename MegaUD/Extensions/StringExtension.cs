namespace MegaUD.Extensions;

public static class StringExtension
{
    public static bool EndsWith(this string source, IEnumerable<string> strings)
    {
        return strings.Any(source.EndsWith);
    }
}