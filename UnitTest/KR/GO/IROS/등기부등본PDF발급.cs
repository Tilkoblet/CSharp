using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace UnitTest.KR.GO.IROS
{
	[TestClass]
	public class 등기부등본PDF발급
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 인터넷등기소의 등기부등본 PDF 발급 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/iros/getpdffile");

				// Body 추가
				_rest.AddBody("TransactionKey", "");        // 등본발급 시 리턴받은 트랜잭션 키 (GUID)
				_rest.AddBody("IsSummary", "");             // 요약 데이터 포함여부(Y/N 공백 또는 다른 문자열일 경우 기본값 Y)

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
