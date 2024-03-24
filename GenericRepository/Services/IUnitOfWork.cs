using GenericRepository.Models;
using GenericRepository.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace GenericRepository.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T, TIdentity> Repository<T, TIdentity>()
            where T : BaseEntity<TIdentity>
            where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>;

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        IUnitOfWork GetFactory(CancellationToken cancellationToken = default);

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IDbContextTransaction GetContextTransaction();
        DbTransaction GetDbTransaction();

        DbConnection GetConnection();

        void SetConnectionString(string connectionString);

        Task<DateTime> GetDateTimeFromSqlServer(CancellationToken cancellationToken);

        //Task SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
