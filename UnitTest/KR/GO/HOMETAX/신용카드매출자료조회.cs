﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.GO.HOMETAX
{
	[TestClass]
	public class 신용카드매출자료조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 홈택스의 신용카드 매출자료 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/hometaxagent/utesfabg25/suimnabseja/sinyongcard/maechuljalyo");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("AgentId", "", true);          // [암호화] 세무대리인 ID(세무대리 관리번호가 있는 경우 / Base64 인코딩)
				_rest.AddBody("AgentPassword", "", true);    // [암호화] 세무대리인 암호(세무대리 관리번호가 있는 경우 / Base64 인코딩)
				_rest.AddBody("BusinessNumber","", true);    // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("Year", "");                   // 검색년도(yyyy) 공백일 경우 검색 기준 해
				_rest.AddBody("StartQuarter", "");           // 검색시작분기(1분기 : 1 / 2분기 : 2 / 3분기 : 3 / 4분기 : 4) 공백일 경우 검색 기준 분기
				_rest.AddBody("EndQuarter", "");             // 검색종료분기(1분기 : 1 / 2분기 : 2 / 3분기 : 3 / 4분기 : 4) 공백일 경우 검색 기준 분기
				
				

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
