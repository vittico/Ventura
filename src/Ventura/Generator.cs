﻿using System;
using System.IO;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

using Ventura.Exceptions;
using static Ventura.Constants;

using Medo.Security.Cryptography;

namespace Ventura
{
    public sealed class Generator: IGenerator
    {
        private readonly SymmetricAlgorithm cipher;
        private readonly GeneratorState state;

        public Generator(CipherOption option)
        {
            state = new GeneratorState
            {
                Counter = 0,
                Seed = new byte[] { }
            };

            switch (option)
            {
                case CipherOption.Aes:
                    cipher = Aes.Create();
                    break;
                default:
                    cipher = TwofishManaged.Create();
                    break;
            }

            cipher.KeySize = 256;
        }

        /// <summary>
        /// Will generate up to 2^20 worth of random data to reduce
        /// the statistical deviation from perfectly random outputs. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] GenerateRandomData(byte[] input)
        {
            //TODO: cannot process 2^20 worth of data or output 2^20 worth of encrypted data?? 
            if (!IsWithinAllowedSize(input))
                throw new GeneratorOutputException("Cannot encrypt more than 1,048,576 bytes");

            byte[] result = new byte[input.Length];

            using (cipher)
            using (var encryptor = cipher.CreateEncryptor())
            using (var memStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
            {
                // cryptoStream.Write(result, 0, 0);
                cryptoStream.Write(input, 0, input.Length);
                Array.Copy(input, result, input.Length);


                //after every request generate an extra 256 bits of pseudorandom data 
                //and use that as the new key for the block cipher. 
            }
            cipher.Dispose();

            return result;
        }

        private string GenerateBlocks(int numberOfBlocks)
        {
            if (!state.Seeded)
                throw new GeneratorSeedException("Generator not seeded!");

            return string.Empty;
        }

        private double GetApproximateSize(byte[] input)
        {
            double size;
            object o = new object();
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, o);
                size = s.Length;
            }

            return size;
        }

        private bool IsWithinAllowedSize(byte[] input)
        {
            double allowedSize = Math.Pow(2, 20);

            return GetApproximateSize(input) <= allowedSize;
        }

        public byte[] Reseed(byte[] key)
        {
            var algorithm = SHA256.Create();
            var hash = algorithm.ComputeHash(key);

            state.Seed = hash;
            state.Counter++;
            state.Seeded = true;

            return null;
        }
    }
}