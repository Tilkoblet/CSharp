using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NHIS
{
	[TestClass]
	public class 병원약국이용이력
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 건강보험공단의 병원약국이용이력 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/nhis/retrievecaredesclist");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true); // [암호화] 인증서 암호(Base64 인코딩)
				_rest.AddBody("IdentityNumber", "", true);                  // [암호화]유저 주민등록번호 앞자리(yyMMdd / Base64 인코딩)
				_rest.AddBody("StartDate", "");                             // 검색시작일(yyyyMMdd) 오늘부터 14개월 전부터 조회 가능
				_rest.AddBody("EndDate", "");                               // 검색종료일(yyyyMMdd) 오늘부터 2개월 전까지 조회 가능

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
