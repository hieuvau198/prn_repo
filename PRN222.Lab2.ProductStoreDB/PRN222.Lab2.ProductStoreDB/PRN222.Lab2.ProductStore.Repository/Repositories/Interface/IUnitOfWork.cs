using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Lab2.ProductStore.Repository.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IAccountMemberRepository AccountMembers { get; }
        Task<int> CompleteAsync();

    }

}
