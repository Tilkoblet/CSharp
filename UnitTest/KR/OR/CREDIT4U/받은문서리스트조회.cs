using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.CREDIT4UI

{
	[TestClass]
	public class 받은문서리스트조회
	{

		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 국민건강보험공단 EDI의 받은문서 리스트 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/nhisedi/retrievedoclist");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("IdentityNumber", "", true);                    // [암호화] 검색 할 위임 사업자등록번호(xxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("FirmSym", "", false);                          // 사업장기호
				_rest.AddBody("UnitFirmSym", "", false);                      //단위사업장기호
				_rest.AddBody("FirmName", "", false);                         // 사업장명
				_rest.AddBody("FirmMgmtNo", "", false);                       // 사업장관리번호
				_rest.AddBody("FromDt", "", false);                           // 조회기간(시작)
				_rest.AddBody("ToDt", "", false);                             // 조회기간(종료)

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
