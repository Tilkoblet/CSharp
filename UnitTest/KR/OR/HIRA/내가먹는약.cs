using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.HIRA
{
	[TestClass]
	public class 내가먹는약
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 건강보험심사평가원의 내가 먹는 약 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/hira/hiraa050300000100");
			
				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("IdentityNumber", "", true);         // [암호화] 주민등록번호(8012151234567 / Base64 인코딩)
				_rest.AddBody("TelecomCompany", "");               // 통신사 SKT : 0 / KT : 1 / LGT : 2 / SKT알뜰폰 : 3 / KT알뜰폰 : 4 / LGT알뜰폰 : 5 / NA : 6
				_rest.AddBody("CellphoneNumber", "", true);        // [암호화] 연락처(01012345678 / Base64 인코딩)

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

