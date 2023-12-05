using Microsoft.EntityFrameworkCore;
using UnitTestDemo.Model;

namespace UnitTestDemo.Service
{
    public class AccountsService: IAccountsService
    {
        public readonly AccountsDbContext orgDbContext;
        public AccountsService(AccountsDbContext orgDbContext) 
        {
            this.orgDbContext = orgDbContext;
        }

        public async Task<Guid> Add(string accountName,string accountAddress,long balance)
        {
            var accountId=Guid.NewGuid();
            orgDbContext.Accounts.Add(new Account
            {
                AccountId = accountId,
                AccountAddress = accountAddress,
                AccountBalance = balance,
                AccountName = accountName
            });
            await orgDbContext.SaveChangesAsync();
            return accountId;
        }

        public async Task<Account> Get(Guid accountId)
        {
            return await orgDbContext.Accounts.Where(x=>x.AccountId==accountId).FirstAsync();
        }

        public IEnumerable<Account> GetAll()
        {
            return  orgDbContext.Accounts.AsEnumerable();
        }

        public async Task Trasfer(Guid sourceAccountId, Guid targetAccountId, long transferAmount)
        {
            var strategy = orgDbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transaction = orgDbContext.Database.BeginTransaction();
                try
                {
                    var sourceAccount =await Get(sourceAccountId);
                    if (sourceAccount == null)
                        throw new Exception("Account not found");
                    if (sourceAccount.AccountBalance - transferAmount > 0)
                    {
                        sourceAccount.AccountBalance -= transferAmount;
                        var targetAccount = await Get(targetAccountId);
                        if (targetAccount == null)
                            throw new Exception("Account not found");
                        targetAccount.AccountBalance += transferAmount;
                        await orgDbContext.SaveChangesAsync();
                    }
                    else
                        throw new Exception("Not enough balance");
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            });
        }
    }
}
