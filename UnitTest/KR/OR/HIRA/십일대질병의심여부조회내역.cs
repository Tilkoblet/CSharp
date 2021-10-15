using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.HIRA
{
	[TestClass]
	public class 십일대질병의심여부조회내역
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 건강보험심사평가원의 11대 질병 의심 여부 조회내역 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/hira/suspecteddiseasesget");

				// Body 추가
				_rest.AddBody("MedicineCodeList", "");     // 내가먹는약 서비스의 약품코드(예 : {"medicine_code_list" : ["660700010","643503630","645903041","648104500","649801381"]})
			
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

