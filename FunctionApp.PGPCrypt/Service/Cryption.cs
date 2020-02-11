using FunctionApp.PGPCrypt.Interface;
using Microsoft.Extensions.Configuration;
using PgpCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp.PGPCrypt.Service
{
    public class Cryption : ICryption
    {

        private readonly IConfigurationRoot _config;

        public Cryption()
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();
            _config = config;

        }

        public async Task<string> EncryptAsync(string data)
        {
            using (PGP pgp = new PGP())
            {
                var inputStream = GenerateStreamFromString(data);
                var outputStream = new MemoryStream();
                var publicKey = _config["PGP_PublicKey"];

                byte[] b = Convert.FromBase64String(publicKey);

                var strOriginal = System.Text.Encoding.UTF8.GetString(b);

                using (Stream publicKeyStream = GenerateStreamFromString(strOriginal))
                {
                    await pgp.EncryptStreamAsync(inputStream, outputStream, publicKeyStream, true, true);
                    outputStream.Seek(0, SeekOrigin.Begin);
                    return GenerateStringFromStream(outputStream);
                }
            }
        }

        public async Task<string> DecryptAsync(string data)
        {
            using (PGP pgp = new PGP())
            {
                var inputStream = GenerateStreamFromString(data);
                var outputStream = new MemoryStream();
                var privateKey = _config["PGP_PrivateKey"];
                var passphrase = _config["PGP_Passphrase"];

                byte[] b = Convert.FromBase64String(privateKey);

                var strOriginal = System.Text.Encoding.UTF8.GetString(b);

                using (Stream privateKeyStream = GenerateStreamFromString(strOriginal))
                {
                    await pgp.DecryptStreamAsync(inputStream, outputStream, privateKeyStream, passphrase);
                    outputStream.Seek(0, SeekOrigin.Begin);
                    return GenerateStringFromStream(outputStream);
                }
            }
        }

        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private string GenerateStringFromStream(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            var text = reader.ReadToEnd();
            return text;
        }


    }
}
