
namespace Setoma.CompSci.Dis.FiftyBest.Data;

public interface IDataStore
{
    Task CreateUser(string userName);
    Task<bool> UserExists(string userName);
}