using System.Drawing;
using Spectre.Console;

namespace MegaUD.Misc
{
    public static class Print
    {
        public static void Logo()
        {
            AnsiConsole.Clear();
            Console.WriteLine();
            Console.WriteLine();
            AnsiConsole.MarkupLine("[bold yellow]                   ███╗░░░███╗███████╗░██████╗░░█████╗░  ██╗░░░██╗██████╗░[/]");
            AnsiConsole.MarkupLine("[bold yellow]                   ████╗░████║██╔════╝██╔════╝░██╔══██╗  ██║░░░██║██╔══██╗[/]");
            AnsiConsole.MarkupLine("[bold yellow]                   ██╔████╔██║█████╗░░██║░░██╗░███████║  ██║░░░██║██║░░██║[/]");
            AnsiConsole.MarkupLine("[bold yellow]                   ██║╚██╔╝██║██╔══╝░░██║░░╚██╗██╔══██║  ██║░░░██║██║░░██║[/]");
            AnsiConsole.MarkupLine("[bold yellow]                   ██║░╚═╝░██║███████╗╚██████╔╝██║░░██║  ╚██████╔╝██████╔╝[/]");
            AnsiConsole.MarkupLine("[bold yellow]                   ╚═╝░░░░░╚═╝╚══════╝░╚═════╝░╚═╝░░╚═╝  ░╚═════╝░╚═════╝░[/]");
            AnsiConsole.MarkupLine("---------------------------------------------------------------------------------------------------------");
            AnsiConsole.MarkupLine("Donate: [bold yellow]BTC - bc1qxrl29sy4hqssqer9q5g75w56zxldugh947gkv9[/] [blue]ETH - 0x44A2aF56c0F83E50AF88c9259BD14D5302ddA0Ce[/]");
            AnsiConsole.MarkupLine("        [silver]LTC - LKGHqwY5LGoWGKB8zcy2GLDuntppBLETGG[/]         [green]TRC20 - TNuG8jRwDr5LVkQww4LJoBzwDTNKMeJdxf[/]");
            AnsiConsole.MarkupLine("---------------------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }
    }
}
