using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace NutritionRestAPI
{
    public static class RestAPI
    {
        public static Nutrition RestAPICall(string url, string username, string password)
        {
            Nutrition nutri = new Nutrition();

            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Credentials = GetCredential(url, username, password);
                request.PreAuthenticate = true;
                string responseText = string.Empty;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }


                nutri = Newtonsoft.Json.JsonConvert.DeserializeObject<Nutrition>(responseText);

                return nutri;
            }

            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse resp = ex.Response as HttpWebResponse;
                    if (resp != null)
                        nutri.StatusCode = resp.StatusCode;
                    else
                        nutri.StatusCode = HttpStatusCode.InternalServerError;
                }
                else
                    nutri.StatusCode = HttpStatusCode.InternalServerError;
            }

            catch (Exception)
            {
                nutri.StatusCode = HttpStatusCode.InternalServerError;
            }
            
            return nutri;
        }

        private static CredentialCache GetCredential(string url,string username,string password)
        {            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential(username,password));
            return credentialCache;
        }
    }
}