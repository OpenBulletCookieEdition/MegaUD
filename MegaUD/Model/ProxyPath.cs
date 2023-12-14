using System.Collections;
using System.Text.RegularExpressions;
using MegaApiClientCore.Enums;
using System.Net;
using MegaApiClientCore.Models;

namespace MegaUD.Model
{
    public class ProxyPath
    {
        private string Path { get; }
        public ProxyType ProxyType { get; }
        public ProxyPath(string path, ProxyType type)
        {
            Path = path;
            ProxyType = type;
        }

        public async Task<IList<Proxy>> GetProxiesAsync()
        {
            switch (Path)
            {
                case var path when new Regex(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$").IsMatch(path):
                    return await ProxyParseFromLinkAsync(path,ProxyType);
                case var path when new Regex(@".txt").IsMatch(path):
                    return await ProxyParseFromPathAsync(path, ProxyType);
                default:
                    throw new Exception("Path proxy invalid");
            }
        }

        private async Task<IList<Proxy>> ProxyParseFromPathAsync(string path,ProxyType type)
        {
            Regex regex = new Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}:\\d{1,6}");

            IList<Proxy> proxies = new List<Proxy>();
            foreach (var proxy in await File.ReadAllLinesAsync(path))
            {
                if(regex.IsMatch(proxy)) proxies.Add(new Proxy(proxy,type));
            }

            return proxies;

        }
        private async Task<IList<Proxy>> ProxyParseFromLinkAsync(string link, ProxyType type)
        {
            HashSet<Proxy> result = new HashSet<Proxy>();

            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 10,

            };
            using HttpClient client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMilliseconds(30000),

            };
            try
            {
                Regex regex = new Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}:\\d{1,6}");
                string input = await client.GetStringAsync(link);

                foreach (Match item in regex.Matches(input))
                {

                    result.Add(new Proxy(item.Value,type));

                }
                
            }
            catch (WebException)
            {
                throw;
            }
            catch (UriFormatException)
            {
                throw new ArgumentException("Invalid Url");
            }
            catch (Exception)
            {
            }
            return result.ToList();
            
        }
    }
}


