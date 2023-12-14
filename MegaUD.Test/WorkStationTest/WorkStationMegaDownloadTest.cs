using System.Collections.Concurrent;
using System.Reflection;
using FakeItEasy;
using MegaApiClientCore;
using MegaApiClientCore.Models;
using MegaUD.Model;
using MegaUD.WorkStation;

namespace MegaUD.Test.WorkStationTest;

public class WorkStationMegaDownloadTest
{
    private readonly WorkStationMegaDownload _workStationMegaDownload;

    public WorkStationMegaDownloadTest()
    {
        _workStationMegaDownload = new WorkStationMegaDownload(new AccountPath(""),1,1,new List<string>{".txt"},40,"C:\\Users\\Administrator\\Desktop\\TestDownload",null);
    }
    
    [Fact]
    public void DownloadNode_NodeMegaApiClient_ReturnBool()
    {
        //Arrange
        Account account = new Account("", "virtualcursosjuridicos@zipmail.com.br", "w30k21l28i24j21");
        Proxy? proxy = null;
        var loginMethod =
            typeof(WorkStationMegaDownload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);
        MegaApiClient megaApiClient = (MegaApiClient)loginMethod.Invoke(_workStationMegaDownload, new object?[2] { account, proxy });
        INode node = megaApiClient.GetNodes()
            .First(node => node.Type == NodeType.File && node.Name.EndsWith(".pdf"));
        var downloadNodeMethod =
            typeof(WorkStationMegaDownload).GetMethod("DownloadNode", BindingFlags.NonPublic | BindingFlags.Instance);
            
        //Act
        bool uploadResult = (bool)downloadNodeMethod.Invoke(_workStationMegaDownload, new object[2]{node,megaApiClient});
        megaApiClient.Logout();
            
        //Assert
        Assert.True(uploadResult);
    }

    [Fact]
    public void Download_MegaApiClient_ReturnBool()
    {
        //Arrange
        Account account = new Account("", "virtualcursosjuridicos@zipmail.com.br", "w30k21l28i24j21");
        Proxy? proxy = null;
        var loginMethod =
            typeof(WorkStationMegaDownload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);
        
        MegaApiClient megaApiClient = (MegaApiClient)loginMethod.Invoke(_workStationMegaDownload, new object?[2] { account, proxy });
        var downloadNodeMethod =
            typeof(WorkStationMegaDownload).GetMethod("Download", BindingFlags.NonPublic | BindingFlags.Instance);
        
        //Act
        bool uploadResult = (bool)downloadNodeMethod.Invoke(_workStationMegaDownload, new object[1]{megaApiClient});
        megaApiClient.Logout();
            
        //Assert
        Assert.True(uploadResult);
    }
    
    [Fact]
    public void Step__ReturnBool()
    {
        //Arrange
        var testWorkstation = new WorkStationMegaDownload(new AccountPath("C:\\Users\\Administrator\\Desktop\\TesAccountsAndProxyForMegaUD\\account.txt"),1,1,new List<string>{".pdf"},5,"C:\\Users\\Administrator\\Desktop\\TestDownload",null);
        var stepMethod =
            typeof(WorkStationMegaDownload).GetMethod("Step", BindingFlags.NonPublic | BindingFlags.Instance);
        ConcurrentStack<Account> stack = new ConcurrentStack<Account>();
        stack.Push(new Account("","virtualcursosjuridicos@zipmail.com.br","w30k21l28i24j21"));
        var accountsProperty =  typeof(WorkStationMegaDownload).GetProperty("Accounts",BindingFlags.NonPublic | BindingFlags.Instance);
        accountsProperty.SetValue(testWorkstation,stack,null);
        
        //Act
        bool stepMethodResult = (bool)stepMethod.Invoke(testWorkstation, null);
            
        //Assert
        Assert.True(stepMethodResult);
    }

}