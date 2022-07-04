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
            // totalCount of data available on server.
            var totalCount =  _bbBankContext.Accounts.Count();
            var accounts =  _bbBankContext.Accounts
                // first n number of records will be skpped based on pageSize and pageIndex
                // for example for pageIndex 2 of pageSize is 10 first 10 records will be skipped.
                .Skip((pageIndex) * pageSize)
                .Take(pageSize)
                .ToList();
            return new AccountsListResponse
            {
                Accounts = accounts,
                ResultCount = totalCount
            };
        }
    }
}
