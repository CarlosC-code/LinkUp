using System.Linq.Expressions;

namespace LinkUp.Core.Domain.Interface
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity?> AddAsync(Entity entity);
        Task<List<Entity>> AddRangeAsync(List<Entity> entities);

        // UPDATE
        Task<Entity?> UpdateAsync(int id, Entity entity);
        Task<Entity?> UpdateAsync(Entity entity);

        // DELETE
        Task DeleteAsync(int id);
        Task DeleteAsync(Expression<Func<Entity, bool>> predicate);

        // READ
        Task<Entity?> GetById(int id);
        Task<List<Entity>> GetAllList();
        Task<List<Entity>> GetAllListWithInclude(List<string> properties);

        // QUERY
        IQueryable<Entity> GetAllQuery();
        IQueryable<Entity> GetAllQueryWithInclude(List<string> properties);

        // EXTRA
        Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate);
        Task<List<Entity>> FindAsync(Expression<Func<Entity, bool>> predicate);
        Task<Entity?> FirstOrDefaultAsync(Expression<Func<Entity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<Entity, bool>> predicate);
    }
}