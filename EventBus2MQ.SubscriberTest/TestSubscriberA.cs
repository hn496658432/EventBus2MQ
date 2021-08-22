﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus2MQ.SubscriberTest
{
    public class TestSubscriberA : ISubscriber
    {
        /// <summary>
        /// 消费队列
        /// </summary>
        public string QueueName => "test1";
        /// <summary>
        /// 订阅事件
        /// </summary>
        public string EventName => "amq.direct";

        /// <summary>
        /// 订阅者业务处理
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public Task ProcessData(string Message)
        {
            Console.WriteLine("{0}收到消息：{1}", QueueName, Message);
            return Task.CompletedTask;
        }
    }
}
