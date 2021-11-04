using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NPS.EDI

{
	[TestClass]
	public class 부과내역조회
	{

		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 국민연금 EDI의 부과내역 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/npsedi/u040205m01");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "", true);                    // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("DocNo", "", false);                            // 문서번호(국민연금보험료 결정내역 조회 후 받은 DocNo 값)
				_rest.AddBody("ConfirmDt", "", false);                        // 해당년월(국민연금보험료 결정내역 조회 후 받은 ConfirmDt 값)
				_rest.AddBody("FmCd", "", false);                             // 국민연금보험료 결정내역 조회 후 받은 FmCd 값
				_rest.AddBody("RgstChrgpId", "", false);                      // 사업장관리번호(국민연금보험료 결정내역 조회 후 받은 RgstChrgpId 값)

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
