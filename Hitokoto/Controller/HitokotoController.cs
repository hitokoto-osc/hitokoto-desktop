using Hitokoto.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Hitokoto.Conroller
{
    class HitokotoController
    {
        public static string GetOneSentence()
        {
            string sentence = HttpWebUtility.Get("https://v1.hitokoto.cn/");
            return sentence;
        }
    }
}
