using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.GO.EFINE

{
	[TestClass]
	public class 운전면허진위여부
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 경찰청교통민원24의 운전면허진위여부 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/efine/licentruth");

				// Body 추가
				_rest.AddBody("BirthDate", "");           // 대상자 생년월일(yyyyMMdd)
				_rest.AddBody("Name", "");                // 대상자 성명
				_rest.AddBody("LicNumber", "", true);     // [암호화] 운전면허번호(예: 서울 - XX - XXXXXX - XX / Base64 인코딩
				_rest.AddBody("SpecialNumber", "", true); // [암호화] 식별번호(면허증 우측 하단 작은 사진 밑에 있는 일련번호 / Base64 인코딩)

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
