# CSharp
C# nuget 프로젝트의 소스코드입니다.

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
			_rest.AddBody("CertFile", _publicCert, true);					// 암호화 처리가 필요한 변수는 마지막 파라미터를 true로 처리
			_rest.AddBody("KeyFile", _privateKey, true);					// 암호화 처리가 필요한 변수는 마지막 파라미터를 true로 처리
			_rest.AddBody("CertPassword", "공동인증서_비밀번호", true);		// 암호화 처리가 필요한 변수는 마지막 파라미터를 true로 처리
			_rest.AddBody("평문", "test");									// 평문으로 전송

			// API 호출
			string _result				= _rest.Call();
		}
		catch (Exception err)
		{
			Debug.WriteLine(err.Message);
		}
	}