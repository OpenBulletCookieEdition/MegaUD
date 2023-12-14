using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MegaApiClientCore;
using MegaApiClientCore.Enums;
using MegaApiClientCore.Models;
using FakeItEasy;
using MegaUD.Model;
using MegaUD.WorkStation;

namespace MegaUD.Test.WorkStationTest
{
    public class WorkStationMegaUploadTest
    {
        private readonly WorkStationMegaUpload _workStationMegaUpload;

        public WorkStationMegaUploadTest()
        {
            _workStationMegaUpload =  new WorkStationMegaUpload(new AccountPath(""), "C:\\Users\\Administrator\\Desktop\\TestUpload", 1, 1, null);
        }


        
        //valid account
        [Fact]
        public void Login_ValidAccountWithoutProxy_ReturnMegaApiClient()
        {
            //Arrange
            Account account = new Account("", "virtualcursosjuridicos@zipmail.com.br", "w30k21l28i24j21");
            Proxy? proxy = null;
            var loginMethod =
                typeof(WorkStationMegaUpload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            MegaApiClient? megaApiClient = (MegaApiClient)loginMethod?.Invoke(_workStationMegaUpload, new object?[2] { account, proxy })!;
            megaApiClient.Logout();

            //Assert
            Assert.NotNull(megaApiClient);
        }

        
        //not valid account
        [Fact]
        public void Login_NotValidAccountWithoutProxy_ReturnExceptionResourceNotExistCode()
        {
            //Arrange
            Account account = new Account("", "virtualcursosjuridicos@zipmail.com.br", "wugwieewu341");
            Proxy? proxy = null;
            var loginMethod =
                typeof(WorkStationMegaUpload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            Action act = () => loginMethod.Invoke(_workStationMegaUpload, new object?[2] { account, proxy });
            TargetInvocationException exception = Assert.Throws<TargetInvocationException>(act);
            ApiException? apiException = exception.InnerException as ApiException;

            //Assert
            Assert.NotNull(apiException);
            Assert.Equal(ApiResultCode.ResourceNotExists, apiException.ApiResultCode);
        }

        //Not registered account ApiResultCode.ResourceNotExists
        [Fact]
        public void Login_NotRegisterAccountWithoutProxy_ReturnExceptionResourceNotExistCode()
        {
            //Arrange
            Account account = new Account("", "irtua5345juridicos@zipmail.com.br", "w30k21l28i24j21");
            Proxy? proxy = null;
            var loginMethod =
                typeof(WorkStationMegaUpload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            Action act = () => loginMethod.Invoke(_workStationMegaUpload, new object?[2] { account, proxy });
            TargetInvocationException exception = Assert.Throws<TargetInvocationException>(act);
            ApiException? apiException = exception.InnerException as ApiException;

            //Assert
            Assert.NotNull(apiException);
            Assert.Equal(ApiResultCode.ResourceNotExists, apiException.ApiResultCode);
        }
        
        //upload success
        [Fact]
        public void Upload_MegaApiClient_ReturnBool()
        {
            
            //Arrange
            Account account = new Account("", "cracenyy@gmail.com", "simas99000");
            Proxy? proxy = null;
            var loginMethod =
                typeof(WorkStationMegaUpload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);
            MegaApiClient megaApiClient = (MegaApiClient)loginMethod.Invoke(_workStationMegaUpload, new object?[2] { account, proxy });
            var uploadMethod =
                typeof(WorkStationMegaUpload).GetMethod("Upload", BindingFlags.NonPublic | BindingFlags.Instance);
            
            //Act
            bool uploadResult = (bool)uploadMethod.Invoke(_workStationMegaUpload, new object[1]{megaApiClient});
            megaApiClient.Logout();
            
            //Assert
            Assert.True(uploadResult);
        }
        
        //upload error QuotaExceeded
        [Fact]
        public void Upload_MegaApiClient_ReturnExceptionQuotaExceeded()
        {
            
            //Arrange
            Account account = new Account("", "alexisbra_13@hotmail.com", "brahyam1096");
            Proxy? proxy = null;
            var loginMethod =
                typeof(WorkStationMegaUpload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);
            MegaApiClient megaApiClient = (MegaApiClient)loginMethod.Invoke(_workStationMegaUpload, new object?[2] { account, proxy });
            var uploadMethod =
                typeof(WorkStationMegaUpload).GetMethod("Upload", BindingFlags.NonPublic | BindingFlags.Instance);
            
            //Act
            Action act = () => uploadMethod.Invoke(_workStationMegaUpload, new object[1]{megaApiClient});
            TargetInvocationException exception = Assert.Throws<TargetInvocationException>(act);
            ApiException? apiException = exception.InnerException as ApiException;
            megaApiClient.Logout();
            
            //Assert
            Assert.NotNull(apiException);
            Assert.Equal(ApiResultCode.QuotaExceeded, apiException.ApiResultCode);
        }
        
        
        
        //Error Proxy
        [Fact]
        public void Login_AccountWithNotValidProxy_ReturnExceptionAggregateException()
        {
            //Arrange
            Account account = new Account("", "virtualcursosjuridicos@zipmail.com.br", "w30k21l28i24j21");
            Proxy? proxy = new Proxy("155.139.154.101:8800",ProxyType.HTTPS);
            var loginMethod =
                typeof(WorkStationMegaUpload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            Action act = () => loginMethod.Invoke(_workStationMegaUpload, new object?[2] { account, proxy });
            TargetInvocationException exception = Assert.Throws<TargetInvocationException>(act);
            AggregateException? aggregateException = exception.InnerException as AggregateException;

            //Assert
            Assert.NotNull(aggregateException);
            
        }
        
        //valid https proxy
        [Fact]
        public void Login_AccountWithValidHttpsProxy_ReturnMegaApiClient()
        {
            //Arrange
            Account account = new Account("", "virtualcursosjuridicos@zipmail.com.br", "w30k21l28i24j21");
            Proxy? proxy = new Proxy("89.58.48.220:10004",ProxyType.HTTPS);
            var loginMethod =
                typeof(WorkStationMegaUpload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            MegaApiClient? megaApiClient = (MegaApiClient)loginMethod?.Invoke(_workStationMegaUpload, new object?[2] { account, proxy })!;
            megaApiClient.Logout();

            //Assert
            Assert.NotNull(megaApiClient);
        }
        
        //valid socks5 proxy
        [Fact]
        public void Login_AccountWithValidSocks5Proxy_ReturnMegaApiClient()
        {
            //Arrange
            Account account = new Account("", "virtualcursosjuridicos@zipmail.com.br", "w30k21l28i24j21");
            Proxy? proxy = new Proxy("207.244.247.118:31168",ProxyType.SOCKS5);
            var loginMethod =
                typeof(WorkStationMegaUpload).GetMethod("Login", BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            MegaApiClient? megaApiClient = (MegaApiClient)loginMethod?.Invoke(_workStationMegaUpload, new object?[2] { account, proxy })!;
            megaApiClient.Logout();

            //Assert
            Assert.NotNull(megaApiClient);
        }
    }
  
}
