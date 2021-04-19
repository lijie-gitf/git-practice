using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //RabbitTest();
            RedisTest();
        }
        /// <summary>
        /// redis测试
        /// </summary>
        public static void RedisTest()
        {
            //连接哨兵
            //var csredis = new CSRedis.CSRedisClient("redis-master", new[] { "127.0.0.1:27000" });
            var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase = 1,poolsize = 50,ssl = false,writeBuffer = 10240");
           
            //初始化redisHelper
            RedisHelper.Initialization(csredis);
            RedisHelper.Del("key1");
            var a = RedisHelper.ZAdd("key1", (1, "这是1"), (2, "这是2"));
            var c = RedisHelper.ZAdd("key1", (3, "这是3"), (0, "这是2"));
            //var b = RedisHelper.ZScore("key1", 2);
            var d = RedisHelper.ZScan("key1", 2);
            int ii = -1;
            var c1 = RedisHelper.ZCard("key1");
            Console.WriteLine(c1);
            //Console.WriteLine(a);
            //Console.WriteLine(a);
            //Console.WriteLine(c);
            //Console.WriteLine(b);
            //Console.WriteLine(d);
            Console.WriteLine(ii);
            foreach ((string, decimal) item in d.Items)
            {

                Console.WriteLine(item.Item1);
            }
            //int i=0;

            //while (true)
            //{
            //    i++;
            //    try
            //    {
            //        Console.WriteLine($"第{i}次写入");
            //        RedisHelper.Set("test_key", $"第{i}次写入");
            //        Console.ReadLine();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //          Console.ReadLine();
            //    }
            //}

        }
        /// <summary>
        /// rabbitmq测试
        /// </summary>
        public static void RabbitTest()
        {
            Task t = new Task(() =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                    VirtualHost = "/",
                    AutomaticRecoveryEnabled = true

                };
                //创建消息队列连接
                var connection = factory.CreateConnection();

                IModel channel = connection.CreateModel();
                //绑定交换机
                channel.ExchangeDeclare("lijie_Exchange", "direct", true, false);

                //申明消息队列
                channel.QueueDeclare("jietest0304", true, false, false);

                //绑定消息队列到交换机
                channel.QueueBind("jietest0304", "lijie_Exchange", "Key_jietest0304");
                //费者在接收到队列里的消息但没有返回确认结果之前,队列不会将新的消息分发给该消费者。队列中没有被消费的消息不会被删除，还是存在于队列中
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                EventingBasicConsumer consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
                //消费事件
                consumer.Received += (p, q) =>
                {
                    ReadOnlyMemory<byte> body = q.Body;
                    var message = System.Text.Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine($"消费:{message}");
                    //消息确认
                    channel.BasicAck(deliveryTag: q.DeliveryTag, multiple: false);
                };
                //启动消费
                channel.BasicConsume("jietest0304", autoAck: false, consumer: consumer);
            });
            t.Start();
            while (true)
            {

            }
        }
    }
}
