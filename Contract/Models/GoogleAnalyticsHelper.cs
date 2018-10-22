using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Contract.Models
{
    public class GoogleAnalyticsHelper
    {
        /// <summary> Método que registra desde el servidor un evento en GoogleAnalytics </summary>
        /// <param name="trackingID">Identificador de la cuenta (UA-77043460-3)</param>
        /// <param name="category"> Event Category. Required.</param>
        /// <param name="action">Event Action. Required.</param>
        /// <param name="label">Event label.</param>
        /// <param name="value">Event value.</param>
        public static void TrackEvent(string trackingID, string category, string action, string label, string value)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = string.Format(@"v=1&tid={0}&cid=1684595580.1470327903&t=event&ec={1}&ea={2}&el={3}&ev={4}", trackingID, category, action, label, value);
            byte[] data = encoding.GetBytes(postData);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://www.google-analytics.com/collect");
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
        }
    }
}