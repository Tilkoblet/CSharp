using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 일자리지원금조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 고용산재토탈서비스의 일자리 지원금 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/selectiljarijiwonprcjigeupnaeyeok");

				// Body 추가
				_rest.AddBody("CertFile"      , File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile"       , File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword"  , Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "", true);                  // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("UserGroupFlag" , "1");                       // 인증서 - 0: 사업장, 1: 사무대행
				_rest.AddBody("IndividualFlag", "1");                       // 인증서 - 0: 개인, 1: 법인
				_rest.AddBody("JiwonYear"     , "2021");					// 지원년도(yyyy)
				_rest.AddBody("GwanriNo"      , "");                        // 조회를 원하는 관리번호
				_rest.AddBody("Jigeup_YYMM"   , "");						// 지급년월(yyyyMM)

				// API 호출  
				string _result				= _rest.Call();
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
