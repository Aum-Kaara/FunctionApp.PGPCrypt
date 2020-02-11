using FluentAssertions;
using FunctionApp.PGPCrypt.Interface;
using FunctionApp.PGPCrypt.Service;
using System;
using System.IO;
using Xunit;

namespace FunctionApp.PGPCrypt.Tests
{
    public class CryptionUnitTest
    {
        [Fact]
        public async void EncryptAsyncShouldEncryptdata()
        {
            var data = "Hello World PGPCore!";
            var decryptedContentFilePath1 = @"..\..\..\TestData\content_encrypted.pgp";
            //Act
            var sut = new Cryption();
            var result = await sut.EncryptAsync(data);
            string decryptedContent = await File.ReadAllTextAsync(decryptedContentFilePath1);

            //Assert
            result.Should().Equals(decryptedContent);
        }

        [Fact]
        public async void DecryptAsyncShouldDecryptdata()
        {
            var data = "Hello World PGPCore!";
            var TestdataFileName = @"..\..\..\TestData\content_encrypted.pgp";
            string decryptedContent = await File.ReadAllTextAsync(TestdataFileName);
            //Act
            var sut = new Cryption();
            var result = await sut.DecryptAsync(decryptedContent);

            //Assert
            result.Should().Equals(data);

        }
    }
}
