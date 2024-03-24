using Microsoft.Extensions.Options;
using OLAP.API.Infrastructure.Options;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace OLAP.API.Certificate
{
    public class CertificateManager
    {
        IdentityJWTOptions _options;
        public CertificateManager(IOptions<IdentityJWTOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public X509Certificate2 Get()
        {
            var assembly = typeof(CertificateManager).GetTypeInfo().Assembly;
            var names = assembly.GetManifestResourceNames();

            /***********************************************************************************************
             *  Please note that here we are using a local certificate only for testing purposes. In a 
             *  real environment the certificate should be created and stored in a secure way, which is out
             *  of the scope of this project.
             **********************************************************************************************/
            //using (var stream = assembly.GetManifestResourceStream(_options.Certificate.CertificatePath))
            //{
            //    return new X509Certificate2(ReadStream(stream), _options.Certificate.CertificatePassword);
            //}

            using (var stream = File.OpenRead(_options.Certificate.CertificatePath))
            {
                return new X509Certificate2(ReadStream(stream), _options.Certificate.CertificatePassword);
            }
        }

        private byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
