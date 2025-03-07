using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AccountDAO
    {
        public static AccountMember GetAccountById(string accountId)
        {
            using var context = new MyStoreContext();
            return context.AccountMembers.FirstOrDefault(c => c.MemberId.Equals(accountId));
        }
    }
}
