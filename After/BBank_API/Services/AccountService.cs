using Entities;
using Entities.Responses;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace Services
{
    public class AccountService : IAccountsService
    {
        private readonly BBBankContext _bbBankContext;
        public AccountService(BBBankContext BBBankContext)
        {
            _bbBankContext = BBBankContext;
        }

        public async Task<AccountsListResponse> GetAllAccountsPaginated(int pageIndex, int pageSize)
        {
            ////// totalCount of data available on server.
            ////var totalCount =  _bbBankContext.Accounts.Count();
            ////var accounts =  _bbBankContext.Accounts
            ////    // first n number of records will be skpped based on pageSize and pageIndex
            ////    // for example for pageIndex 2 of pageSize is 10 first 10 records will be skipped.
            ////    .Skip((pageIndex) * pageSize)
            ////    .Take(pageSize)
            ////    .ToList();
            ////return new AccountsListResponse
            ////{
            ////    Accounts = accounts,
            ////    ResultCount = totalCount
            ////};

            // totalCount of data available on server.
            var totalCount = await _bbBankContext.Accounts.CountAsync();
            var accounts = await _bbBankContext.Accounts.Include(x => x.User)
                // first n number of records will be skpped based on pageSize and pageIndex
                // for example for pageIndex 2 of pageSize is 10 first 10 records will be skipped.
                .Skip((pageIndex) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new AccountsListResponse
            {
                Accounts = accounts,
                ResultCount = totalCount
            };
        }

        public async Task OpenAccount(Account account)
        {
            // Setting up ID of new incoming Account to be created.
            account.Id = Guid.NewGuid().ToString();
            // If the user with the same User ID is already in teh system we simply set the userId forign Key of Account with it else 
            // first we create that user and then use it's ID.
            var user = await _bbBankContext.Users.FirstOrDefaultAsync(x => x.Id == account.User.Id);
            if (user == null)
            {
                await _bbBankContext.Users.AddAsync(account.User);
                account.UserId = account.User.Id;
            }
            else
            {
                account.UserId = user.Id;
            }
            // Once User ID forigen key and Account ID Primary Key is set we add the new accoun in Accounts.
            await this._bbBankContext.Accounts.AddAsync(account);
            // Once everything in place we make the Database call.
            await this._bbBankContext.SaveChangesAsync();
        }

        public async Task UpdateAccount(Account account)
        {
            this._bbBankContext.Users.Update(account.User);
            this._bbBankContext.Accounts.Update(account);
            await this._bbBankContext.SaveChangesAsync();
        }

        public async Task<Account> GetAccountByID(string id)
        {
            return await _bbBankContext.Accounts.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }         
    }
}
