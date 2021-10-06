using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.GO.IROS
{
	[TestClass]
	public class 등기신청사건처리현황조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 인터넷등기소의 등기신청사건처리현황조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/iros/revtwelcomeevtc");

				// Body 추가
				_rest.AddBody("IrosID", "", true); // [암호화] iros.go.kr 로그인 ID(Base64 인코딩)
				_rest.AddBody("IrosPwd", "", true); // [암호화] iros.go.kr 로그인 패스워드(Base64 인코딩)
				_rest.AddBody("UniqueNo", ""); // 부동산 고유번호('-'을 제외한 14자리)
				_rest.AddBody("InsRealClsCd", ""); // 구분(공백시 건물) 토지 : 0 / 건물 : 1 / 집합건물 : 2

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
