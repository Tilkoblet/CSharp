using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.CREDIT4U
{
	[TestClass]
	public class SMS본인인증서비스2단계
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 한국신용정보원의 SMS 본인인증 서비스 2단계 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/credit4u/checkedselfauthcode");

				// Body 추가
				_rest.AddBody("UserName", "", true);                   // [암호화] 가입자 명(Base64 인코딩)
				_rest.AddBody("IdentityNumber", "", true);             // [암호화] 주민등록번호(8012151XXXXXX / Base64 인코딩)
				_rest.AddBody("UserCellphone", "", true);              // [암호화] 연락처(010XXXXXXXX / Base64 인코딩)
				_rest.AddBody("CaptchaCode", "");                      // 캡챠코드Tilko Session이 유지되는 180초 이내에 입력을 하셔야 합니다.

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
