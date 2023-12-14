using MegaUD.Enums;
using MegaUD.WorkStation;

namespace MegaUD.Survey.SurveySelection;

public interface ISurveySelection
{
    public WorkStationBase SelectJob(ModeType type);
}