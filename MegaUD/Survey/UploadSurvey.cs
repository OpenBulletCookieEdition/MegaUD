using MegaUD.Model;
using MegaUD.Survey.Stages;
using MegaUD.WorkStation;

namespace MegaUD.Survey;

public class UploadSurvey : BaseSurvey
{
    protected override WorkStationBase ConcreteSurvey(AccountPath accountsPath, int threads, ProxyPath? proxyPath = null)
    {
        var retries = RetriesStage.GetRetriesCount();
        string uploadFolderPath = UploadStage.GetUploadFolderPath();
        WorkStationMegaUpload workStationMegaUpload =
            new WorkStationMegaUpload(accountsPath, uploadFolderPath, threads, retries, proxyPath);
        return workStationMegaUpload;
    }
}