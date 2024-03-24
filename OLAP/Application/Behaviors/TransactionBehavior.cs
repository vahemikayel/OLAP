using MediatR;
using GenericRepository.Services;
using OLAP.API.Infrastructure.BaseReuqestTypes;

namespace OLAP.API.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : BaseRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!request.UsesTransaction)
            {
                return await next();
            }

            TResponse response = default;
            try
            {
                if (request.UsesTransaction)
                    await _unitOfWork.BeginTransactionAsync(cancellationToken);

                response = await next();

                if (request.IsDataEdit)
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (request.UsesTransaction)
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                if (request.UsesTransaction)
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
