using GenericRepository.Models;
using GenericRepository.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GenericRepository.Repositories
{
    public class GenericRepository<T, TIdentity, TDBContext> : IGenericRepository<T, TIdentity>
        where T : BaseEntity<TIdentity>
        where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>
        where TDBContext : DbContext, new()
    {
        private readonly TDBContext _context;// BaseDBContext<TContext> _context;

        public GenericRepository(TDBContext context)//BaseDBContext<TContext> context
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public T Add(T entity)
        {
            var a = _context.Set<T>()
                            .Add(entity);
            return a.Entity;
        }
        
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var a = await _context.Set<T>()
                                  .AddAsync(entity, cancellationToken);
            return a.Entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>()
                          .AddRangeAsync(entities, cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).CountAsync(cancellationToken);
        }

        public async Task<bool> ExistByIdAsync(TIdentity id, CancellationToken cancellationToken = default)
        {
            //var item = await _context.Set<T>()
            //                         .FindAsync(id, cancellationToken);
            var item = await _context.Set<T>()
                                     .FindAsync(new object[] { id }, cancellationToken);
            return item != null;
        }

        public async Task<bool> ExistByIdsAsync(List<TIdentity> ids, CancellationToken cancellationToken = default)
        {
            var exist = await _context.Set<T>()
                                      .AnyAsync(x => ids.Contains(x.Id), cancellationToken);
            return exist;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                                 .Where(expression)
                                 .ToListAsync(cancellationToken);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                           .Where(expression);
        }
        
        public IQueryable<T> Find(ISpecification<T> specification)
        {
            return ApplySpecification(specification);
        }

        public async Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> expression, CancellationToken cancellation)
        {
            return await _context.Set<T>()
                                 .MaxAsync(expression, cancellation);
        }

        public async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).AnyAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                                 .AnyAsync(expression, cancellationToken);
        }

        public async Task<bool> AnyOtherAsync(TIdentity id, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                                 .Where(x => !x.Id.Equals(id))
                                 .AnyAsync(expression, cancellationToken);
        }

        public async Task<List<T>> GetAllAsync(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>()
                           .AsQueryable();
        }

        public async Task<T> GetAsync(TIdentity id, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<T> GetAsync(TIdentity id, bool asNoTracking, CancellationToken cancellationToken = default)
        {
            var query = _context.Set<T>()
                                .AsQueryable();
            if (asNoTracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }
        
        public async Task<T> GetAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }
        
        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                                 .FirstOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<List<T>> GetByIdsAsync(List<TIdentity> ids, CancellationToken cancellationToken = default)
        {
            var res = await _context.Set<T>()
                                    .Where(x => ids.Contains(x.Id))
                                    .ToListAsync(cancellationToken);
            return res;
        }

        public async Task<T> RemoveAsync(TIdentity id, CancellationToken cancellationToken = default)
        {
            var a = await _context.Set<T>()
                                  .FindAsync(id, cancellationToken);

            var res = _context.Set<T>()
                              .Remove(a);
            return res.Entity;
        }

        public T Remove(T entity)
        {
            var res = _context.Remove(entity);
            return res.Entity;
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _context.RemoveRange(entity);
        }
        
        public async Task<int> Remove(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ExecuteDeleteAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>()
                    .Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateIfStateNone(T entity)
        {
            if (_context.Entry(entity).State != EntityState.Unchanged)
                return;

            Update(entity);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T, TIdentity>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }
    }
}
