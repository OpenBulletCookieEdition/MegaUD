using MegaUD;
using MegaUD.Misc;
using MegaUD.Survey.Stages;
using MegaUD.Survey.SurveySelection;
using MegaUD.WorkStation;

Console.Title = Info.LABEL;
Print.Logo();

ISurveySelection jobSelection = new SurveySelection();
using IWorkStation workStation = jobSelection.SelectJob(ModeStage.GetMode());
await workStation.StartAsync();

Console.ReadLine();
