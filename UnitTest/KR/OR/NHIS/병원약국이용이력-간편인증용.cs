using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NHIS
{
	[TestClass]
	public class 병원약국이용이력간편인증용
	{

		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 국민건강보험공단의 병원/약국 이용 이력 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/nhissimpleauth/retrievecaredesclist");

				// Body 추가
				_rest.AddBody("IdentityNumber", "", true);                // [암호화]유저 주민등록번호 앞자리(yyMMdd / Base64 인코딩)
				_rest.AddBody("StartDate", "", false);                    // 검색시작일(yyyyMMdd) 오늘부터 14개월 전부터 조회 가능
				_rest.AddBody("EndDate", "", false);                      // 검색종료일(yyyyMMdd) 오늘부터 2개월 전까지 조회 가능
				_rest.AddBody("CxId", "", false);                         // CxId
				_rest.AddBody("PrivateAuthType", "", false);              // 인증종류 KakaoTalk / Payco / KbMobile / SamsungPass / TelecomPass
				_rest.AddBody("ReqTxId", "", false);                      // ReqTxId
				_rest.AddBody("Token", "", false);                        // Token
				_rest.AddBody("TxId	", "", false);                        // TxId
				_rest.AddBody("UserName", "", true);                      // [암호화] 이용자명
				_rest.AddBody("BirthDate", "", true);                     // [암호화] 생년월일(yyyyMMdd)
				_rest.AddBody("UserCellphoneNumber", "", true);           // [암호화] 휴대폰번호

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
