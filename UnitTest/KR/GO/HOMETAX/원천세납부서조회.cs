﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.HOMETAX

{
	[TestClass]
	public class 원천세납부서조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 홈택스 의 원천세 납부서 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "api/v1.0/hometaxgeneral/uternaaz111/woncheonse/napbu");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "", true);                          // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩) 공백일 검색기간은 30일, 아닐경우 검색기간은 365일", true);
				_rest.AddBody("StartDate", "");                                     // 검색시작일(yyyyMMdd) 공백일 경우 기본값을 API에서 셋팅
				_rest.AddBody("EndDate", "");                                       // 검색종료일(yyyyMMdd) 공백일 경우 기본값을 API에서 셋팅
			
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
