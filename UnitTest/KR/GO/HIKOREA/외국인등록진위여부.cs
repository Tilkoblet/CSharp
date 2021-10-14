using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.GO.HIKOREA

{
	[TestClass]
	public class 외국인등록진위여부
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// Hi, Korea의 외국인등록진위여부 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "api/v1.0/hikorea/infofrnregidchkrsltr/kr");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword , true);
				_rest.AddBody("IdentityNumber", "", true);                      // [암호화] 조회하는 사람의 주민등록번호(8012151XXXXXX / Base64 인코딩)
                _rest.AddBody("TargetIdentityNumber", "", true);                // [암호화] 조회 대상의 외국인등록번호(9012156XXXXXX / Base64 인코딩)
				_rest.AddBody("TargetPublishDate", "");                         // 조회 대상의 외국인등록증 발급일자(yyyyMMdd)


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
