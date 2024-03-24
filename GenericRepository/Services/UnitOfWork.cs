using GenericRepository.Models;
using GenericRepository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace GenericRepository.Services
{
    public class UnitOfWork<TDBContext> : IUnitOfWork, IDisposable, IAsyncDisposable
        where TDBContext : DbContext, new()
        //where TDBContext2 : DbContext, new()
    {
        private readonly Hashtable _repositories;
        private IDbContextTransaction _transaction;
        private TDBContext _baseDBContext { get; set; }//BaseDBContext<TDBContext> _baseDBContext;
        
        //private ConcurrentDictionary<EContextType, TDBContext> _contexts;

        public UnitOfWork(TDBContext baseDBContext)//BaseDBContext<TDBContext> baseDBContext
        {
            _repositories = new Hashtable();
            //_contexts = new ConcurrentDictionary<EContextType, TDBContext>();
            _baseDBContext = baseDBContext ?? throw new ArgumentNullException(nameof(baseDBContext));
            //_baseDBContext = dBContext2 ?? throw new ArgumentNullException(nameof(dBContext2));
        }

        public IDbContextTransaction GetContextTransaction()
        {
            return _transaction;
        }

        public DbTransaction GetDbTransaction()
        {
            return _transaction.GetDbTransaction();
        }

        public DbConnection GetConnection()
        {
            return _baseDBContext.Database.GetDbConnection();
        }

        public void SetConnectionString(string connectionString)
        {
            _baseDBContext.Database.SetConnectionString(connectionString);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _baseDBContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _baseDBContext.Database.CommitTransactionAsync(cancellationToken);
            _transaction = null;
        }

        public void Dispose()
        {
             _transaction?.Rollback();
            _baseDBContext.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            //if (_transaction != null && _transaction.)
            //    await _transaction?.RollbackAsync();

            await _baseDBContext.DisposeAsync();
        }

        public IUnitOfWork GetFactory(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _baseDBContext.Database.RollbackTransactionAsync(cancellationToken);
            _transaction = null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _baseDBContext.SaveChangesAsync(cancellationToken);
        }
        
        public int SaveChanges()
        {
            return _baseDBContext.SaveChanges();
        }

        //public Task SaveEntitiesAsync(CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<DateTime> GetDateTimeFromSqlServer(CancellationToken cancellationToken)
        {
            var connection = GetConnection();
            using (var cmd = connection.CreateCommand())
            {
                await connection.OpenAsync(cancellationToken);
                cmd.CommandText = "SELECT GETDATE()";
                var res = await cmd.ExecuteScalarAsync(cancellationToken);
                await connection.CloseAsync();
                return (DateTime)res;
            }
        }

        public IGenericRepository<T, TIdentity> Repository<T, TIdentity>()
            where T : BaseEntity<TIdentity>
            where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>
        {
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<T, TIdentity, TDBContext>);
                var repositoryInstance = Activator.CreateInstance(repositoryType, new object[] { _baseDBContext });

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T, TIdentity>)_repositories[type];
        }

    }
}
