using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Repository.Repositories.Interface;

namespace PRN222.Lab2.ProductStore.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyStoreDbContext _context;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public IAccountMemberRepository AccountMembers { get; }

        public UnitOfWork(MyStoreDbContext context,
                          IProductRepository products,
                          ICategoryRepository categories,
                          IAccountMemberRepository accountMembers)
        {
            _context = context;
            Products = products;
            Categories = categories;
            AccountMembers = accountMembers;
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }


}
