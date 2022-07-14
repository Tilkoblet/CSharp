## CSharp
C# nuget 프로젝트의 소스코드입니다.


## 데이터 형태별 샘플 코드
|파일명|설명|API 예시|
|---|---|---|
|UnitTest/TestCase1.cs|인증서 필요 없음, 파라미터 암호화 필요 없음|인터넷등기소 등기물건 주소검색|
|UnitTest/TestCase2.cs|인증서 필요 없음, 파라미터 암호화 필요함|경찰청교통민원24 운전면허진위여부|
|UnitTest/TestCase3.cs|인증서 필요함|정부24 주민등록진위여부|
|UnitTest/TestCase4_1.cs|간편인증 요청|국민건강보험공단 간편인증 요청|
|UnitTest/TestCase4_2.cs|간편인증용 API 호출|국민건강보험공단 건강검진내역|
|UnitTest/TestCase5.cs|바이너리 데이터를 파일로 저장|정부24 건축물대장 발급|


## 샘플 코드 (API 호출)
아래는 건강보험공단의 건강검진결과 API를 호출하는 사용법입니다.

	public void TestMethod1()
	{
		try
		{
			// API 상세설명 URL
			// https://tilko.net/Help/Api/POST-api-apiVersion-Nhis-Ggpab003M0105

			Tilko.API.REST _rest		= new Tilko.API.REST("API_KEY");
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
			_rest.AddBody("CertFile", _publicCert);					// 자동으로 암호화 처리
			_rest.AddBody("KeyFile", _privateKey);					// 자동으로 암호화 처리
			_rest.AddBody("CertPassword", "공동인증서_비밀번호");	// 자동으로 암호화 처리
			_rest.AddBody("평문", "test", false);					// 평문으로 전송

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
