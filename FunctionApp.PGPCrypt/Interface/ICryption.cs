using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp.PGPCrypt.Interface
{
    public interface ICryption
    {
        Task<string> EncryptAsync(String data);
        Task<string> DecryptAsync(String data);
    }
}
