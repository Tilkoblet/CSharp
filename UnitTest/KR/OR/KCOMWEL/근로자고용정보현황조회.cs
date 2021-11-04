using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 근로자고용정보현황조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 고용산재토탈의 근로자 고용정보 현황 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/selectgeunrojagyiryeok");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "2248167722", true);           // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("UserGroupFlag", "1");                  // 인증서 - 사업장(0)/사무대행(1) 구분
				_rest.AddBody("IndividualFlag", "1");                 // 인증서 - 개인(0)/법인(1) 구분           
				_rest.AddBody("GwanriNo", "49379001870");                       // 관리번호
				_rest.AddBody("BoheomFg", "2");                       // 보험구분 - 산재(0)/고용(1)/전체(2)
				_rest.AddBody("GyStatusCd", "3");                     // 고용상태 - 고용(0)/고용종료(1)/고용휴직(2)/전체(3)
				_rest.AddBody("GeunrojaNm", "");                     // 근로자 성명
				_rest.AddBody("GeunrojaRgNo", "");                   // 근로자 주민등록번호(13자리)
				

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
