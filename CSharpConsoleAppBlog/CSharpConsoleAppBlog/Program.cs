using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CSharpConsoleAppBlog
{
    class Program
    {
        private static readonly string SupportedPublicKey = GetPublicKey();

        private static string GetPublicKey()
        {
            FileStream fs = new FileStream("server.crt", FileMode.Open);
            byte[] certBytes = new byte[fs.Length];
            fs.Read(certBytes, 0, (Int32)fs.Length);
            fs.Close();
            X509Certificate x509cert = new X509Certificate(certBytes);

            return x509cert.GetPublicKeyString();
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return SupportedPublicKey == certificate?.GetPublicKeyString();
        }

        static void Main(string[] args)
        {

            Request().GetAwaiter().GetResult();
        }

        public static async Task Request()
        {

            //HTTP Public Key Pinning
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

            var client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
                   
            var uri = new Uri(string.Format("https://localhost:3000", string.Empty));

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Connection successful!!!");
            }
            
            Console.ReadLine();
        }

        
    }
}
