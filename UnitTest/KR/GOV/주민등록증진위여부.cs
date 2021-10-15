using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.GOV
{
	[TestClass]
	public class 주민등록증진위여부
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 정부24의 주민등록증진위여부 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/gov/aa090userjumincheckresapp");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("PersonName", "");                      // 조회 대상의 이름
				_rest.AddBody("IdentityNumber", "", true);            // [암호화] 조회 대상의 주민등록번호(8012151234567 / Base64 인코딩)
				_rest.AddBody("PublishDate", "");                     // 신분증 발행일(주민등록증 하단 발급기관 위의 발행날짜 / yyyyMMdd)
			
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
