using GenericRepository.Models;
using GenericRepository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GenericRepository.Repositories
{
    public interface IGenericRepository<T, TIdentity>
        where T : BaseEntity<TIdentity>
        where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>//IConvertible, 
         //where TDBContext : DbContext, new()
    {
        T Add(T entity);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void UpdateIfStateNone(T entity);
        Task<T> RemoveAsync(TIdentity id, CancellationToken cancellationToken = default);
        T Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        Task<int> Remove(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(TIdentity id, ISpecification<T> specification = null, CancellationToken cancellationToken = default);
        Task<T> GetAsync(TIdentity id, bool asNoTracking, CancellationToken cancellationToken = default);
        Task<T> GetAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        Task<List<T>> GetByIdsAsync(List<TIdentity> ids, CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(ISpecification<T> specification = null, CancellationToken cancellationToken = default);
        IQueryable<T> GetAll();
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<bool> ExistByIdAsync(TIdentity id, CancellationToken cancellationToken = default);
        Task<bool> ExistByIdsAsync(List<TIdentity> ids, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        IQueryable<T> Find(ISpecification<T> specification);

        Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> expression, CancellationToken cancellation);
        Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        Task<bool> AnyOtherAsync(TIdentity id, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
