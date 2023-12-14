using MegaApiClientCore;
using MegaUD.Model;
using Spectre.Console;


namespace MegaUD.WorkStation
{
    public class WorkStationMegaUpload : WorkStationMega
    {
        private string UploadFolderPath { get; }
        private int Retries { get; }

        public WorkStationMegaUpload(AccountPath accountPath, string uploadFolderPath, int thread, int retries,
            ProxyPath? proxyPath = null) : base(accountPath, thread, proxyPath)
        {
            UploadFolderPath = uploadFolderPath;
            Retries = retries;
        }

        #region Stats

        private int Uploaded { get; set; }
        private int QuotaExited { get; set; }
        private int ErrorUpload { get; set; }
        private int BlockedAccount { get; set; }

        #endregion

        #region Lockers

        private readonly object _lockerErrorUploadFiles = new();

        #endregion


        protected override async Task<bool> Step(MegaApiClient megaApiClient)
        {
            int cycleUpload = 0;
            ReUpload:
            try
            {
                await Upload(megaApiClient);
                return true;
            }
            catch (Exception ex)
            {
                if (ex is ApiException exApi)
                {
                    switch (exApi.ApiResultCode)
                    {
                        case ApiResultCode.QuotaExceeded:
                            QuotaExited++;
                            break;
                        case ApiResultCode.ResourceAdministrativelyBlocked:
                            BlockedAccount++;
                            break;
                    }
                }
                else
                {
                    if (Retries >= cycleUpload)
                    {
                        cycleUpload++;
                        goto ReUpload;
                    }

                    lock (_lockerErrorUploadFiles)
                    {
                        File.AppendAllText(LocalDirectory + "\\[ErrorUpload] " + DateForSave + ".txt",
                            $"{megaApiClient.Email}:{megaApiClient.Password}" + Environment.NewLine + ex.Message +
                            Environment.NewLine);
                        File.AppendAllText(LocalDirectory + "\\[ErrorUploadAccounts] " + DateForSave + ".txt",
                            $"{megaApiClient.Email}:{megaApiClient.Password}" + Environment.NewLine);
                    }

                    ErrorUpload++;
                }
                return false;
            }
        }

        private async Task<bool> Upload(IMegaApiClient megaApiClient)
        {
            IEnumerable<INode> nodes = await megaApiClient.GetNodesAsync();

            foreach (string file in Directory.GetFiles(UploadFolderPath))
            {
                INode root = nodes.Single(x => x.Type == NodeType.Root);
                megaApiClient.UploadFile(file, root, Token);
                Uploaded++;
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
                            $"Threads: {Threads} | Accounts: {TotalAccounts} | Good: {GoodAccounts} | Bad: {BadAccounts} | 2FA: {TwoFa} | Blocked: {BlockedAccount} | Uploaded: {Uploaded} | ErrorUploaded: {ErrorUpload} | Proxy: {TotalProxies} ErrorProxy: {ErrorProxy}";
                        task1.Value = Progress;
                    }
                });
        }
    }
}