// Copyright 2013-2014 Albert L. Hives
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace HareShow.Jobs
{
    using System;
    using Monitors;
    using Quartz;
    using Security;

    public class QueueMonitorJob :
        IJob
    {
        private readonly IQueueMonitor _queueMonitor;
        private readonly ISecurity _security;

        public QueueMonitorJob(IQueueMonitor queueMonitor, ISecurity security)
        {
            _queueMonitor = queueMonitor;
            _security = security;
        }

        public void Execute(IJobExecutionContext context)
        {
            string username = _security.Decrypt(context.JobDetail.JobDataMap.GetString("username"));
            string password = _security.Decrypt(context.JobDetail.JobDataMap.GetString("password"));

            var stats = _queueMonitor.Get(username, password);
            Console.WriteLine("[{0}] Queue monitor fired.", context.FireTimeUtc.Value.ToString("MM/dd/yyyy hh:mm:ss"));
            //Console.WriteLine("Messages: {0}", stats.Messages);
            //Console.WriteLine("Messages Ready: {0}", stats.MessagesReady);
            //Console.WriteLine("Messages Unacknowledged: {0}", stats.MessagesUnacknowledged);
            //Console.WriteLine("Rate: {0}", stats.QueueTotals.MessagesDetails.Rate);
            //Console.WriteLine("Interval: {0}", stats.QueueTotals.MessagesDetails.Interval);
            //Console.WriteLine("Last Event: {0}", stats.QueueTotals.MessagesDetails.LastEvent);
            //Console.WriteLine("Rate: {0}", stats.QueueTotals.MessagesReadyDetails.Rate);
            //Console.WriteLine("Interval: {0}", stats.QueueTotals.MessagesReadyDetails.Interval);
            //Console.WriteLine("Last Event: {0}", stats.QueueTotals.MessagesReadyDetails.LastEvent);
            //Console.WriteLine("Rate: {0}", stats.QueueTotals.MessagesUnacknowledgedDetails.Rate);
            //Console.WriteLine("Interval: {0}", stats.QueueTotals.MessagesUnacknowledgedDetails.Interval);
            //Console.WriteLine("Last Event: {0}", stats.QueueTotals.MessagesUnacknowledgedDetails.LastEvent);
            //Console.WriteLine("****************************************");

            //_queueMonitor.Save(heartbeat);
        }
    }
}