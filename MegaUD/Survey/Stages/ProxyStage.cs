using System.Text.RegularExpressions;
using MegaApiClientCore.Enums;
using MegaUD.Misc;
using Spectre.Console;

namespace MegaUD.Survey.Stages
{
    public static class ProxyStage
    {
        public static bool UseProxy()
        {
            string result = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Use proxy?:")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Yes","No" }));
            if (result == "Yes") return true;
            else return false;

        }

        public static ProxyType GetProxyType()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<ProxyType>()
                    .Title("Select [red]Proxy Type[/]:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(new[] {
                        ProxyType.HTTPS, ProxyType.SOCKS4, ProxyType.SOCKS5 }));
        }

        public static string GetProxyPath()
        {
            Print.Logo();
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Proxy Path: ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Wrong Path[/]")
                    .Validate(path =>
                    {
                        return path switch
                        {
                            var p when new Regex(
                                        @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                                    .IsMatch(p)
                                => ValidationResult.Success(),
                            var p when new Regex(
                                        @".txt")
                                    .IsMatch(p) && File.Exists(p)
                                => ValidationResult.Success(),
                            _ => ValidationResult.Error("[red]Wrong Path[/]")
                        };
                    }));
        }
    }
}
