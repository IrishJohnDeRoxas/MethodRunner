using IrishJohnDeRoxas.MethodRunner;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Type type = typeof(CommonInterviewQuestions);

BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;

MethodInfo[] methodInfo = type.GetMethods(flags);

var methodNames = methodInfo.Select(m => $"{m.Name}/{m.GetParameters().Length}").ToList();

var stopwatch = new Stopwatch();

while (true)
{
    var prompt = new SelectionPrompt<string>()
    .Title("Select a method")
    .AddChoices(methodNames)
    .AddChoices("[red]Exit[/]");

    var selectedMethod = AnsiConsole.Prompt(prompt);

    if (selectedMethod == "[red]Exit[/]")
    {
        await AnsiConsole.Status()
        .Spinner(Spinner.Known.Triangle)
        .StartAsync("Exiting...", async ctx =>
        {
            await Task.Delay(500);
        });

        break;
    }

    var selectedMethodName = Utils.CleanMethodName(selectedMethod);

    AnsiConsole.MarkupLine($"[green]{selectedMethod}[/]");

    MethodInfo method = type.GetMethod(selectedMethodName, flags) ?? throw new ArgumentNullException(nameof(method));

    object[] parameters = Utils.GetParameters(method);

    var result =
        await AnsiConsole.Status()
        .Spinner(Spinner.Known.Triangle)
        .SpinnerStyle(Style.Parse("yellow"))
        .StartAsync("Executing method...", async ctx =>
        {
            await Task.Delay(500);

            var task = Task.Run(() =>
            {
                stopwatch.Restart();

                return method.Invoke(null, parameters);
            });

            var output = await task;

            stopwatch.Stop();
            return output;
        });

    Utils.DisplayResult(result);
    AnsiConsole.MarkupLine($"[grey]Execution time: {stopwatch.Elapsed.TotalMilliseconds:N2} ms[/]\n");
}
