using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.GO.IROS
{
    [TestClass]
    public class 등기부등본조회
    {
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 인터넷등기소의 등기부등본조회 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/iros/risuretrieve");

				// Body 추가
				_rest.AddBody("IrosID", "로그인ID", true);		// [암호화] iros.go.kr 로그인 ID(Base64 인코딩)
				_rest.AddBody("IrosPwd", "로그인PW", true);		// [암호화] iros.go.kr 로그인 패스워드(Base64 인코딩)
				_rest.AddBody("EmoneyNo1", "", true);			// [암호화] 전자지불 선불카드 총 12자리 중 영문을 포함한 앞 8자리 입력(Base64 인코딩)
				_rest.AddBody("EmoneyNo2", "", true);			// [암호화] 전자지불 선불카드 총 12자리 중 나머지 뒤 4자리 숫자 입력(Base64 인코딩)
				_rest.AddBody("EmoneyPwd", "", true);			// [암호화] 전자지불 선불카드 비밀번호(Base64 인코딩)
				_rest.AddBody("UniqueNo", "");					// 부동산 고유번호('-'을 제외한 14자리)
				_rest.AddBody("JoinYn", "");					// 공동담보/전세목록 추출여부(Y/N 공백 또는 다른 문자열일 경우 기본값 N)
				_rest.AddBody("CostsYn", "");					// 매매목록추출여부(Y/N 공백 또는 다른 문자열일 경우 기본값 N)
				_rest.AddBody("DataYn", "");					// 전산폐쇄추출여부(Y/N 공백 또는 다른 문자열일 경우 기본값 N)
				_rest.AddBody("ValidYn", "");					// 유효사항만 포함여부(Y/N 공백 또는 다른 문자열일 경우 기본값 N)
				_rest.AddBody("IsSummary", "");					// 요약데이터 표시여부(Y/N 공백 또는 다른 문자열일 경우 기본값 Y)

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
