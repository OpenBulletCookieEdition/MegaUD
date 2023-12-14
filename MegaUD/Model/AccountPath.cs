using System;
using System.Collections.Concurrent;


namespace MegaUD.Model
{
    public class AccountPath
    {
        private string Path { get; }
        public string Name => Path.Substring(Path.LastIndexOf('\\') + 1);
        public AccountPath(string path)
        {
            Path = path;
        }

        public async Task<ConcurrentStack<Account>> GetAccountsAsync()
        {
            IEnumerable<string> accountsString = await File.ReadAllLinesAsync(Path);
            ConcurrentStack<Account> accounts = new ConcurrentStack<Account>();

            foreach (string account in accountsString)
            {
                string[] array = account.Split(';', ':');
                if (array.Length > 2 || !array[0].Contains('@') || !array[0].Split('@')[1].Contains('.'))
                {
                    continue;
                }
                string email = array[0];
                string password = array[1];

                if (String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(email)) continue;

                accounts.Push(new Account(account,email, password));
            }
            return accounts;

        }
    }
}
