using GenericRepository.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GenericRepository.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Connection and Transaction will by apply to dBContext
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="dBContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task MergeContextsTransaction(this IUnitOfWork unitOfWork, DbContext dBContext, CancellationToken cancellationToken = default)
        {
            dBContext.Database.SetDbConnection(unitOfWork.GetConnection());
            await dBContext.Database.UseTransactionAsync(unitOfWork.GetDbTransaction(), cancellationToken);
        }
    }
}
