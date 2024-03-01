using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Tollab.Admin.Web.Services.Zoom
{
    public class ZoomManagerService : IZoomManagerService
    {
        public (HttpStatusCode status, string hostURL, string joinURL , long meetingId, string meetingPassword) CreateSchedualMeeting(string name, int duration, DateTime date, long countryId,int zoomAccount)
        {
            var apiSecretCast = "";
            var IssuerCast  = "";
            if(zoomAccount == 1)
            {
                apiSecretCast = "JTpmaAaDaTtlJcjLw7pTDId899RydwLrVQ8R";
                IssuerCast = "ZU2qefs0Rp6rLTwOsPq9lQ";
            }else if(zoomAccount == 2)
            {
                apiSecretCast = "PpvvxTWgvFFVcv0lgLze3W8KeoEy310LFGrH";
                IssuerCast = "joMTQWA_Ts2h60CQ5IWj_g";
            }            
            else if(zoomAccount == 3)
            {
                apiSecretCast = "BHPKfMFwPuQvycVdCxfYTSFtihDa6VKx3LFl";
                IssuerCast = "RKgLUJLOQ3WEFwq6g3nZwg";
            }
            else if(zoomAccount == 4)
            {
                apiSecretCast = "R4M4Q67Al8pvB0yLRvVhrI9GnptkKs6NVgEq";
                IssuerCast = "0ppklubpSEq8cv6ECblC4Q";
            }
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var apiSecret = apiSecretCast;

            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = IssuerCast,
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var client = new RestClient("https://api.zoom.us/v2/users/me/meetings");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            var passwordForMeeting = GetRandomPassword();
            request.AddJsonBody(new 
            { 
                topic = name, 
                duration = duration.ToString(), 
                start_time = date.ToString("yyyy-MM-ddTHH:mm:ss"), 
                timezone = GetTimeZone(countryId), 
                type = "2" ,
                password = passwordForMeeting
            });

            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));
            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            var jObject = JObject.Parse(restResponse.Content);

            var host = (string)jObject["start_url"];
            var join = (string)jObject["join_url"];
            var id = (long)jObject["id"];
            var password = string.IsNullOrEmpty(((string)jObject["password"])) ? passwordForMeeting : ((string)jObject["password"]);
            return (statusCode, host, join, id, password);
        }

        public HttpStatusCode Delete(long meetingId)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var apiSecret = "C8IKO7IHqUTfFaaWO7g72z7ycB9KTLjr3mrE";
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "SRPZGtMSS-Wfw86avfJjqA",
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var client = new RestClient("https://api.zoom.us/v2/meetings/"+ meetingId);
            var request = new RestRequest(Method.DELETE);

            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));
            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            return statusCode;
        }

        public HttpStatusCode UpdateSchedualMeeting(string name, int duration, DateTime date, long countryId , long meetingId,int zoomAccount)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var apiSecretCast = "";
            var IssuerCast = "";
            if (zoomAccount == 1)
            {
                apiSecretCast = "JTpmaAaDaTtlJcjLw7pTDId899RydwLrVQ8R";
                IssuerCast = "ZU2qefs0Rp6rLTwOsPq9lQ";
            }
            else if (zoomAccount == 2)
            {
                apiSecretCast = "PpvvxTWgvFFVcv0lgLze3W8KeoEy310LFGrH";
                IssuerCast = "joMTQWA_Ts2h60CQ5IWj_g";
            }
            else if (zoomAccount == 3)
            {
                apiSecretCast = "BHPKfMFwPuQvycVdCxfYTSFtihDa6VKx3LFl";
                IssuerCast = "RKgLUJLOQ3WEFwq6g3nZwg";
            }
            else if (zoomAccount == 4)
            {
                apiSecretCast = "R4M4Q67Al8pvB0yLRvVhrI9GnptkKs6NVgEq";
                IssuerCast = "0ppklubpSEq8cv6ECblC4Q";
            }
            var apiSecret = apiSecretCast;
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = IssuerCast,
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var client = new RestClient("https://api.zoom.us/v2/meetings/"+ meetingId);
            var request = new RestRequest(Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new
            {
                topic = name,
                duration = duration.ToString(),
                start_time = date.ToString("yyyy-MM-ddTHH:mm:ss"),
                timezone = GetTimeZone(countryId),
                type = "2"
            });

            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));
            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            return statusCode;
        }


        private string GetTimeZone(long  countryId)
        {
            switch(countryId)
            {
                case 3:
                    return "Asia/Kuwait";
                case 20011:
                    return "Africa/Cairo";
                case 20012:
                    return "Asia/Amman";
                case 20013:
                    return "Asia/Qatar";
                default:
                    return null;
            }
        }

        private string GetRandomPassword()
        {
            string password = string.Empty;
            Random randomGenerator = new Random();
            for (int i=0;i< 6; i++)
            {
                var num = randomGenerator.Next(0, 10);
                password += num.ToString();
            }
            return password;
        }
    }
}