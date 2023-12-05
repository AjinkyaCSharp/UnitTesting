using UnitTestDemo.Model;

namespace UnitTestDemo.Service
{
    public interface IAccountsService
    {
        Task<Guid> Add(string accountName, string accountAddress, long balance);
        Task<Account> Get(Guid accountId);
        IEnumerable<Account> GetAll();
        Task Trasfer(Guid sourceAccountId, Guid targetAccountId, long transferAmount);

    }
}
