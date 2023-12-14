using MegaUD.Enums;
using MegaUD.WorkStation;

namespace MegaUD.Survey.SurveySelection;

public class SurveySelection : ISurveySelection
{
    public WorkStationBase SelectJob(ModeType type)
    {

        switch (type)
        {
            case ModeType.UPLOAD:
                return new UploadSurvey().Survey();
            case ModeType.DOWNLOAD:
                return new DownloadSurvey().Survey();
            case ModeType.CHECK:
                return new CheckSurvey().Survey();
            default: throw new ArgumentException(nameof(type));
        }
    }
}