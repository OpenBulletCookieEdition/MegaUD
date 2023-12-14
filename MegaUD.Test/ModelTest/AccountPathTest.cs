using MegaUD.Model;

namespace MegaUD.Test.ModelTest;

public class AccountPathTest
{
    [Fact]
    public async Task GetAccountsAsync_PathToFolder_ReturnAccountsList()
    {
        //Arrange
        AccountPath accountPath = new AccountPath("C:\\Users\\Administrator\\Desktop\\TesAccountsAndProxyForMegaUD\\accounts.txt");
        
        //Act
        IEnumerable<Account> accounts = await accountPath.GetAccountsAsync();
        
        //Assert
        Assert.NotEmpty(accounts);
    }
}