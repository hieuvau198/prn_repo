﻿using PRN222.ProductStore.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Repository.Repositories.Interfaces
{
    public interface IAccountMemberRepository : IGenericRepository<AccountMember>
    {
        Task<AccountMember> GetAccountByEmail(string email, string password);
    }
}
