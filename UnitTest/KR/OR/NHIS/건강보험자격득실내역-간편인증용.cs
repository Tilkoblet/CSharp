using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.NHIS
{
	[TestClass]
	public class 건강보험자격득실내역간편인증용
	{

		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 국민건강보험공단의 건강보험자격득실내역 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/nhissimpleauth/jpaea00401");

				// Body 추가
				_rest.AddBody("NhisQuery", "", false);                // 검색조건 전체 : 0 / 직장가입자 : 1 / 지역가입자 : 2 / 가입자 전체 : 3
				_rest.AddBody("CxId", "", false);                     // CxId
				_rest.AddBody("PrivateAuthType", "", false);          // 인증종류 KakaoTalk / Payco / KbMobile / SamsungPass / TelecomPass
				_rest.AddBody("ReqTxId", "", false);                  // ReqTxId
				_rest.AddBody("Token", "", false);                    // Token
				_rest.AddBody("TxId", "", false);                     // TxId
				_rest.AddBody("UserName", "", true);                  // [암호화] 이용자명
				_rest.AddBody("BirthDate", "", true);                 // [암호화] 생년월일(yyyyMMdd)
				_rest.AddBody("UserCellphoneNumber", "", true);       // [암호화] 휴대폰번호

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
