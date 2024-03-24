using GenericRepository.Models;

namespace OLAP.API.Models.Entity.Data
{
    public class IndicatorEModel : BaseEntity<Guid>
    {
        public string IndicatorName { get; set; }

        public string IndicatorCode { get; set; }

        public IList<DataEModel> DataItems { get; set; } = new List<DataEModel>();
    }
}
