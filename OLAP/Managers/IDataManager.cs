using OLAP.API.Models.Request.Data;

namespace OLAP.API.Managers
{
    public interface IDataManager
    {
        Task<bool> UploadData(List<DataRequestModel> data, CancellationToken cancellationToken = default);
    }
}
