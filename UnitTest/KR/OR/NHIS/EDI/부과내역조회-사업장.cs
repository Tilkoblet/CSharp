using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NHIS.EDI

{
	[TestClass]
	public class 부과내역조회사업장
	{

		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 국민건강보험공단 EDI의 부과내역조회(사업장) endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/nhisedi/bmbb_311");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("IdentityNumber", "", true);                    // [암호화] 검색 할 위임 사업자등록번호(xxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("FirmSym", "", false);                          // 사업장기호
				_rest.AddBody("UnitFirmSym", "", false);                      // 단위사업장기호
				_rest.AddBody("FirmName", "", false);                         // 사업장명
				_rest.AddBody("FirmMgmtNo", "", false);                       // 사업장관리번호
				_rest.AddBody("WrtChasu", "", false);                         // 작성일차수
				_rest.AddBody("WrtDupSeq", "", false);                        // 작성일차수Seq
				_rest.AddBody("GojiYyyymm", "", false);                       // 고지년월
				_rest.AddBody("GojiChasu", "", false);                        // 고지차수
				_rest.AddBody("UnbNo", "", false);                            // UnbNo

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
