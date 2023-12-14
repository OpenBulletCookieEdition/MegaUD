using MegaUD.Misc;
using Spectre.Console;

namespace MegaUD.Survey.Stages
{
    public static class UploadStage
    {
        public static string GetUploadFolderPath()
        {
            Print.Logo();
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Upload folder path (example C:\\folder): ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Wrong Directory[/]")
                    .Validate(path =>
                    {
                        return path switch
                        {
                            var p when Directory.Exists(p) => ValidationResult.Success(),
                            _ => ValidationResult.Error("[red]Wrong Path[/]")
                        };
                    }));
        }
    }
}
