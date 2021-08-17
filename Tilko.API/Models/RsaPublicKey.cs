using System;

namespace Tilko.API.Models
{
	/// <summary>
	/// RSA 공개키 모델
	/// </summary>
	internal class RsaPublicKey : BaseModel
	{
		/// <summary>
		/// API 키에 매칭되는 RSA 공개키
		/// </summary>
		public string PublicKey { get; set; }

		/// <summary>
		/// 전달한 API 키(검증 용)
		/// </summary>
		public string ApiKey { get; set; }
	}
}
