namespace OLAP.API.Models.Response.Data
{
    public class DataReadModel
    {
        public CountryReadModel Country { get; set; }

        public IndicatorReadModel Indicator { get; set; }

        public string Frequency { get; set; }

        public DateOnly Date { get; set; }

        public float Value { get; set; }
    }
}
