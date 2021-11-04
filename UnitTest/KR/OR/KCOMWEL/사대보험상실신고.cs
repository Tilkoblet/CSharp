using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace UnitTest.KR.OR.KCOMWEL
{
	[TestClass]
	public class 사대보험상실신고
	{
		[TestMethod]
		public void TILKO_API()
		{
			try
			{
				Tilko.API.REST _rest = new Tilko.API.REST(Constant.ApiKey);
				_rest.Init();

				// 건강보험공단의 사대보험상실신고 endPoint 설정
				_rest.SetEndPointUrl(Constant.ApiHost + "/api/v1.0/kcomwel/samusangsilsingo");

				// Body 추가
				_rest.AddBody("CertFile", File.ReadAllBytes(string.Format(@"{0}\signCert.der", Constant.CertPath)), true);
				_rest.AddBody("KeyFile", File.ReadAllBytes(string.Format(@"{0}\signPri.key", Constant.CertPath)), true);
				_rest.AddBody("CertPassword", Constant.CertPassword, true);
				_rest.AddBody("BusinessNumber", "2248167722", true);  // [암호화] 검색 할 사업자등록번호 또는 주민등록번호(xxxxxxxxxx 또는 xxxxxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("UserGroupFlag", "1");         // 인증서 - 사업장(0)/사무대행(1) 구분
				_rest.AddBody("IndividualFlag", "1");        // 인증서 - 개인(0)/법인(1) 구분
				_rest.AddBody("GwanriNo", "49379001870");              // 관리번호
				_rest.AddBody("GeunrojaRgNo", "", true);    // [암호화]근로자 주민등록번호(xxxxxxxxxxxxx / Base64 인코딩)
				_rest.AddBody("SangsilDt", "");             // 상실일자(yyyyMMdd)
				_rest.AddBody("DBosuChongak", "");          // 당해년도 보수총액
				_rest.AddBody("DSanjengMM", "");            // 당해년도 근무개월수
				_rest.AddBody("JBosuChongak", "");          // 전년도 보수총액
				_rest.AddBody("JSanjengMM", "");            // 전년도 근무개월수
				_rest.AddBody("SangsilSayu", "");           // 상실사유 - 개인사정으로인한자진퇴사(0)/사업장이전근로조건변동임금체불등으로자진퇴사(1)/폐업도산(2)/경영상필요및회사불황으로인원감축등에의한퇴사해고권고사직명예퇴직포함(3)/예술인근로자의귀책사유에의한징계해고권고사직(4)/정년(5)/계약기간만료공사종료(6)/고용보험비적용(7)/이중고용(8)
				_rest.AddBody("SangsilSayuDetail", "");     // 구체적 사유
				_rest.AddBody("NHICSangsilBuhoCd", "");     // 건강보험 상실 부호 - 퇴직(0)/사망(1)/의료급여수급권자(2)/유공자등건강보험배제신청(3)/기타외국인당연적용제외등(4)
				_rest.AddBody("NPSSangsilBuhoCd", "");      // 국민연금 상실 부호 - 사망(0)/사용관계종료(1)/국적상실국외이주(2)/육십세도달(3)/다른공적연금가입(4)/전출통폐합(5)/국민기초생활보장법에따른수급자(6)/노령연금수급권취득자중특수직종60세미만(7)/협정국연금가입(8)/체류기간만료외국인(9)/적용제외체류자격외국인(10)

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
