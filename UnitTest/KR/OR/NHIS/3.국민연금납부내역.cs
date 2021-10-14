using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NHIS
{
	[TestClass]
	public class 국민연금납부내역
	{ 
	 
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 건강보험공단의 국민연금납부내역 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + " api/v1.0/nhis/jpaca00101/gugminyeongeum");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("Year", "" );               // 검색년도(yyyy)
				_rest.AddBody("StartMonth", "");          // 검색 시작 월(MM)
				_rest.AddBody("EndMonth", "");

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
