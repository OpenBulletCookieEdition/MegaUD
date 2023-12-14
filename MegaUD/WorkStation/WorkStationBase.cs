using System.Collections.Concurrent;
using MegaApiClientCore.Models;
using MegaUD.Model;

namespace MegaUD.WorkStation
{
    public abstract class WorkStationBase: IWorkStation
    {
        private ProxyPath? ProxyPath { get; }
        private AccountPath AccountsPath { get;  }
        private IList<Task> TasksList { get; }
        protected int Threads { get; private set; }

        protected WorkStationBase(AccountPath accountPath,  int thread, ProxyPath? proxyPath = null)
        {
            ProxyPath = proxyPath;
            AccountsPath = accountPath;
            Threads = thread;
            TasksList = new List<Task>();
        }

        protected ConcurrentStack<Account>? Accounts { get; private set; }
        protected IList<Proxy>? Proxies { get; private set; }

        protected int TotalAccounts=> Accounts?.Count ?? 0;
        protected int TotalProxies=> Proxies?.Count ?? 0;
        
        protected string? LocalDirectory { get; private set; }
        protected string? DateForSave { get; private set; }
        
        
        private CancellationTokenSource Source { get; set; }
        protected CancellationToken Token { get; private set; }
        
        public async Task StartAsync()
        {
            Accounts = await AccountsPath.GetAccountsAsync();
            Proxies = ProxyPath is not null ? await ProxyPath.GetProxiesAsync() : null;
   
            Source = new CancellationTokenSource();
            Token = Source.Token;

            LocalDirectory = Environment.CurrentDirectory + '\\' + AccountsPath.Name + DateTime.Now.ToString(" [dd-MM-yyyy] HH-mm-ss") + "\\";
            Directory.CreateDirectory(LocalDirectory);
            DateForSave = DateTime.Now.ToString("HH-mm-ss");
            
            if (Accounts.Count < Threads)
            {
                Threads = Accounts.Count;
            }
            
            TaskFactory taskFactory = new TaskFactory(Token);
            for (int i = 0; i < Threads; i++)
            {
                TasksList.Add(taskFactory.StartNew(Process, Token));
            }
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            TimerTik();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        }

        protected abstract Task Process();
        protected abstract Task TimerTik();

        public void Dispose()
        {
            Accounts?.Clear();
            Proxies?.Clear();
            Source?.Dispose();
            TasksList.Clear();

        }
    }
}
