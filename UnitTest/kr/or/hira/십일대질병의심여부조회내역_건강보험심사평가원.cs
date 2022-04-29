using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace UnitTest.kr.or.hira
{
    [TestClass]
    public class 십일대질병의심여부조회내역_건강보험심사평가원
    {
        string apiHost   = "https://api.tilko.net/";
        string apiKey    = "";


        // RSA 암호화 함수
        public string rsaEncrypt(string publicKey, byte[] aesKey)
        {
            string encryptedData	= "";

            using (RSACryptoServiceProvider rsaCSP = importPublicKey(publicKey))
            {
                byte[] byteEncryptedData	= rsaCSP.Encrypt(aesKey, false);
                encryptedData				= Convert.ToBase64String(byteEncryptedData);
                rsaCSP.Dispose();
            }

            return encryptedData;
        }


        public static RSACryptoServiceProvider importPublicKey(string pem)
        {
            string PUBLIC_HEADER	= "-----BEGIN PUBLIC KEY-----";
            string PUBLIC_FOOTER	= "-----END PUBLIC KEY-----";

            if (!pem.Contains(PUBLIC_HEADER))
            {
                pem = PUBLIC_HEADER + Environment.NewLine +
                      pem + Environment.NewLine +
                      PUBLIC_FOOTER;
            }

            PemReader pr						= new PemReader(new StringReader(pem));
            AsymmetricKeyParameter publicKey	= (AsymmetricKeyParameter)pr.ReadObject();
            RSAParameters rsaParams				= DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);

            RSACryptoServiceProvider csp		= new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParams);

            return csp;
        }


        // RSA 공개키(Public Key) 조회 함수
        public string getPublicKey()
        {
            string rsaPublicKey	= "";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // 틸코 인증 서버에 RSA 공개키 요청
                string url	= string.Format("{0}/api/Auth/GetPublicKey?APIkey={1}", apiHost, apiKey);
                using (var response = httpClient.GetAsync(url).Result)
                {
                    var resContent	= response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject resJson	= JObject.Parse(resContent);

                    rsaPublicKey	= (string)resJson["PublicKey"].ToString();
                }
            }

            return rsaPublicKey;
        }


        [TestMethod]
        public void main()
        {
            // RSA Public Key 조회
            string rsaPublicKey		= getPublicKey();
            Debug.WriteLine("rsaPublicKey: " + rsaPublicKey);


            // AES Secret Key 및 IV 생성
            byte[] aesKey			= new byte[16];
            new Random().NextBytes(aesKey);
            byte[] aesIv			= new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };


            // AES Key를 RSA Public Key로 암호화
            string aesCipherKey		= rsaEncrypt(rsaPublicKey, aesKey);
            Debug.WriteLine("aesCipherKey: " + aesCipherKey);


            // API URL 설정
            // HELP: https://tilko.net/Help/Api/POST-api-apiVersion-Hira-SuspectedDiseasesGet
            string url				= apiHost + "api/v1.0/Hira/SuspectedDiseasesGet";
            

            // API 요청 파라미터 설정
            Dictionary<string, string> bodies	= new Dictionary<string, string>();
            bodies.Add("MedicineCodeList", "__VALUE__");


            // API 호출
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("API-KEY", apiKey);
                httpClient.DefaultRequestHeaders.Add("ENC-KEY", aesCipherKey);

                // 틸코 데이터 서버에 데이터 요청
                var reqContent = new StringContent(JsonConvert.SerializeObject(bodies), Encoding.UTF8, "application/json");
                using (var response = httpClient.PostAsync(url, reqContent).Result)
                {
                    var resContent	= response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Debug.WriteLine("resContent: " + resContent);

                    
                }
            }
        }
    }
}
