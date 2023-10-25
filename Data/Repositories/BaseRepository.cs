using Microsoft.EntityFrameworkCore;

using SimpleCRM.Data.DbContexts;
using SimpleCRM.Data.Interfaces;
using SimpleCRM.Data.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCRM.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly MembershipContext context;

        public BaseRepository(MembershipContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(TEntity create)
        {
            await this.context.Set<TEntity>().AddAsync(create);
        }

        public void Delete(TEntity delete)
        {
            this.context.Set<TEntity>().Remove(delete);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }
    }
}
