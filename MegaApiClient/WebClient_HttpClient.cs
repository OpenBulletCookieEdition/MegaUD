using MegaApiClientCore.Enums;
using MegaApiClientCore.Models;

#if !NET40
namespace MegaApiClientCore
{
  using System;
  using System.IO;
  using System.Net;
  using System.Reflection;
  using System.Text;
  using System.Threading;
#if NET471 || NETSTANDARD
  using System.Security.Authentication;
#endif
  using System.Net.Http;
  using System.Net.Http.Headers;
    using MegaApiClientCore.Enums;

    public class WebClient : IWebClient
  {
    private const int DefaultResponseTimeout = Timeout.Infinite;

    private static readonly HttpClient s_sharedHttpClient = CreateHttpClient(DefaultResponseTimeout, GenerateUserAgent(), null);

    private readonly HttpClient _httpClient;

    public WebClient(int responseTimeout = DefaultResponseTimeout, ProductInfoHeaderValue userAgent = null, Proxy proxy = null)
    {
#if NET47
      if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) && !ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.SystemDefault))
      {
        throw new NotSupportedException("mega.nz API requires support for TLS v1.2 or higher. Check https://gpailler.github.io/MegaApiClient/#compatibility for additional information");
      }
#elif NET45 || NET46
      if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
      {
        throw new NotSupportedException("mega.nz API requires support for TLS v1.2 or higher. Check https://gpailler.github.io/MegaApiClient/#compatibility for additional information");
      }
#endif

      if (responseTimeout == DefaultResponseTimeout && userAgent == null && proxy == null)
      {
        _httpClient = s_sharedHttpClient;
      }
      else
      {
        _httpClient = CreateHttpClient(responseTimeout, userAgent ?? GenerateUserAgent(), proxy);
      }
    }

    public int BufferSize { get; set; } = Options.DefaultBufferSize;

    public string PostRequestJson(Uri url, string jsonData)
    {
      using (var jsonStream = new MemoryStream(jsonData.ToBytes()))
      {
        using (var responseStream = PostRequest(url, jsonStream, "application/json"))
        {
          return StreamToString(responseStream);
        }
      }
    }

    public string PostRequestRaw(Uri url, Stream dataStream)
    {
      using (var responseStream = PostRequest(url, dataStream, "application/json"))
      {
        return StreamToString(responseStream);
      }
    }

    public Stream PostRequestRawAsStream(Uri url, Stream dataStream)
    {
      return PostRequest(url, dataStream, "application/octet-stream");
    }

    public Stream GetRequestRaw(Uri url)
    {
      return _httpClient.GetStreamAsync(url).Result;
    }

    private Stream PostRequest(Uri url, Stream dataStream, string contentType)
    {
      using (var content = new StreamContent(dataStream, BufferSize))
      {
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
          Content = content
        };

        var response = _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).Result;
        if (!response.IsSuccessStatusCode
            && response.StatusCode == HttpStatusCode.InternalServerError
            && response.ReasonPhrase == "Server Too Busy")
        {
          return new MemoryStream(Encoding.UTF8.GetBytes(((long)ApiResultCode.RequestFailedRetry).ToString()));
        }

        response.EnsureSuccessStatusCode();

        return response.Content.ReadAsStreamAsync().Result;
      }
    }

    private string StreamToString(Stream stream)
    {
      using (var streamReader = new StreamReader(stream, Encoding.UTF8))
      {
        return streamReader.ReadToEnd();
      }
    }

    private static HttpClient CreateHttpClient(int timeout, ProductInfoHeaderValue userAgent, Proxy proxy)
    {
#if NET471 || NETSTANDARD
            HttpClientHandler handler = new HttpClientHandler()
            {
                SslProtocols = SslProtocols.Tls12
            };
            //https://stackoverflow.com/questions/52939211/the-ssl-connection-could-not-be-established
            //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            if (proxy != null)
            {
                handler.UseProxy = true;

                var proxyInClient = new WebProxy();

                if(proxy.Type == ProxyType.HTTPS)
                {
                    handler.Proxy = new WebProxy(proxy.ProxyString);
                }
                else if(proxy.Type == ProxyType.SOCKS4)
                {
                    proxyInClient.Address = new Uri($"socks4://{proxy.ProxyString}");
                    handler.Proxy = proxyInClient;
                }
                else if (proxy.Type == ProxyType.SOCKS5)
                {
                    proxyInClient.Address = new Uri($"socks5://{proxy.ProxyString}");
                    handler.Proxy = proxyInClient;
                }

            }
            var httpClient = new HttpClient(handler, true); 
#else
      var httpClient = new HttpClient();
#endif
      httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
      httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);

      return httpClient;
    }

    private static ProductInfoHeaderValue GenerateUserAgent()
    {
      var assemblyName = typeof(WebClient).GetTypeInfo().Assembly.GetName();
      return new ProductInfoHeaderValue(assemblyName.Name, assemblyName.Version.ToString(2));
    }
  }
}
#endif
