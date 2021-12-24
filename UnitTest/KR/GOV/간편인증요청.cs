
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.GOV
{
	[TestClass]
	public class 간편인증요청
	{
		[TestMethod]
		public void TILKO_API()
		{
			// API 상세설명 URL
			// https://tilko.net/Help/Api/POST-api-apiVersion-GovSimpleAuth-SimpleAuthRequest

			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 정부24의 간편인증 요청 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/GovSimpleAuth/SimpleAuthRequest");

				// Body 추가
				_rest.AddBody("PrivateAuthType", "4", false);	// 인증종류 0: 카카오톡 / 1: 페이코 / 2: 국민은행모바일 / 3: 삼성패스 / 4: Pass
				_rest.AddBody("UserName", "홍길동", true);                  // [암호화] 이용자명
				_rest.AddBody("BirthDate", "20210101", true);               // [암호화] 생년월일(yyyyMMdd)
				_rest.AddBody("UserCellphoneNumber", "01012345678", true);  // [암호화] 휴대폰번호

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
