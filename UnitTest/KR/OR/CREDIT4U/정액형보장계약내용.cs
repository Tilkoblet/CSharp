using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.CREDIT4U
{
	[TestClass]
	public class 정액형보장계약내용
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 한국신용정보원의 정액형 보장 계약내용 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/credit4u/contractdata");

				// Body 추가
				_rest.AddBody("UserID", "", true);                    // [암호화] 로그인 아이디(Base64 인코딩)
				_rest.AddBody("UserPassword", "", true);              // [암호화] 로그인 비밀번호(Base64 인코딩)
				_rest.AddBody("ContractStatus", "");                  // 계약상태(A:전체/S:정상 공백 시 정상 데이터만 조회합니다.)
																	  
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
