namespace OLAP.API.Infrastructure.Options
{
    public class CertificateOptions
    {
        public static readonly string SectionName = "Certificate";

        public string CertificatePath { get; set; }

        public string CertificatePassword { get; set; }

        public string SigningAlgorithm { get; set; }
    }
}
