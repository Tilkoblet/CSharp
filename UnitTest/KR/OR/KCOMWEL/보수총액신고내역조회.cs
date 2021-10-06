﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 보수총액신고내역조회
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 고용산재토탈서비스의 보수총액 신고내역 조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/selectbosujeopsulist");

				// Body 추가
				_rest.AddBody("CertFile"      , File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile"       , File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword"  , Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "[조회를 원하는 사업자등록번호]", true);
				_rest.AddBody("UserGroupFlag" , "0");					// 0: 사업장, 1: 사무대행
				_rest.AddBody("IndividualFlag", "1");					// 0: 개인, 1: 법인
				_rest.AddBody("BoheomYear"    , "2020");
				_rest.AddBody("GwanriNo"      , "");

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
