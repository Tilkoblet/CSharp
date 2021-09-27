using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 일용근로자고용정보현황조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 고용산재토탈서비스의 일용근로자 고용정보 현황 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/selectilyonggeunrojainfojohoe");

				// Body 추가
				_rest.AddBody("CertFile"      , File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile"       , File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword"  , Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "[조회를 원하는 사업자등록번호]", true);
				_rest.AddBody("UserGroupFlag" , "1");					// 0: 사업장, 1: 사무대행
				_rest.AddBody("IndividualFlag", "1");					// 0: 개인, 1: 법인
				_rest.AddBody("BoheomFg"      , "2");					// 0: 산재, 1: 고용, 2: 전체
				_rest.AddBody("GwanriNo"      , "[조회를 원하는 관리번호]");
				_rest.AddBody("DaesangYYFrom" , "2021");
				_rest.AddBody("DaesangYYTo"   , "2021");

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
