using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.HOMETAX
{
	[TestClass]
	public class 법인세신고서조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 홈택스의 법인세 신고서 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "api/v1.0/hometaxidlogin/uternaaz110/beobinse/singo");

				// Body 추가
				_rest.AddBody("UserId", "", true);                    // [암호화] 홈택스 ID(Base64 인코딩)
				_rest.AddBody("UserPassword", "", true);              // [암호화] 홈택스 암호(Base64 인코딩)
				_rest.AddBody("BusinessNumber", "", true);            // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩) 공백일 검색기간은 30일, 아닐경우 검색기간은 365일


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
