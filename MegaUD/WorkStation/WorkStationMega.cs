using MegaApiClientCore;
using MegaApiClientCore.Models;
using MegaUD.Model;
using MegaUD.Utils;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace MegaUD.WorkStation;

public abstract class WorkStationMega : WorkStationBase
{
    protected readonly ILogger<WorkStationMega> Logger;

    protected WorkStationMega(AccountPath accountPath, int thread, ProxyPath? proxyPath = null) : base(
        accountPath, thread, proxyPath)
    {
        Logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<WorkStationMega>();
    }

    #region Stats
    protected int GoodAccounts { get; private set; }
    protected int BadAccounts { get; private set; }
    protected int ErrorProxy { get; private set; }
    protected int TwoFa { get; private set; }
    protected int Progress { get; set; }

    #endregion

    #region Lockers
    private readonly object _lockerError = new();
    #endregion

    private async Task<MegaApiClient> Login(Account account, Proxy? proxy)
    {
        MegaApiClient megaApiClient = new MegaApiClient(5000, proxy);
        await megaApiClient.LoginAsync(account.Email, account.Password);
        GoodAccounts++;
        return megaApiClient;
    }

    protected void LogErrorThreadSafety(object lockObject, string exMessage, params object?[] args)
    {
        lock (lockObject)
        {
            Logger.LogError(exMessage, args);
        }
    }
    protected override async Task Process()
    {
        try
        {
            while (TotalAccounts > 0 && !Token.IsCancellationRequested)
            {
                if (!Accounts!.TryPop(out Account? account))
                {
                    continue;
                }

                try
                {
                    MegaApiClient megaApiClient = await Login(account, GREFC<Proxy>.GetRandomElement(Proxies));
                    await Step(megaApiClient);
                    Progress++;
                    await megaApiClient.LogoutAsync();
                }
                catch (ApiException ex)
                {
                    Progress++;

                    switch (ex.ApiResultCode)
                    {
                        case ApiResultCode.TwoFactorAuthenticationError:
                            TwoFa++;
                            continue;
                    }

                    BadAccounts++;

                    if (ex.ApiResultCode != ApiResultCode.ResourceNotExists &&
                        ex.ApiResultCode != ApiResultCode.TwoFactorAuthenticationError)
                    {
                        LogErrorThreadSafety(_lockerError,$"ApiException block{ex} {account.FullString}");
                    }
                }
                catch (InvalidCastException ex)
                {
                    Accounts?.Push(account);
                    
                    LogErrorThreadSafety(_lockerError, $"InvalidCastException block {ex.Message} {account.FullString}");
                }
                catch (AggregateException ex)
                {
                    ErrorProxy++;
                    Accounts?.Push(account);
                    
                    LogErrorThreadSafety(_lockerError, $"AggregateException block {ex.Message} {account.FullString}");
                }
                catch (ArgumentException ex)
                {
                    LogErrorThreadSafety(_lockerError, $"{ex.Message} {account.FullString}");
                }
                catch (NullReferenceException ex)
                {
                    LogErrorThreadSafety(_lockerError, $"{ex.Message} {account.FullString}");
                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    LogErrorThreadSafety(_lockerError, $"{ex.Message} {account.FullString}");
                }
            }
        }
        catch (Exception ex)
        {
            lock (_lockerError)
            {
                Logger.LogCritical(ex.ToString());
            }
        }
    }

    protected abstract Task<bool> Step(MegaApiClient megaApiClient);
}