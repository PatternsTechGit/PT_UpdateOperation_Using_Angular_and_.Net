﻿using Entities;
using Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IAccountsService
    {
        Task<AccountsListResponse> GetAllAccountsPaginated(int pageIndex, int pageSize);
        Task OpenAccount(Account account);
    }
}
