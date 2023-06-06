namespace PolishNotation;

public static class Program
{
    public static void Main()
    {
        const string input = "(5+8-4*2))";
        var rpnParser = new RpnParser();
        rpnParser.Parse(input);
    }
}