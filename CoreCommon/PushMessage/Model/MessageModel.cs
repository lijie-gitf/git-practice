using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon
{
   public class MessageModel<T> where T:class
    {
        /// <summary>
        /// 回调
        /// </summary>
        public Func<T, bool> callbackFunc;

        /// <summary>
        /// 要发布的数据
        /// </summary>
        public string data;
    }
}
