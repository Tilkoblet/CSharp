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

				// 인터넷등기소의 등기물건 주소검색 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/iros/risuconfirmsimplec");

				// Body 추가
				_rest.AddBody("Address", "");             // 주소 (빠른 조회를 원할 시 정확한 주소 입력 필요)
				_rest.AddBody("Sangtae", "");             // 상태(공백 시 현행폐쇄) 현행:0/폐쇄:1/ 현행폐쇄:2
				_rest.AddBody("KindClsFlag", "");         // 부동산구분(공백 시 전체) 전체: 0 / 집합건물:1 / 건물:2 / 토지:3
				_rest.AddBody("Region", "");              // 도시(공백 시 전체) 전체:0/서울특별시:1/부산광역시:2/대구광역시:3/인천광역시:4/광주광역시:5/대전광역시:6/울산광역시:7/세종특별자치시:8/경기도:9/강원도:10/충청북도:11/충청남도:12/전라북도:13/전라남도:14/경상북도:15/경상남도:16/제주특별자치도:17
				
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
