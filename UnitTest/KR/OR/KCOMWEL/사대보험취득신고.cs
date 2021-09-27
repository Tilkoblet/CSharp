using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 사대보험취득신고
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 고용산재토탈서비스의 사무수임사업장 내역 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/samuchwideuksingo");

				// Body 추가
				_rest.AddBody("CertFile"         , File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile"          , File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword"     , Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber"   , "[조회를 원하는 사업자등록번호]", true);
				_rest.AddBody("UserGroupFlag"    , "1");					// 0: 사업장, 1: 사무대행
				_rest.AddBody("IndividualFlag"   , "1");					// 0: 개인, 1: 법인
				_rest.AddBody("GwanriNo"         , "[조회를 원하는 관리번호]");
				_rest.AddBody("GeunrojaRgNo"     , "[근로자 주민등록번호 13자리]");
				_rest.AddBody("GeunrojaNm"       , "[근로자 성명]");
				_rest.AddBody("JikJongCd"        , "0");					// enum.txt 파일에서 [JikJongCd(직종부호코드)] 참고
				_rest.AddBody("ChwideukMMNapbuYN", "[취득월납부여부(Y/N)]");
				_rest.AddBody("GyeYakJikYN"      , "[계약직여부(Y/N)]");
				_rest.AddBody("ChwideukDt"       , "[취득일자(yyyyMMdd)]");
				_rest.AddBody("JuSojeongGeunroTm", "[1주소정근로시간]");
				_rest.AddBody("MmAvgBosu"        , "[월평균보수(원)]");
				_rest.AddBody("NPSChwideukBuho"  , "0");					// 0: 십팔세이상당연취득, 1: 십팔세미만취득, 2: 전입사업장통폐합, 3: 대학강사, 4: 육십시간미만신청취득
				_rest.AddBody("NHICChwideukBuho" , "0");					// 0: 최초취득, 1: 의료급여수급권자해제, 2: 직장가입자변경, 3: 직장피부양자상실, 4: 지역가입자에서변경, 5: 국가유공자상실, 6: 기타, 7: 직권말소후재등록, 8: 직장가입자이중가입
				_rest.AddBody("GYBYN"            , "[고용보험 여부(Y/N)]");
				_rest.AddBody("NHICYN"           , "[건강보험 여부(Y/N)]");
				_rest.AddBody("NPSYN"            , "[국민연금 여부(Y/N)]");
				_rest.AddBody("SJBYN"            , "[산재보험 여부(Y/N)]");
				_rest.AddBody("ILJARIYN"         , "[일자리신청여부(Y/N)]");
				_rest.AddBody("GyeyakToDt"       , "[계약종료년월(YYYYMM)]");
				_rest.AddBody("GukJeok"          , "1");					// enum.txt 파일에서 [GukJeok(국적)] 참고

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
