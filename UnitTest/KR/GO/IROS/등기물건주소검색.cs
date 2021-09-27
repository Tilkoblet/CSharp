using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace UnitTest.KR.GO.IROS
{
	[TestClass]
	public class 등기물건주소검색
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 인터넷등기소의 등기물건주소검색 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/iros/risuconfirmsimplec");

				// Body 추가
				_rest.AddBody("Address", "서울특별시 중구 다동길 5");
				_rest.AddBody("Sangtae", "2");
				_rest.AddBody("KindClsFlag", "0");
				_rest.AddBody("Region", "0");

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
