using MegaUD.Model;
using MegaUD.Survey.Stages;
using MegaUD.WorkStation;

namespace MegaUD.Survey;

public class DownloadSurvey : BaseSurvey
{
    protected override WorkStationBase ConcreteSurvey(AccountPath accountsPath, int threads, ProxyPath? proxyPath = null)
    {
        var retries = RetriesStage.GetRetriesCount();
        var downloadFolderPath = DownloadStage.GetDownloadFolderPath();
        var fileTypes = DownloadStage.GetFileTypes().Split(';').ToList();
        var maxFileSize = DownloadStage.GetMaxFileSize();

        return new WorkStationMegaDownload(accountsPath, threads, retries, fileTypes, maxFileSize, downloadFolderPath,
            proxyPath);
    }
}