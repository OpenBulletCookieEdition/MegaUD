namespace MegaUD.WorkStation;

public interface IWorkStation : IDisposable
{
    Task StartAsync();
}