using MegaUD.Misc;
using Spectre.Console;

namespace MegaUD.Survey.Stages
{
    public static class RetriesStage
    {
        public static int GetRetriesCount()
        {
            Print.Logo();
            return AnsiConsole.Prompt(
                new TextPrompt<int>("Number of retries when loading fails (default 0): ")
                    .PromptStyle("green")
                    .DefaultValue(0)
                    .ValidationErrorMessage("[red]Enter a number from 0 to 5[/]")
                    .Validate(retries =>
                    {
                        return retries switch
                        {
                            < 0 => ValidationResult.Error("[red]Retries cannot be less than 0[/]"),
                            > 5 => ValidationResult.Error("[red]Retries cannot be more than 5[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));
        }
    }
}
