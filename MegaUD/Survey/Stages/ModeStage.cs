using MegaUD.Enums;
using Spectre.Console;

namespace MegaUD.Survey.Stages
{
    

    public static class ModeStage
    {
        public static ModeType GetMode()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<ModeType>()
                    .Title("Select [red]mode[/]:")
                    .PageSize(10)
                    .AddChoices(new[] {
                        ModeType.UPLOAD, ModeType.DOWNLOAD,ModeType.CHECK
                    }));
        }
    }
}
