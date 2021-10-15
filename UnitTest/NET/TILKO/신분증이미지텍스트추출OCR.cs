using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.NET.TILKO
{
	[TestClass]
	public class 신분증이미지텍스트추출OCR
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 틸코닷넷의 신분증이미지 텍스트 추출(OCR) endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/tilko/ocr/license");

				// Body 추가
				_rest.AddBody("Base64", "", true);     // [암호화] 신분증 이미지 Base64
				_rest.AddBody("LicenseType", "");      // 신분증 구분 주민등록증 : 0 / 운전면허증 : 1 / 외국인등록증 : 2

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
