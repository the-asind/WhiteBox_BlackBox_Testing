using System.Text;

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
        ValidateExpression(expression);
        var infixExpression = GetInfixExpression(expression);
        var postfixExpressionBuilder = new StringBuilder();
        var stack = new Stack<char>();

        for (var i = 0; i < infixExpression.Length; i++)
        {
            var character = infixExpression[i];

            if (char.IsDigit(character))
            {
                postfixExpressionBuilder.Append(GetNumberString(infixExpression, ref i)).Append(' ');
            }
            else
            {
                if (character == '(')
                {
                    stack.Push(character);
                }
                else if (character == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(') postfixExpressionBuilder.Append(stack.Pop());
                    stack.Pop();
                }
                else
                {
                    if (IsOperator(character))
                    {
                        var op = character;

                        if (op == '-' && (i == 0 || (i > 1 && IsOperator(infixExpression[i - 1])))) op = '~';

                        while (stack.Count > 0 && GetOperatorPriority(stack.Peek()) >= GetOperatorPriority(op))
                            postfixExpressionBuilder.Append(stack.Pop()).Append(" ");

                        stack.Push(op);
                    }
                }
            }
        }

        while (stack.Count > 0) postfixExpressionBuilder.Append(stack.Pop());

        return postfixExpressionBuilder.ToString();
    }

    private bool IsOperator(char c)
    {
        return _operationPriority.ContainsKey(c);
    }

    private int GetOperatorPriority(char c)
    {
        return _operationPriority[c];
    }


    private static string GetInfixExpression(string expression)
    {
        return expression.Replace(" ", "").Replace(".", ",");
    }

    private static void ValidateExpression(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            throw new ArgumentException("Input string is empty");
        if (!IsExpressionValid(expression))
            throw new ArgumentException("Input string contains invalid characters");
        if (!IsParenthesesBalanced(expression))
            throw new ArgumentException("Input string has unpaired brackets");
    }

    private static bool IsExpressionValid(string expression)
    {
        var allowedChars = "0123456789-+*/()^.,";

        foreach (var c in expression)
            if (!allowedChars.Contains(c))
                return false;

        return true;
    }

    private static bool IsParenthesesBalanced(string expression)
    {
        var balance = 0;

        foreach (var t in expression)
            switch (t)
            {
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

        return balance == 0;
    }

    private static string GetNumberString(string expression, ref int position)
    {
        var strNumber = new StringBuilder();
        for (; position < expression.Length; position++)
        {
            var character = expression[position];

            if (char.IsDigit(character) || character == ',')
            {
                strNumber.Append(character);
            }
            else
            {
                position--;
                break;
            }
        }

        return strNumber.ToString();
    }
}