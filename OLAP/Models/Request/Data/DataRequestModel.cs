namespace OLAP.API.Models.Request.Data
{
    public class DataRequestModel
    {
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string IndicatorName { get; set; }

        public string IndicatorCode { get; set; }

        public string Frequency { get; set; }

        public DateOnly Date { get; set; }

        public float Value { get; set; }
    }
}
