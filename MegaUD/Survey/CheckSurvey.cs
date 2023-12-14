using MegaUD.Model;
using MegaUD.WorkStation;

namespace MegaUD.Survey;

public class CheckSurvey : BaseSurvey
{
    protected override WorkStationBase ConcreteSurvey(AccountPath accountsPath, int threads, ProxyPath? proxyPath = null)
    {
        return new WorkStationMegaCheck(accountsPath, threads, proxyPath);
    }
}