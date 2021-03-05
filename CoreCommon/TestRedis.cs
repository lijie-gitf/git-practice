using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon
{
    public class TestRedis
    {
        private object lockobj = new object();
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
            lock (lockobj)
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
           


        }
       
    }
}
