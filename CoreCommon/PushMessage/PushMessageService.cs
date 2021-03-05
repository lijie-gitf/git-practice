
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreCommon.PushMessage
{
    public class PushMessageService : IPushMessageService
    {
        private IConnectionChannelPool _pool;
        private PublicOptions _options;
        string routeKey = "";
        public PushMessageService(IConnectionChannelPool pool, PublicOptions options)
        {
            _pool = pool;
            _options = options;
            Init();

        }
      
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            var channel = _pool.Rent();
            try
            {
                routeKey = _options.Queue;

                //创建队列
                channel.QueueDeclare(_options.Queue, true, false, false, new Dictionary<string, object>()
                {
                    //{ "x-message-ttl",60 * 60 * 1000 * 24 * 7},
                });

                //创建绑定
                if (!string.IsNullOrWhiteSpace(_options.Exchange))
                {
                    routeKey = $"Key_{routeKey}";
                    channel.ExchangeDeclare(_options.Exchange, "direct", true);
                    channel.QueueBind(_options.Queue, _options.Exchange, routeKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _pool.Return(channel);
        }
        public void PushMessage<T>(MessageModel<T> messageModel) where T : class
        {
            
            var channel = _pool.Rent();

            HashSet<ulong> seqs = new HashSet<ulong>();

            //消息确认事件
            channel.BasicAcks += (sender, e) => {

            };

            //消息取消事件
            channel.BasicNacks += (sender, e) =>
            {
               
            };

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
           
            //开启消息确认模式
            channel.ConfirmSelect();

            //发送消息
            channel.BasicPublish(_options.Exchange, routeKey, properties, Encoding.UTF8.GetBytes(messageModel.data));

            
            _pool.Return(channel);
        }
    }
    /// <summary>
    /// 发布订单消息配置
    /// </summary>
    public class PublicOptions
    {
        public string Exchange { get; set; }

        public string Queue { get; set; }

        public int RetryCount { get; set; } = 3;

        public int WaitCount { get; set; } = 100;
    }
}
