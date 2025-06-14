﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers
{
    public static class MasterKeyGenerator
    {
        public static string Generate()
        {
            using (var cryptoProvider = new RSACryptoServiceProvider())
            {
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Guid.NewGuid() + DateTime.Now.ToString());
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }
    }
}
