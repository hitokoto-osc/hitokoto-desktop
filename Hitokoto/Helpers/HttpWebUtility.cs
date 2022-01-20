using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Hitokoto.Helpers
{
    class HttpWebUtility
    {
        private static readonly string DefaultUserAgent = "HitokotoDesktop/1.0";
        public static int TimeOut { get; set; } = 5000;

        public static string Get(string url)
        {
            try
            {
                HttpResponseMessage webresponse = CreateGetHttpResponse(url, TimeOut, DefaultUserAgent, "", null);
                if (webresponse == null)
                {
                    return "";
                }
                Stream resStream = webresponse.Content.ReadAsStream();
                using (StreamReader sr = new(resStream))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Get Error:" + ex.Message);
            }
            return "";
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="referer">请求来源Referer，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        private static HttpResponseMessage CreateGetHttpResponse(string url, int? timeout, string userAgent, string referer, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            CookieContainer cookiesContainer = new();
            HttpClientHandler handler = new() { CookieContainer = cookiesContainer };
            HttpClient client = new(handler);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(DefaultUserAgent);
            client.Timeout = TimeSpan.FromSeconds(10);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrEmpty(userAgent))
            {
                requestMessage.Headers.UserAgent.Clear();
                requestMessage.Headers.UserAgent.ParseAdd(userAgent);
            }
            if (!string.IsNullOrEmpty(referer)) { requestMessage.Headers.Referrer = new Uri(referer); }
            if (timeout.HasValue) { client.Timeout = TimeSpan.FromMilliseconds(timeout.Value); }
            if (cookies != null) { cookiesContainer.Add(cookies); }

            return client.Send(requestMessage);
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpResponseMessage CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            return CreatePostHttpResponse(url, parameters, timeout, userAgent, null, requestEncoding, cookies);
        }
        public static HttpResponseMessage CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, string referer, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException(nameof(requestEncoding));
            }
            CookieContainer cookiesContainer = new();
            HttpClientHandler handler = new() { CookieContainer = cookiesContainer };
            HttpClient client = new(handler);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(DefaultUserAgent);
            client.Timeout = TimeSpan.FromSeconds(10);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(parameters) };

            if (!string.IsNullOrEmpty(userAgent))
            {
                requestMessage.Headers.UserAgent.Clear();
                requestMessage.Headers.UserAgent.ParseAdd(userAgent);
            }
            if (!string.IsNullOrEmpty(referer)) { requestMessage.Headers.Referrer = new Uri(referer); }
            if (timeout.HasValue) { client.Timeout = TimeSpan.FromMilliseconds(timeout.Value); }
            if (cookies != null) { cookiesContainer.Add(cookies); }

            return client.Send(requestMessage);
        }
    }
}
