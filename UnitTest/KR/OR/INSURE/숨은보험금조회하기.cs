using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.INSURE
{
	[TestClass]
	public class 숨은보험금조회하기
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 생명보험협회의 숨은 보험금 조회하기 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/insure/resultihidnumdetail");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", "", true);            //[암호화] 인증서 암호(Base64 인코딩)
				_rest.AddBody("IdentityNumber", "", true);          // [암호화] 주민등록번호(8012151XXXXXX / Base64 인코딩)
				_rest.AddBody("CellphoneNumber", "", true);         // [암호화] 연락처(010-XXXX-XXXX / Base64 인코딩)

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
