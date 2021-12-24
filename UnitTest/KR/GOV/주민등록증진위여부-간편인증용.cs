using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.GOV
{
	[TestClass]
	public class 주민등록증진위여부간편인증용
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				// API 상세설명 URL
				// https://tilko.net/Help/Api/POST-api-apiVersion-GovSimpleAuth-AA090UserJuminCheckResApp

				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 정부24의 주민등록증진위여부 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/GovSimpleAuth/AA090UserJuminCheckResApp");

				// Body 추가
				_rest.AddBody("CxId", "", false);				// 간편인증 요청 후 받은 CxId 값
				_rest.AddBody("PrivateAuthType", "", false);	// 간편인증 요청 후 받은 PrivateAuthType 값
				_rest.AddBody("ReqTxId", "", false);			// 간편인증 요청 후 받은 ReqTxId 값
				_rest.AddBody("Token", "", false);				// 간편인증 요청 후 받은 Token 값
				_rest.AddBody("TxId", "", false);				// 간편인증 요청 후 받은 TxId 값
				_rest.AddBody("UserName", "", true);			// [암호화] 간편인증 요청 후 받은 UserName 값
				_rest.AddBody("BirthDate", "", true);			// [암호화] 간편인증 요청 후 받은 BirthDate 값
				_rest.AddBody("UserCellphoneNumber", "", true);	// [암호화] 간편인증 요청 후 받은 UserCellphoneNumber 값
				_rest.AddBody("PersonName", "", true);			// [암호화] 조회 대상자의 성명
				_rest.AddBody("IdentityNumber", "", true);		// [암호화] 조회 대상자의 주민등록번호(9001011234567)
				_rest.AddBody("PublishDate", "", false);		// 주민등록증 발행일(yyyyMMdd)

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
