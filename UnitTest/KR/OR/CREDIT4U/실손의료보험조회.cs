using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.CREDIT4U
{
	[TestClass]
	public class 실손의료보험조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 한국신용정보원의 실손의료보험 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/credit4u/silsondata");

				// Body 추가
				_rest.AddBody("UserID", "", true);                  // [암호화] 로그인 아이디(Base64 인코딩)
				_rest.AddBody("UserPassword", "", true);            // [암호화] 로그인 비밀번호(Base64 인코딩)

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
