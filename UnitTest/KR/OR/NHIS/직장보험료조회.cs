using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NHIS
{
    [TestClass]
    public class 직장보험료조회
    {
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 건강보험공단의 직장보험료조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/nhis/jpzaa00110");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("Year", "2020");

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
