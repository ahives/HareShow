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

namespace HareShow
{
    using System;
    using Contracts;
    using HareDu;
    using Jobs;
    using Quartz;
    using Quartz.Impl;
    using Security;
    using Topshelf;

    public class QueueMonitorService :
        ServiceControl
    {
        private readonly HareDuClient _client;
        private readonly IScheduler _scheduler;

        public QueueMonitorService()
        {
            _scheduler = CreateScheduler();
            _client = HareDuFactory.New(x =>
                                            {
                                                x.ConnectTo("http://localhost:15672");
                                                x.EnableLogging("HareDuLogger");
                                            });
        }

        public bool Start(HostControl hostControl)
        {
            // TODO: get start datetime from app.config
            string startDateTimeString = "20130221";

            DateTime startDateTime;
            if (!DateTime.TryParse(startDateTimeString, out startDateTime))
            {
                // TODO: if the start datetime is bad then throw an exception here
            }

            // TODO: get interval from app.config
            string intervalString = "00:00:10";
            TimeSpan interval;
            if (!TimeSpan.TryParse(intervalString, out interval))
            {
                // TODO: if the interval time is bad then throw an exception here
            }

            // TODO: get username and password from app.config
            string username = "guest";
            string password = "guest";
            JobCreator.Create<QueueMonitorJob>(_scheduler, Guid.NewGuid(), new DateTimeOffset(startDateTime), interval,
                                               username, password);

            var queueMonitor = new QueueMonitor(_client);
            var security = new SecurityImpl();
            _scheduler.JobFactory = new HareShowJobFactory<IQueueMonitor, ISecurity>(queueMonitor, security);
            _scheduler.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _scheduler.Shutdown();

            return true;
        }

        private IScheduler CreateScheduler()
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            return scheduler;
        }
    }
}