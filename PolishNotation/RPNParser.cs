using System.Text;
using System.Text.RegularExpressions;

namespace PolishNotation;

public class RpnParser
{
    private readonly Dictionary<char, int> _operationPriority = new()
    {
        { '(', 0 },
        { '+', 1 },
        { '-', 1 },
        { '*', 2 },
        { '/', 2 },
        { '^', 3 },
        { '~', 4 }
    };

    public string Parse(string expression)
    {
        CheckExpression(expression);
        var infixExpression = GetInfixExpression(expression);
        var postfixExpression = "";
        var stack = new Stack<char>();

        for (var i = 0; i < infixExpression.Length; i++)
        {
            var c = infixExpression[i];

            if (char.IsDigit(c))
            {
                postfixExpression += GetNumberString(infixExpression, ref i) + " ";
            }
            else
                switch (c)
                {
                    case '(':
                        stack.Push(c);
                        break;
                    case ')':
                    {
                        while (stack.Count > 0 && stack.Peek() != '(')
                            postfixExpression += stack.Pop();
                        stack.Pop();
                        break;
                    }
                    default:
                    {
                        if (_operationPriority.ContainsKey(c))
                        {
                            var op = c;

                            if (op == '-' &&
                                (i == 0 || (i > 1 && _operationPriority.ContainsKey(infixExpression[i - 1]))))
                                op = '~';

                            while (stack.Count > 0 && _operationPriority[stack.Peek()] >= _operationPriority[op])
                                postfixExpression += stack.Pop() + " ";

                            stack.Push(op);
                        }

                        break;
                    }
                }
        }

        return stack.Aggregate(postfixExpression, (current, op) => current + op);
    }

    private static string GetInfixExpression(string expression)
    {
        return expression.Replace(" ", "").Replace(".", ",");
    }

    private static void CheckExpression(string expression)
    {
        if (!Regex.IsMatch(expression, @"^[\d\-+\*\/\s\(\)\^\.\,]+$"))
            throw new ArgumentException("Input string contains invalid characters");
        if (!CheckParenthesesBalance(expression))
            throw new ArgumentException("Input string has unpaired brackets");
    }

    private static string GetNumberString(string expression, ref int position)
    {
        var strNumber = new StringBuilder();

        for (; position < expression.Length; position++)
        {
            var number = expression[position];
            if (char.IsDigit(number))
            {
                strNumber.Append(number);
            }
            else if (number == ',')
            {
                strNumber.Append(number);
            }
            else
            {
                position--;
                break;
            }
        }

        return strNumber.ToString();
    }

    private static bool CheckParenthesesBalance(string expression)
    {
        var balance = 0;
        foreach (var t in expression)
        {
            switch (t) {
                case '(':
                    balance++;
                    break;
                case ')':
                {
                    balance--;
                    if (balance < 0)
                        return false;
                    break;
                }
            }
        }

        return balance == 0;
    }
}