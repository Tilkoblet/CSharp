using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 민원접수현황선택출력
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 고용산재토탈서비스의 민원접수현황 선택 출력 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/printpdfjingsuminwon");

				// Body 추가
				_rest.AddBody("CertFile"      , File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile"       , File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword"  , Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "[조회를 원하는 사업자등록번호]", true);
				_rest.AddBody("UserGroupFlag" , "1");					// 0: 사업장, 1: 사무대행
				_rest.AddBody("IndividualFlag", "1");					// 0: 개인, 1: 법인
				_rest.AddBody("JEOPSU_NO"     , "[출력할 민원의 JEOPSU_NO]");
				_rest.AddBody("MINWON_DOC_CD" , "[출력할 민원의 MINWON_DOC_CD]");
				_rest.AddBody("IMSI_JEOPSU_NO", "[출력할 민원의 IMSI_JEOPSU_NO]");
				_rest.AddBody("PUBAP_ACPT_NO" , "[출력할 민원의 PUBAP_ACPT_NO]");

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
