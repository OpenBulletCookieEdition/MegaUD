using MegaUD.Misc;
using Spectre.Console;

namespace MegaUD.Survey.Stages
{
    public static class ThreadStage
    {

        public static int GetThreads()
        {
            Print.Logo();
            return AnsiConsole.Prompt(
                new TextPrompt<int>("Threads: ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Enter a number from 1 to 500[/]")
                    .Validate(threads =>
                    {
                        return threads switch
                        {
                            < 1 => ValidationResult.Error("[red]Threads cannot be less than 1[/]"),
                            > 500 => ValidationResult.Error("[red]Threads cannot be more than 500[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));
        }
    }
}
