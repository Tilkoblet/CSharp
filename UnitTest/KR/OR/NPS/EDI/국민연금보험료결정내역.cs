using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NPS.EDI

{
	[TestClass]
	public class 국민연금보험료결정내역
	{

		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 국민연금 EDI의 국민연금보험료 결정내역 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/npsedi/u040201m01");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "", true);                    // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("StartDate	", "", false);                    // 검색 시작일자(YYYYMMDD)
				_rest.AddBody("EndDate", "", false);                          // 검색 종료일자(YYYYMMDD)
				_rest.AddBody("WediUsrId", "", false);                        // 위임사업장 선택 조회 후 받은 WediUsrId 값

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
