using Spectre.Console;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IrishJohnDeRoxas.MethodRunner;

public static partial class Utils
{
    [GeneratedRegex(@"[\d/]")]
    private static partial Regex MethodNameCleanerRegex();

    [GeneratedRegex(@"[\W_]")]
    private static partial Regex PalindromeStrCleanerRegex();

    //[GeneratedRegex(@"^ *(\d+ *)+$")
    [GeneratedRegex(@"^[\d ]+$")]
    private static partial Regex NumbersAndSpacesOnlyRegex();

    public static string CleanMethodName(string input)
    {
        return MethodNameCleanerRegex().Replace(input, "");
    }

    public static string CleanPalindromeStr(string input)
    {
        return PalindromeStrCleanerRegex().Replace(input, "").ToLower();
    }

    public static bool IsValidNumericInput(string input)
    {
        return NumbersAndSpacesOnlyRegex().IsMatch(input);
    }

    public static void DisplayResult(object? result)
    {
        string message = result switch
        {
            IEnumerable<int> i => $"Returned value: [green]{{{string.Join(", ", i)}}}[/]",
            long i => $"Returned value: [green]{i:N0}[/]",
            null => "[red]Null value returned[/]",
            _ => $"Returned value: [green]{result}[/]"
        };

        AnsiConsole.MarkupLine($"{message} \n");
    }

    private static object AskParameters(ParameterInfo parameter)
    {
        string typeName = Markup.Escape(parameter.ParameterType.Name);
        string paramName = Markup.Escape(parameter.Name ?? "parameter");
        string prompt = $"Set value for [green]{paramName}[/] ([red]{typeName}[/]):";

        if (parameter.ParameterType == typeof(IList<int>))
        {
            string example = "[grey]space separated numbers (e.g., 0 1 2 3)[/]";

            var listOfNumbers = new TextPrompt<string>($"{prompt} {example}")
                .Validate(str =>
                {
                    return IsValidNumericInput(str)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Invalid format![/]");
                });

            var input = AnsiConsole.Prompt(listOfNumbers);

            return input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();
        }

        var result = AnsiConsole.Ask<string>(prompt);
        return Convert.ChangeType(result, parameter.ParameterType);
    }

    public static object[] GetParameters(MethodInfo method)
    {
        object[] parameters =
            method
            .GetParameters()
            .Select(AskParameters)
            .ToArray();

        return parameters;
    }

}
