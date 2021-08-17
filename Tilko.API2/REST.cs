using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Tilko.API.Models;

namespace Tilko.API
{
    /// <summary>
    /// RESTful API 호출을 위한 클래스입니다.
    /// </summary>
    public class REST
    {
		#region Fields
		static string _apiServer            = "https://api.tilko.net";
        string _endPointUrl                 = string.Empty;
        string _apiKey                      = string.Empty;     // API키
        byte[] _aesKey                      = new byte[16];     // AES 암호화에 사용할 키
        byte[] _aesIv                       = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };  // AES 암호화에 사용할 IV(고정)
        byte[] _rsaPublicKey                = new byte[0];
        static Random _rnd                  = new Random();
        Tilko.API.Encryption.AES _aes       = new Tilko.API.Encryption.AES();
        Dictionary<string, string> _headers = new Dictionary<string, string>();
        Dictionary<string, string> _bodies  = new Dictionary<string, string>();
        #endregion

        #region REST : 생성자
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="ApiKey">API키</param>
        public REST(string ApiKey)
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new Exception("API key cannot be null or empty.");
            }
            _apiKey     = ApiKey;
        }
        #endregion

        #region SetEndPointUrl : API 호출 URL 설정
        /// <summary>
        /// API 호출 URL 설정
        /// </summary>
        /// <param name="EndPointUrl">API 호출 URL</param>
        public void SetEndPointUrl(string EndPointUrl)
        {
            if (string.IsNullOrEmpty(EndPointUrl))
            {
                throw new Exception("EndPointUrl cannot be null or empty.");
            }
            _endPointUrl     = EndPointUrl;
        }
        #endregion

        #region Init : REST를 초기화
        /// <summary>
        /// REST를 초기화 합니다.
        /// 전달 받은 API키에 대응되는 RSA 공개키를 서버로부터 수신합니다.
        /// </summary>
        public void Init()
		{
			try
			{
                _headers                = new Dictionary<string, string>();
                _bodies                 = new Dictionary<string, string>();

                // EndPoint init
                _endPointUrl            = string.Empty;

                // AES init
                _rnd.NextBytes(_aesKey);

                // RSA init
				RsaPublicKey _pubKey;
				using (HttpClient _httpClient = new HttpClient())
				{
					_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
					
					// 틸코 인증 서버에 RSA 공개키 요청
                    string _url                 = string.Format("{0}/api/Auth/GetPublicKey?APIkey={1}", _apiServer, _apiKey);
					var _response				= _httpClient.GetAsync(_url).Result;
					var _resContent				= _response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
					_pubKey						= JsonConvert.DeserializeObject<RsaPublicKey>(_resContent.ToString());
                    if (_pubKey.Status != "OK")
                    {
                        throw new Exception(_pubKey.Message);
                    }
                    else if (_pubKey.ApiKey != _apiKey)
                    {
                        throw new Exception("Requested API key and responsed API key does not match!");
                    }
                    _rsaPublicKey               = Convert.FromBase64String(_pubKey.PublicKey);
				}

                // Encrypt AES key
				byte[] _aesCipherKey			= new byte[0];
                using (var _rsa = Tilko.API.Encryption.RSA.DecodePublicKey(_rsaPublicKey))
                {
					_aesCipherKey       = _rsa.Encrypt(_aesKey, RSAEncryptionPadding.Pkcs1);

                    _headers.Add("API-KEY", _apiKey);
                    _headers.Add("ENC-KEY", Convert.ToBase64String(_aesCipherKey));
                }
			}
			catch
			{
				throw;
			}
        }
        #endregion

        #region AddBody : API BODY에 들어갈 값을 추가
        /// <summary>
        /// API BODY에 들어갈 값을 추가합니다.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void AddBody(string Key, string Value)
		{
			try
			{
                if (_bodies.ContainsKey(Key))
                {
                    throw new Exception("There is already the same key in bodies.");
                }
                byte[] _cipher      = _aes.Encrypt(_aesKey, _aesIv, Encoding.UTF8.GetBytes(Value));
                _bodies.Add(Key, Convert.ToBase64String(_cipher));
			}
			catch
			{
				throw;
			}
        }

        /// <summary>
        /// API BODY에 들어갈 값을 추가합니다.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void AddBody(string Key, int Value)
		{
			try
			{
                if (_bodies.ContainsKey(Key))
                {
                    throw new Exception("There is already the same key in bodies.");
                }
                byte[] _intBytes     = BitConverter.GetBytes(Value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(_intBytes);
                byte[] _result      = _intBytes;
                byte[] _cipher      = _aes.Encrypt(_aesKey, _aesIv, _result);
                _bodies.Add(Key, Convert.ToBase64String(_cipher));
			}
			catch
			{
				throw;
			}
        }

        /// <summary>
        /// API BODY에 들어갈 값을 추가합니다.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void AddBody(string Key, byte[] Value)
		{
			try
			{
                if (_bodies.ContainsKey(Key))
                {
                    throw new Exception("There is already the same key in bodies.");
                }
                byte[] _cipher      = _aes.Encrypt(_aesKey, _aesIv, Value);
                _bodies.Add(Key, Convert.ToBase64String(_cipher));
			}
			catch
			{
				throw;
			}
        }
        #endregion

        public string Call()
		{
			try
			{
				using (HttpClient _httpClient = new HttpClient())
				{
					_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    /*
					 * 헤더 설정
					 */
                    foreach (var _item in _headers)
                    {
					    _httpClient.DefaultRequestHeaders.Add(_item.Key, _item.Value);
                    }

					// 틸코 데이터 서버에 데이터 요청
					var _reqContent				= new StringContent(JsonConvert.SerializeObject(_bodies), Encoding.UTF8, "application/json");
					var _response				= _httpClient.PostAsync(_endPointUrl, _reqContent).Result;
					var _resContent				= _response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
					return _resContent.ToString();
				}
			}
			catch
			{
				throw;
			}
        }
    }
}
