﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.CREDIT4U
{
	[TestClass]
	public class SMS본인인증서비스1단계
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 한국신용정보원의 SMS 본인인증 서비스 1단계 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/credit4u/checkedselfcaptcha");

				// Body 추가
				_rest.AddBody("UserID", "", true);                     // [암호화] 로그인 아이디(Base64 인코딩)
 				_rest.AddBody("UserPassword", "", true);               // [암호화] 로그인 비밀번호(Base64 인코딩)
				_rest.AddBody("UserName", "", true);                   // [암호화] 가입자 명(Base64 인코딩)
				_rest.AddBody("IdentityNumber", "", true);             // [암호화] 주민등록번호(8012151XXXXXX / Base64 인코딩)
				_rest.AddBody("NiceMobileCorp", "");                   // 통신사코드SKT : 0 / KT : 1 / LGT : 2 / SKM(알뜰폰) : 3 / KTM(알뜰폰) : 4 / LGM(알뜰폰) : 5

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
