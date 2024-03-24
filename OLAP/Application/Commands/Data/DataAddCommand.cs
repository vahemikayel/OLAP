using MediatR;
using OLAP.API.Infrastructure.BaseReuqestTypes;
using OLAP.API.Managers;
using OLAP.API.Models.Request.Data;

namespace OLAP.API.Application.Commands.Data
{
    public class DataAddCommand : BaseRequest<bool>
    {
        public List<DataRequestModel> Items { get; set; }
    }

    public class DataAddCommandHandler : IRequestHandler<DataAddCommand, bool>
    {
        private readonly IDataManager _dataManager;

        public DataAddCommandHandler(IDataManager dataManager)
        {
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        public async Task<bool> Handle(DataAddCommand request, CancellationToken cancellationToken)
        {
            return await _dataManager.UploadData(request.Items, cancellationToken);
        }
    }
}
