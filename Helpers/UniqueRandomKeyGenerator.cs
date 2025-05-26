using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
	public class UniqueRandomKeyGenerator
	{
		private const string ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

		public static string GenerateUniqueRandomKey(int length, string pin)
		{
			StringBuilder stringBuilder = new StringBuilder();

			if (length <= 0)
				throw new ArgumentException("Length should be greater than zero.");

			int randomCharsLength = length > ValidChars.Length ? ValidChars.Length : length;

			stringBuilder.Append(GenerateRandomKey(randomCharsLength));

			// Subtracting the length of the random characters from the total length
			int remainingLength = length - randomCharsLength;

			// Adding PIN and timestamp
			if (!string.IsNullOrEmpty(pin) && remainingLength > 0)
			{
				string combinedString = pin + DateTime.Now.Ticks.ToString();
				int combinedLength = Math.Min(remainingLength, combinedString.Length);
				stringBuilder.Append(combinedString.Substring(0, combinedLength));
			}

			return stringBuilder.ToString();
		}

		private static string GenerateRandomKey(int length)
		{
			StringBuilder randomStringBuilder = new StringBuilder();

			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				byte[] uintBuffer = new byte[sizeof(uint)];

				while (length-- > 0)
				{
					rng.GetBytes(uintBuffer);
					uint num = BitConverter.ToUInt32(uintBuffer, 0);
					randomStringBuilder.Append(ValidChars[(int)(num % (uint)ValidChars.Length)]);
				}
			}

			return randomStringBuilder.ToString();
		}
	}
}
