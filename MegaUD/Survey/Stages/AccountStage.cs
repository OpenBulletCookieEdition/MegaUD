using System.Text.RegularExpressions;
using MegaUD.Misc;
using Spectre.Console;

namespace MegaUD.Survey.Stages
{
    public static class AccountStage
    {
        public static string GetAccountPath()
        {
            Print.Logo();
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Accounts path: ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Wrong Path[/]")
                    .Validate(path =>
                    {
                        return path switch
                        {
                            var p when new Regex(
                                        @".txt")
                                    .IsMatch(p) && File.Exists(p)
                                => ValidationResult.Success(),
                            _ => ValidationResult.Error("[red]Wrong Path[/]")
                        };
                    })
            );
        }
    }
}