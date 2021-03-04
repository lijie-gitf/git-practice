using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.PushMessage
{
   public interface IConnectionChannelPool
    {
        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <returns></returns>
         IConnection GetConnection();

        /// <summary>
        /// 获取一个Rabbit实例
        /// </summary>
        /// <returns></returns>
        IModel Rent();

        /// <summary>
        /// 将rabbit放入ConcurrentQueue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Return(IModel model);

    }
}
