using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses
{
    public class AccountsListResponse
    {
        public IEnumerable<Account> Accounts { get; set; }
        // paginator requires total counts of the data available on server
        public int ResultCount { get; set; }
    }
}
