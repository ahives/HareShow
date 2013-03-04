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

    public class ChannelMonitorJob :
        IJob
    {
        private readonly IChannelMonitor _monitor;
        private readonly ISecurity _security;

        public ChannelMonitorJob(IChannelMonitor monitor, ISecurity security)
        {
            _monitor = monitor;
            _security = security;
        }

        public void Execute(IJobExecutionContext context)
        {
            string username = _security.Decrypt(context.JobDetail.JobDataMap.GetString("username"));
            string password = _security.Decrypt(context.JobDetail.JobDataMap.GetString("password"));

            var stats = _monitor.Get(username, password);
            Console.WriteLine("[{0}] Channel monitor fired.", context.FireTimeUtc.Value.ToString("MM/dd/yyyy hh:mm:ss"));

            //foreach (var channel in stats)
            //{
            //    Console.WriteLine("Acknowledged = {0}", channel.Acknowledged);
            //}
        }
    }
}