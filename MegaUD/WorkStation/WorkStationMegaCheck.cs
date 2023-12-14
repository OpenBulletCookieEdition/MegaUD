using MegaApiClientCore;
using MegaUD.Model;
using Spectre.Console;

namespace MegaUD.WorkStation;

public class WorkStationMegaCheck : WorkStationMega
{
    public WorkStationMegaCheck(AccountPath accountPath, int thread, ProxyPath? proxyPath = null) : base(accountPath,
        thread, proxyPath)
    {
    }

    private object LockerGood { get; } = new();

    protected override async Task TimerTik()
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                // Define tasks
                var task1 = ctx.AddTask("[green]Progress: [/]");
                task1.MaxValue = TotalAccounts;

                while (!ctx.IsFinished)
                {
                    // Simulate some work
                    await Task.Delay(250);
                    Console.Title =
                        $"Threads: {Threads} | Accounts: {TotalAccounts} | Good: {GoodAccounts} | Bad: {BadAccounts} | 2FA: {TwoFa} | Proxy: {TotalProxies} ErrorProxy: {ErrorProxy}";
                    task1.Value = Progress;
                }
            });
    }

    protected override async Task<bool> Step(MegaApiClient megaApiClient)
    {
        IAccountInformation accountInformation = await megaApiClient.GetAccountInformationAsync();
        long quotaGb = accountInformation.TotalQuota / 1000000000;

        lock (LockerGood)
        {
            SaveBySize(quotaGb, $"{megaApiClient.Email}:{megaApiClient.Password}");
        }

        return true;
    }

    private void SaveBySize(long quotaGb, string credentials)
    {
        switch (quotaGb)
        {
            case < 30:
                File.AppendAllText(LocalDirectory + "\\[20-30 GB].txt",
                    $"[{quotaGb}GB] {credentials}\n");
                return;
            case > 30 and < 60:
                File.AppendAllText(LocalDirectory + "\\[30-60 GB].txt",
                    $"[{quotaGb}GB] {credentials}\n");
                return;
            case > 60 and < 500:
                File.AppendAllText(LocalDirectory + "\\[60-500 GB].txt",
                    $"[{quotaGb}GB] {credentials}\n");
                return;
            case > 500 and < 3000:
                File.AppendAllText(LocalDirectory + "\\[500-3000 GB].txt",
                    $"[{quotaGb}GB] {credentials}\n");
                return;
            case > 3000 and < 10000:
                File.AppendAllText(LocalDirectory + "\\[3-10 TB].txt",
                    $"[{quotaGb / 1000}TB] {credentials}\n");
                return;
            case > 10000:
                File.AppendAllText(LocalDirectory + "\\[10+ TB].txt",
                    $"[{quotaGb / 1000}TB] {credentials}\n");
                return;
        }
    }
}