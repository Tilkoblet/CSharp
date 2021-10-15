using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.CREDIT4U
{
	[TestClass]
	public class 인증코드발송
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 한국신용정보원의 인증코드 발송 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/credit4u/sendauthcode");

				// Body 추가
				_rest.AddBody("UserEmail", "", true);                  // [암호화] 사이트 가입용 이메일(예: 이메일@도메인 / Base64 인코딩)
				
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
