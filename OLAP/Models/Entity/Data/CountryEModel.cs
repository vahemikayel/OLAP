using GenericRepository.Models;

namespace OLAP.API.Models.Entity.Data
{
    public class CountryEModel : BaseEntity<Guid>
    {
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public IList<DataEModel> DataItems { get; set; } = new List<DataEModel>();
    }
}
