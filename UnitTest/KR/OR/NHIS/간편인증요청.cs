
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NHIS
{
	[TestClass]
	public class 간편인증요청
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 국민건강보험공단의 간편인증 요청 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/nhissimpleauth/simpleauthrequest");

				// Body 추가
				_rest.AddBody("PrivateAuthType", "", false);               // 인증종류 0: 카카오톡 / 1: 페이코 / 2: 국민은행모바일 / 3: 삼성패스 / 4: Pass
				_rest.AddBody("UserName", "", true);                       // [암호화] 이용자명
				_rest.AddBody("BirthDate", "", true);                      // [암호화] 생년월일(yyyyMMdd)
				_rest.AddBody("UserCellphoneNumber", "", true);            // [암호화] 휴대폰번호

				// API 호출
				string _result = _rest.Call();
				if (_rest.HttpStatusCode != System.Net.HttpStatusCode.OK)
				{
					throw new Exception(_rest.Message);
				}
			}
			catch (Exception err)
			{
				Debug.WriteLine(err.Message);
			}
		}
	}
}
