using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyStoreContext _context;
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<AccountMember> AccountMembers { get; }

        public UnitOfWork(MyStoreContext context)
        {
            _context = context;
            Products = new GenericRepository<Product>(context);
            Categories = new GenericRepository<Category>(context);
            AccountMembers = new GenericRepository<AccountMember>(context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
