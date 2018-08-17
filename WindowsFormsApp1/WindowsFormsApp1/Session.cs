using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;

namespace DiamondStories
{
    class Session
    {
        int uid = 0;
        public static Random random = new Random();
        public async void SendSession(int id, Label la)
        {
            uid = id;
            la.Text = "Ustawiam sesję...";
            Uri myUri = new Uri("http://localhost:51836/api/accounts/Session");
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            webRequest.Method = "POST";
            webRequest.Headers.Add("Authorization", "Bearer " + Client.GetBearerToken());
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);

            HttpContent content = new FormUrlEncodedContent(
                new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("key1", "value1"),
                    new KeyValuePair<string,string>("key2","value2"),
                    new KeyValuePair<string,string>("key3","value3"),
                    new KeyValuePair<string,string>("key4", "value4"),
                    new KeyValuePair<string, string>("key5", "value5"),
                    new KeyValuePair<string, string>("key6", "value6")
                });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            content.Headers.ContentType.CharSet = "UTF-8";
            HttpResponseMessage resposne = await client.PostAsync(new Uri("my_url"), content);
        }

        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(callbackResult);
            string ip = new WebClient().DownloadString("http://ipinfo.io/ip").ToString().TrimEnd(Environment.NewLine.ToCharArray());
            string requestBody = string.Format("{{\"Id\":\"{0}\",\"Sessionip\":\"{1}\",\"Sessionid\":\"{2}\"}}", uid, ip, SessionID(20));
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            webRequest.BeginGetResponse(new AsyncCallback(GetResponseStreamCallback), webRequest);
        }

        public static string SessionID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnoprstuwxyzq";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
