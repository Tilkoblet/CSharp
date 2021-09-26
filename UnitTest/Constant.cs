using System;

namespace UnitTest
{
	public class Constant
	{
		/// <summary>
		/// API 키
		/// API 키는 틸코 API 홈페이지에서 발급 받으세요.
		/// https://tilko.net
		/// </summary>
		public static string ApiKey { get { return "[API 키]"; } }
		
		/// <summary>
		/// 공동인증서 경로 설정
		/// 공동인증서는 "C:\Users\[사용자계정]\AppData\LocalLow\NPKI\yessign\USER\[인증서DN명]"에 존재합니다.
		/// </summary>
		public static string CertPath { get { return @"C:\Users\[사용자계정]\AppData\LocalLow\NPKI\yessign\USER\[인증서DN명]"; } }

		/// <summary>
		/// 공동인증서 비밀번호
		/// </summary>
		public static string CertPassword { get { return "[인증서 비밀번호]"; } }
	}
}
