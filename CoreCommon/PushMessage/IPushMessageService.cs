using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.PushMessage
{
   public interface IPushMessageService
    {
        /// <summary>
        /// 推送消息
        /// </summary>
        void PushMessage<T>(MessageModel<T> messageModel) where T :class;
    }
}
