using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Shahrdari.WebApplication.ApplicationHelper
{
    public class Sms
    {
        private static readonly HttpClient client = new HttpClient();
        public bool SendSms(string PhoneNumber,string Code)
        {
            var api = new Kavenegar.KavenegarApi("71625976436F417872583968335938637870516D4F3131304B7634746D796743");
            api.VerifyLookup(PhoneNumber, Code, "VerifySabzBash");
            return true;
        }
    }
    /*public static class SmsPattern
    {
        public string VerifyCode { get { return  }; set; }
    }*/
}