using System;
using System.IO;
using System.Net;
using Shahrdari.BussinessLayer;
using System.Web.Script.Serialization;
using System.Collections;

namespace Shahrdari.WebApplication.Areas.User.Helper
{
    public class ApiCall
    {
        private string url;
        private string data;

        public ApiCall(string Url, string Data)
        {
            url = Url;
            data = Data;
        }

        public string CallApiPost()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (Stream webStream = request.GetRequestStream())
            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
            {
                requestWriter.Write(data);
            }
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    return responseReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string CallApiGet()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

    }
}