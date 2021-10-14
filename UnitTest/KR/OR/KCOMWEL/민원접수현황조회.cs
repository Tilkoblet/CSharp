using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 민원접수현황조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 고용산재토탈의 민원접수현황 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/retrievejingsuminwon");

				// Body 추가
				_rest.AddBody("CertFile"      , File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile"       , File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword"  , Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "", true);          // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("UserGroupFlag", "1");				// 인증서 - 사업장(0)/사무대행(1) 구분
				_rest.AddBody("IndividualFlag", "1");               // 인증서 - 개인(0)/법인(1) 구분
				_rest.AddBody("JeopsuDtFrom", "");					// 접수일(시작 - yyyyMMdd)
				_rest.AddBody("JeopsuDtTo", "");					// 접수일(종료 - yyyyMMdd)
				_rest.AddBody("JingsuBosangFg", "");				// 민원접수 종류 - 징수(0)/사대공통서식(1)/일자리(2)

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
