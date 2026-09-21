using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity>
        where Entity : class
    {
        private readonly LinkUpAppContext _context;

        public GenericRepository(LinkUpAppContext context)
        {
            _context = context;
        }

        // ---------------- CREATE ----------------
        public virtual async Task<Entity?> AddAsync(Entity entity)
        {
            await _context.Set<Entity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<List<Entity>> AddRangeAsync(List<Entity> entities)
        {
            await _context.Set<Entity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        // ---------------- UPDATE ----------------
        public virtual async Task<Entity?> UpdateAsync(int id, Entity entity)
        {
            var entry = await _context.Set<Entity>().FindAsync(id);
            if (entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entry;
            }
            return null;
        }

        // NUEVO: actualizar sin necesitar el id por separado
        public virtual async Task<Entity?> UpdateAsync(Entity entity)
        {
            _context.Set<Entity>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // ---------------- DELETE ----------------
        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Entity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<Entity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // NUEVO: eliminar por predicado (ej: eliminar amistad por userId)
        public virtual async Task DeleteAsync(Expression<Func<Entity, bool>> predicate)
        {
            var entities = await _context.Set<Entity>().Where(predicate).ToListAsync();
            if (entities.Any())
            {
                _context.Set<Entity>().RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        // ---------------- READ ----------------
        public virtual async Task<Entity?> GetById(int id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }

        public virtual async Task<List<Entity>> GetAllList()
        {
            return await _context.Set<Entity>().ToListAsync();
        }

        public virtual async Task<List<Entity>> GetAllListWithInclude(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();
            foreach (var property in properties)
                query = query.Include(property);
            return await query.ToListAsync();
        }

        // ---------------- QUERY ----------------
        public virtual IQueryable<Entity> GetAllQuery()
        {
            return _context.Set<Entity>().AsQueryable();
        }

        public virtual IQueryable<Entity> GetAllQueryWithInclude(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();
            foreach (var property in properties)
                query = query.Include(property);
            return query;
        }

        
        public virtual async Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await _context.Set<Entity>().AnyAsync(predicate);
        }

     
        public virtual async Task<List<Entity>> FindAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await _context.Set<Entity>().Where(predicate).ToListAsync();
        }

        
        public virtual async Task<Entity?> FirstOrDefaultAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await _context.Set<Entity>().FirstOrDefaultAsync(predicate);
        }

        
        public virtual async Task<int> CountAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await _context.Set<Entity>().CountAsync(predicate);
        }
    }
}