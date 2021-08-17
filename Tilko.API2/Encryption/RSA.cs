using System;
using System.IO;
using System.Security.Cryptography;

namespace Tilko.API.Encryption
{
	/// <summary>
	/// Reference
	/// https://gist.github.com/chenrui1988/6b104a010172786dbcbc0aafc466d291#file-rsakeyutils-cs
	/// </summary>
	internal class RSA
	{
		#region DecodePublicKey : RSA 공개키를 RSACryptoServiceProvider로 반환
		/// <summary>
		/// RSA 및 X.509 인증서 형식의 공개키를 RSACryptoServiceProvider로 반환
		/// </summary>
		/// <param name="PublicKeyBytes"></param>
		/// <returns></returns>
		public static RSACryptoServiceProvider DecodePublicKey(byte[] PublicKeyBytes)
		{
			try
			{
				using (MemoryStream _ms = new MemoryStream(PublicKeyBytes))
				{
					using (BinaryReader _br = new BinaryReader(_ms))
					{
						byte[] _seqOID	= { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
						byte[] _seq		= new byte[15];
						byte _byteValue;
						ushort _shortValue;

						_shortValue		= _br.ReadUInt16();

						switch (_shortValue)
						{
							case 0x8130:
							{
								_br.ReadByte();
								break;
							}
							case 0x8230:
							{
								_br.ReadInt16();
								break;
							}
							default:
								return null;
						}

						_seq			= _br.ReadBytes(15);
						_shortValue		= _br.ReadUInt16();
						if (_shortValue == 0x8103)
						{
							_br.ReadByte();
						}
						else if (_shortValue == 0x8203)
						{
							_br.ReadInt16();
						}
						else
						{
							throw new Exception("Please confirm the RSA public key data!");
						}

						_byteValue		= _br.ReadByte();
						if (_byteValue != 0x00)
						{
							throw new Exception("Please confirm the RSA public key data!");
						}

						_shortValue		= _br.ReadUInt16();
						if (_shortValue == 0x8130)
						{
							_br.ReadByte();
						}
						else if (_shortValue == 0x8230)
						{
							_br.ReadInt16();
						}
						else
						{
							throw new Exception("Please confirm the RSA public key data!");
						}

						CspParameters _params			= new CspParameters();
						_params.Flags					= CspProviderFlags.NoFlags;
						_params.KeyContainerName		= Guid.NewGuid().ToString().ToUpperInvariant();
						_params.ProviderType			= ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1))) ? 0x18 : 1;

						RSACryptoServiceProvider _csp	= new RSACryptoServiceProvider(_params);
						RSAParameters _rsasParams		= new RSAParameters();
						_rsasParams.Modulus				= _br.ReadBytes(Helpers.DecodeIntegerSize(_br));
						RSAParameterTraits _traits		= new RSAParameterTraits(_rsasParams.Modulus.Length * 8);
						_rsasParams.Modulus				= Helpers.AlignBytes(_rsasParams.Modulus, _traits.size_Mod);
						_rsasParams.Exponent			= Helpers.AlignBytes(_br.ReadBytes(Helpers.DecodeIntegerSize(_br)), _traits.size_Exp);

						_csp.ImportParameters(_rsasParams);
						_br.Close();
						_ms.Close();
						return _csp;
					}
				}
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region class Helpers
		/// <summary>
		/// Helpers
		/// </summary>
		class Helpers
		{
			public static bool CompareBytearrays(byte[] a, byte[] b)
			{
				if (a.Length != b.Length)
					return false;
				int i = 0;
				foreach (byte c in a)
				{
					if (c != b[i])
						return false;
					i++;
				}
				return true;
			}
			public static byte[] AlignBytes(byte[] inputBytes, int alignSize)
			{
				int inputBytesSize = inputBytes.Length;

				if ((alignSize != -1) && (inputBytesSize < alignSize))
				{
					byte[] buf = new byte[alignSize];
					for (int i = 0; i < inputBytesSize; ++i)
					{
						buf[i + (alignSize - inputBytesSize)] = inputBytes[i];
					}
					return buf;
				}
				else
				{
					return inputBytes;
				}
			}

			public static int DecodeIntegerSize(System.IO.BinaryReader rd)
			{
				byte byteValue;
				int count;

				byteValue = rd.ReadByte();
				if (byteValue != 0x02) return 0;

				byteValue = rd.ReadByte();
				if (byteValue == 0x81)
				{
					count = rd.ReadByte();
				}
				else if (byteValue == 0x82)
				{
					byte hi = rd.ReadByte(); byte lo = rd.ReadByte();
					count = BitConverter.ToUInt16(new[] { lo, hi }, 0);
				}
				else
				{
					count = byteValue;
				}

				while (rd.ReadByte() == 0x00)
				{
					count -= 1;
				}
				rd.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);

				return count;
			}
		}
		#endregion

		#region class RSAParameterTraits
		/// <summary>
		/// RSAParameterTraits
		/// </summary>
		class RSAParameterTraits
		{
			public RSAParameterTraits(int modulusLengthInBits)
			{
				int assumedLength = -1;
				double logbase = Math.Log(modulusLengthInBits, 2);
				if (logbase == (int)logbase)
				{
					assumedLength = modulusLengthInBits;
				}
				else
				{
					assumedLength = (int)(logbase + 1.0);
					assumedLength = (int)(Math.Pow(2, assumedLength));
					System.Diagnostics.Debug.Assert(false);
				}

				switch (assumedLength)
				{
					case 512:
						this.size_Mod = 0x40;
						this.size_Exp = -1;
						this.size_D = 0x40;
						this.size_P = 0x20;
						this.size_Q = 0x20;
						this.size_DP = 0x20;
						this.size_DQ = 0x20;
						this.size_InvQ = 0x20;
						break;
					case 1024:
						this.size_Mod = 0x80;
						this.size_Exp = -1;
						this.size_D = 0x80;
						this.size_P = 0x40;
						this.size_Q = 0x40;
						this.size_DP = 0x40;
						this.size_DQ = 0x40;
						this.size_InvQ = 0x40;
						break;
					case 2048:
						this.size_Mod = 0x100;
						this.size_Exp = -1;
						this.size_D = 0x100;
						this.size_P = 0x80;
						this.size_Q = 0x80;
						this.size_DP = 0x80;
						this.size_DQ = 0x80;
						this.size_InvQ = 0x80;
						break;
					case 4096:
						this.size_Mod = 0x200;
						this.size_Exp = -1;
						this.size_D = 0x200;
						this.size_P = 0x100;
						this.size_Q = 0x100;
						this.size_DP = 0x100;
						this.size_DQ = 0x100;
						this.size_InvQ = 0x100;
						break;
					default:
						System.Diagnostics.Debug.Assert(false); break;
				}
			}

			public int size_Mod = -1;
			public int size_Exp = -1;
			public int size_D = -1;
			public int size_P = -1;
			public int size_Q = -1;
			public int size_DP = -1;
			public int size_DQ = -1;
			public int size_InvQ = -1;
		}
		#endregion
	}
}
