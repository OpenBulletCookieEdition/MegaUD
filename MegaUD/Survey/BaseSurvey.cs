using MegaUD.Model;
using MegaUD.Survey.Stages;
using MegaUD.WorkStation;

namespace MegaUD.Survey;

public abstract class BaseSurvey : ISurvey
{
    protected abstract WorkStationBase ConcreteSurvey(AccountPath accountsPath, int threads, ProxyPath? proxyPath = null);

    public WorkStationBase Survey()
    {
        ProxyPath? proxyPath = null;
        int threads = 0;  
        if (ProxyStage.UseProxy())
        {
            var proxyType = ProxyStage.GetProxyType();
            var path = ProxyStage.GetProxyPath();
            proxyPath = new ProxyPath(path, proxyType);
        }   
        
        var accountsPath =  new AccountPath(AccountStage.GetAccountPath());
        threads = ThreadStage.GetThreads();     

        return ConcreteSurvey(accountsPath, threads, proxyPath);
    }        
    
}