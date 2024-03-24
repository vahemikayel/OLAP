using GenericRepository.Models;
using System.Diagnostics.Metrics;

namespace OLAP.API.Models.Entity.Data
{
    public class DataEModel : BaseEntity<Guid>
    {
        public Guid CountryId { get; set; }

        public Guid IndicatorId { get; set; }

        public string Frequency { get; set; }

        public DateOnly Date { get; set; }

        public float Value { get; set; }

        public CountryEModel Country { get; set; }

        public IndicatorEModel Indicator { get; set; }
    }
}
