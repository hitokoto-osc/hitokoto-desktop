using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hitokoto
{
    public class HitokotoEntity
    {
        public class Sentence
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 一言内容
            /// </summary>
            public string hitokoto { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 来自
            /// </summary>
            public string from { get; set; }
            /// <summary>
            /// 创建者
            /// </summary>
            public string creator { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public string cearted_at { get; set; }
        }
    }
}
