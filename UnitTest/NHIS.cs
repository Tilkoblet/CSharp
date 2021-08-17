using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest
{
	/// <summary>
	/// 건강보험공단 테스트
	/// </summary>
	[TestClass]
	public class NHIS
	{
		string _apiKey			= "API_KEY";

		[TestMethod]
		public void 건강검진결과()
		{
			try
			{
				Tilko.API.REST _rest		= new Tilko.API.REST(_apiKey);
				_rest.Init();

				// 건강보험공단의 건강검진결과 endPoint 설정
				_rest.SetEndPointUrl("https://api.tilko.net/api/v1.0/nhis/ggpab003m0105");

				/*
				 * 공동인증서 경로 설정
				 * 공동인증서는 "C:\Users\[사용자계정]\AppData\LocalLow\NPKI\yessign\USER\[인증서DN명]"에 존재합니다.
				 */
				string _basePath			= Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\LocalLow\NPKI\yessign\USER\[인증서DN명]";
				string _publicPath			= string.Format(@"{0}\signCert.der", _basePath);
				string _privatePath			= string.Format(@"{0}\signPri.key", _basePath);
				byte[] _publicCert			= File.ReadAllBytes(_publicPath);
				byte[] _privateKey			= File.ReadAllBytes(_privatePath);
				
				// Body 추가
				_rest.AddBody("CertFile", _publicCert);
				_rest.AddBody("KeyFile", _privateKey);
				_rest.AddBody("CertPassword", "공동인증서_비밀번호");

				// API 호출
				string _result				= _rest.Call();
			}
			catch (Exception err)
			{
				Debug.WriteLine(err.Message);
			}
		}
	}
}
