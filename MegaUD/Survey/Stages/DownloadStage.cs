using MegaUD.Misc;
using Spectre.Console;

namespace MegaUD.Survey.Stages;

public static class DownloadStage
{
    public static string GetDownloadFolderPath()
    {
        Print.Logo();
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Download folder path (example C:\\folder): ")
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
    
    public static string GetFileTypes()
    {
        Print.Logo();
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Desired file types for downloading (example .txt;.doc;.js): ")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]Wrong file types![/]")
                .Validate(fileTypes =>
                {
                    return fileTypes switch
                    {
                        var p when p.Contains(';')&& p.Contains('.') => ValidationResult.Success(),
                        _ => ValidationResult.Error("[red]Wrong Path[/]")
                    };
                }));
    }

    
    public static int GetMaxFileSize()
    {
        Print.Logo();
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Max file size (KB): ")
                .PromptStyle("green")
                .DefaultValue(500)
                .ValidationErrorMessage("[red]Enter a number from 0 to 5000[/]")
                .Validate(threads =>
                {
                    return threads switch
                    {
                        < 0 => ValidationResult.Error("[red]Max file size cannot be less than 0[/]"),
                        > 5000 => ValidationResult.Error("[red]Max file size cannot be more than 5000[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));
    }
    


}