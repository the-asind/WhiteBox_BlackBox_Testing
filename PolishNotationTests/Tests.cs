using PolishNotation;

namespace PolishNotationTests;

[TestClass]
public class Tests
{
    [TestMethod]
    public void BinaryOperatorTest()
    {
        const string input = "1+2";
        const string output = "1 2 +";
        var rpnParser = new RpnParser();

        Assert.AreEqual(output, rpnParser.Parse(input));
    }

    [TestMethod]
    public void UnaryMinusOperatorTest()
    {
        const string input = "-1+2";
        const string output = "1 ~ 2 +";
        var rpnParser = new RpnParser();

        Assert.AreEqual(output, rpnParser.Parse(input));
    }

    [TestMethod]
    public void BracketsTest()
    {
        const string input = "(1+2)/(-3*4)";
        const string output = "1 2 +3 ~ 4 */";
        var rpnParser = new RpnParser();

        Assert.AreEqual(output, rpnParser.Parse(input));
    }

    [TestMethod]
    public void DuplicatePlusTest()
    {
        const string input = "1++2";
        var rpnParser = new RpnParser();

        Assert.AreEqual(rpnParser.Parse(input), "1 + 2 +");
    }

    [TestMethod]
    public void NestedBracketsTest()
    {
        const string input = "((1+2)-3)^4/5";
        const string output = "1 2 +3 -4 ^ 5 /";
        var rpnParser = new RpnParser();

        Assert.AreEqual(output, rpnParser.Parse(input));
    }

    [TestMethod]
    public void UnaryPlusTest()
    {
        const string input = "+1+2";
        const string output = "1 + 2 +";
        var rpnParser = new RpnParser();

        Assert.AreEqual(output, rpnParser.Parse(input));
    }

    [TestMethod]
    public void EmptyExpressionTest()
    {
        const string input = "";
        var rpnParser = new RpnParser();

        Assert.ThrowsException<ArgumentException>(() => rpnParser.Parse(input));
    }

    [TestMethod]
    public void UnpairedBracketsTest()
    {
        const string input = "(1+2-4*3))";
        var rpnParser = new RpnParser();

        Assert.ThrowsException<ArgumentException>(() => rpnParser.Parse(input));
    }
}