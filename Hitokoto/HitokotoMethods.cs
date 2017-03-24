using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hitokoto
{
    class HitokotoMethods
    {
        public static string  Get()
        {
            string result = "";
            HttpWebResponse hitokotoResponse =  HttpWebResponseUtility.CreateGetHttpResponse("https://sslapi.hitokoto.cn/", 5000, "HitokotoDesktop/1.0", "", null);
            Stream resStream = hitokotoResponse.GetResponseStream();
            using (StreamReader sr = new StreamReader(resStream))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }
    }
}
