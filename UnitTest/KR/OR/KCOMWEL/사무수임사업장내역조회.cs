using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 사무수임사업장내역조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 고용산재토탈서비스의 사무수임사업장 내역 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/selectsdgsaeopjanginfonaeyeok");

				// Body 추가
				_rest.AddBody("CertFile"           , File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile"            , File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword"       , Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber"     , "[조회를 원하는 사업자등록번호]", true);
				_rest.AddBody("UserGroupFlag"      , "1");					// 0: 사업장, 1: 사무대행
				_rest.AddBody("IndividualFlag"     , "1");					// 0: 개인, 1: 법인
				_rest.AddBody("BoheomFg"           , "2");					// 0: 산재, 1: 고용, 2: 전체
				_rest.AddBody("BugwaGojiYn"        , "2");					// 0: 부과고지, 1: 자진신고, 2: 전체
				_rest.AddBody("SjSaeopFg"          , "0");					// 0: 계속, 1: 유기, 2: 일괄계속, 3: 일괄유기, 4: 해외사업장, 5: 중소기업사업주, 6: 자영업자, 7: 전체
				_rest.AddBody("GySaeopFg"          , "0");					// 0: 계속, 1: 유기, 2: 일괄계속, 3: 일괄유기, 4: 해외사업장, 5: 중소기업사업주, 6: 자영업자, 7: 전체
				_rest.AddBody("GySaeopjangStatusCd", "0");					// 0: 정상, 1: 소멸, 2: 해지, 3: 전체
				_rest.AddBody("SjSaeopjangStatusCd", "0");					// 0: 정상, 1: 소멸, 2: 해지, 3: 전체
				_rest.AddBody("GwanriNo"           , "");
				_rest.AddBody("GwanriJisaCd"       , "0");					// enum.txt 파일에서 [GwanriJisaCd(관리지사)] 참고
				_rest.AddBody("JeopsuInfoJoheoYN"  , "N");					// Y/N
				_rest.AddBody("JeopsuDtFrom"       , "");					// yyyyMMdd
				_rest.AddBody("JeopsuDtTo"         , "");					// yyyyMMdd

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
