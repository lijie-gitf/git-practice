using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CoreCommon.PushMessage
{
    public class ConnectionChannelPool : IConnectionChannelPool, IDisposable
    {
        //ConcurrentQueue 的默认大小
        const int defaultSize = 500;
        /// 创建一个连接的委托
        readonly Func<IConnection> connectionActivator;
        //管理消息队列的容器
        readonly ConcurrentQueue<IModel> pool = new ConcurrentQueue<IModel>();
        IConnection connection;

        int count;
        int maxSize;

        public ConnectionChannelPool(RabbitMQOptions options)
        {
            maxSize = defaultSize;
               connectionActivator = CreateConnection(options);
        }
        private Func<IConnection> CreateConnection(RabbitMQOptions options)
        {
            var factory = new ConnectionFactory { 
            HostName=options.HostName,
            VirtualHost=options.VirtualHost,
            UserName=options.UserName,
            Password=options.Password,
            RequestedConnectionTimeout = options.RequestedConnectionTimeout,
            SocketReadTimeout = options.SocketReadTimeout,
            SocketWriteTimeout = options.SocketWriteTimeout,
            AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled
           };
            return () => factory.CreateConnection();
        }


        /// <summary>
        /// 获取一个连接对象，如果容器中有，从容器取并移除
        /// </summary>
        /// <returns></returns>
        public IModel Rent()
        {
            IModel model;
            if (pool.TryDequeue(out model))
            {
                //线程管理容器中存在，取容器中，容器管理的大小-1
                Interlocked.Decrement(ref count);

                return model;
            }
            model = GetConnection().CreateModel();
            return model;
        }
        /// <summary>
        /// 推送结束后，将对象加入该容器
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Return(IModel model)
        { 
            if (Interlocked.Increment(ref count) <= maxSize)
            {
                //如果队列不存在，将消息队列加入线程管理队列
                pool.Enqueue(model);
                return true;
            }
            //容器管理的大小-1
            Interlocked.Decrement(ref count);

            return false;
        }
        public void Dispose()
        {
            maxSize = 0;
            IModel model;
            while (pool.TryDequeue(out model))
            {
                model.Dispose();
            }
        }

        public IConnection GetConnection()
        {
            if (connection != null && connection.IsOpen)
                return connection;

            connection = connectionActivator();
            connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;
            return connection;
        }

        /// <summary>
        /// 连接中断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {

        }
    }

}
