using MegaApiClientCore.Enums;
using MegaApiClientCore.Models;
using MegaUD.Model;

namespace MegaUD.Test.ModelTest;

public class ProxyPathTest
{
    [Fact]
    public async Task GetProxiesAsync_PathToFolder_ReturnProxiesList()
    {
        //Arrange 
        ProxyPath proxyPath =
            new ProxyPath("C:\\Users\\Administrator\\Desktop\\TesAccountsAndProxyForMegaUD\\proxy.txt", ProxyType.SOCKS5);
        
        //Act 
        IList<Proxy> proxiesList = await proxyPath.GetProxiesAsync();
        
        //Assert
        Assert.NotEmpty(proxiesList);
    }
    
    [Fact]
    public async Task GetProxiesAsync_Link_ReturnProxiesList()
    {
        //Arrange 
        ProxyPath proxyPath =
            new ProxyPath("https://proxy.notdev.space/proxies/socks5.txt", ProxyType.SOCKS5);
        
        //Act 
        IList<Proxy> proxiesList = await proxyPath.GetProxiesAsync();
        
        //Assert
        Assert.NotEmpty(proxiesList);
    }
}