using System;
using System.IO;
using System.Security.Cryptography;

namespace Tilko.API.Encryption
{
	internal class AES
	{
		#region Encrypt : AES 암호화
		/// <summary>
		/// AES 암호화
		/// </summary>
		/// <param name="Key">AES 키</param>
		/// <param name="Iv">AES IV</param>
		/// <param name="PlainText">암호화할 데이터</param>
		/// <returns></returns>
		public byte[] Encrypt(byte[] Key, byte[] Iv, byte[] PlainText)
		{
			byte[] _ret		= new byte[0];

			try
			{
				using (RijndaelManaged _aes = new RijndaelManaged())
				{
					_aes.Key				= Key;
					_aes.IV					= Iv;
					_aes.Mode				= CipherMode.CBC;
					_aes.Padding			= PaddingMode.PKCS7;

					using (ICryptoTransform _enc = _aes.CreateEncryptor(_aes.Key, _aes.IV))
					{
						using (MemoryStream _ms = new MemoryStream())
						{
							using (CryptoStream _cs = new CryptoStream(_ms, _enc, CryptoStreamMode.Write))
							{
								_cs.Write(PlainText, 0, PlainText.Length);
								_cs.FlushFinalBlock();
								_ret	= _ms.ToArray();
							}
						}
					}
					_aes.Clear();
				}
			}
			catch
			{
				throw;
			}

			return _ret;
		}
		#endregion

		#region Decrypt : AES 복호화
		/// <summary>
		/// AES 복호화
		/// </summary>
		/// <param name="Key"></param>
		/// <param name="CipherText"></param>
		/// <returns></returns>
		public byte[] Decrypt(byte[] Key, byte[] Iv, byte[] CipherText)
		{
			byte[] _ret		= new byte[0];

			try
			{
				using (RijndaelManaged _aes = new RijndaelManaged())
				{
					_aes.Key				= Key;
					_aes.IV					= Iv;
					_aes.Mode				= CipherMode.CBC;
					_aes.Padding			= PaddingMode.PKCS7;

					using (ICryptoTransform _enc = _aes.CreateDecryptor(_aes.Key, _aes.IV))
					{
						using (MemoryStream _ms = new MemoryStream(CipherText))
						{
							using (CryptoStream _cs = new CryptoStream(_ms, _enc, CryptoStreamMode.Read))
							{
								byte[] _ret2	= new byte[CipherText.Length];
								int _count		= _cs.Read(_ret2, 0, _ret2.Length);
								_ret			= new byte[_count];
								Array.Copy(_ret2, _ret, _count);
							}
						}
					}
					_aes.Clear();
				}
			}
			catch
			{
				throw;
			}

			return _ret;
		}
		#endregion
	}
}
