using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.CREDIT4U
{
	[TestClass]
	public class 사이트가입
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 한국신용정보원의 사이트 가입 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/credit4u/joinsite");

				// Body 추가
				_rest.AddBody("EmailCode", "", true);           // [암호화] 이메일 인증코드(Base64 인코딩)
				_rest.AddBody("UserEmail", "", true);           // [암호화] 사이트 가입용 이메일(예: 이메일@도메인 / Base64 인코딩)
				_rest.AddBody("UserID", "", true);              // [암호화] 로그인 아이디(Base64 인코딩)
				_rest.AddBody("UserPassword", "", true);        // [암호화] 로그인 비밀번호(Base64 인코딩)      
				_rest.AddBody("UserName", "", true);            // [암호화] 가입자 명(Base64 인코딩)           
				_rest.AddBody("DateOfBirth", "");               //생년월일(yyyyMMdd)
				_rest.AddBody("Sex", "");                       // 성별(남자 : m / 여자 : f)

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
