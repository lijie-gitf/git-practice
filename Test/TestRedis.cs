using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class TestRedis
    {
        public static TestRedis user
        {
            get
            {
                return new TestRedis();
            }
        }
        public CSRedisClient client;

        public static readonly Dictionary<string, CSRedisClient> keyValues = new Dictionary<string, CSRedisClient>();

        public TestRedis()
        {

            if (!keyValues.ContainsKey("res"))
            {
                client = new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240,prefix=key");
                keyValues["res"] = client;

            }
            else
            {
                client = keyValues["res"];
            }


        }
        private static CSRedisClient _CacheMaster;
        private static object lockobj = new object();
        public static CSRedisClient CacheMaster
        {
            get
            {
                lock (lockobj)
                {
                    if (_CacheMaster == null)
                    {
                        _CacheMaster = new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240,prefix=key");
                    }
                    return _CacheMaster;
                }

            }
        }
    }
}
