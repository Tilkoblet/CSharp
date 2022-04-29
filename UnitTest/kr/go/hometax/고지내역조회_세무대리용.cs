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

namespace UnitTest.kr.go.hometax
{
    [TestClass]
    public class 고지내역조회_세무대리용
    {
        string apiHost   = "https://api.tilko.net/";
        string apiKey    = "";


        // AES 암호화 함수
        public string aesEncrypt(byte[] key, byte[] iv, byte[] plainText)
        {
            byte[] ret		= new byte[0];

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.Key 		= key;
                aes.IV 			= iv;
                aes.Mode 		= CipherMode.CBC;
                aes.Padding 	= PaddingMode.PKCS7;

                using (ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                        {
                            cs.Write(plainText, 0, plainText.Length);
                            cs.FlushFinalBlock();
                            ret = ms.ToArray();
                        }
                    }
                }
                aes.Clear();
            }

            return Convert.ToBase64String(ret);
        }


        // AES 암호화 함수
        public string aesEncrypt(byte[] key, byte[] iv, string plainText)
        {
            byte[] ret		= new byte[0];

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.Key				= key;
                aes.IV				= iv;
                aes.Mode			= CipherMode.CBC;
                aes.Padding			= PaddingMode.PKCS7;

                using (ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                        {
                            cs.Write(Encoding.UTF8.GetBytes(plainText), 0, Encoding.UTF8.GetBytes(plainText).Length);
                            cs.FlushFinalBlock();
                            ret	= ms.ToArray();
                        }
                    }
                }
                aes.Clear();
            }

            return Convert.ToBase64String(ret);
        }


        // RSA 암호화 함수
        public string rsaEncrypt(string publicKey, byte[] aesKey)
        {
            string encryptedData 	= "";

            using (RSACryptoServiceProvider rsaCSP = importPublicKey(publicKey))
            {
                byte[] byteEncryptedData 	= rsaCSP.Encrypt(aesKey, false);
                encryptedData 				= Convert.ToBase64String(byteEncryptedData);
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
            // HELP: https://tilko.net/Help/Api/POST-api-apiVersion-HometaxAgent-UTENFAAA07-SuImNabSeJa-GoJiNaeYeog
            string url				= apiHost + "api/v1.0/HometaxAgent/UTENFAAA07/SuImNabSeJa/GoJiNaeYeog";
            

            // 인증서 경로 설정
            string certPath  = "C:/Users/username/AppData/LocalLow/NPKI/yessign/USER/user01/";
            string certFile  = certPath + "signCert.der";
            string keyFile   = certPath + "signPri.key";


            // API 요청 파라미터 설정
            Dictionary<string, string> bodies	= new Dictionary<string, string>();
            bodies.Add("CertFile", aesEncrypt(aesKey, aesIv, File.ReadAllBytes(certFile)));
            bodies.Add("KeyFile", aesEncrypt(aesKey, aesIv, File.ReadAllBytes(keyFile)));
            bodies.Add("CertPassword", aesEncrypt(aesKey, aesIv, "__VALUE__"));
            bodies.Add("AgentId", aesEncrypt(aesKey, aesIv, "__VALUE__"));
            bodies.Add("AgentPassword", aesEncrypt(aesKey, aesIv, "__VALUE__"));
            bodies.Add("BusinessNumber", aesEncrypt(aesKey, aesIv, "__VALUE__"));


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
