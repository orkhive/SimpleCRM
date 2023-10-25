using SimpleCRM.Data.Entities;

namespace SimpleCRM.Data.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T create);
        void Delete(T delete);
        Task<bool> SaveChangesAsync();
    }
}