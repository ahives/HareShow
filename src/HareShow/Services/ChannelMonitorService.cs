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

namespace HareShow.Services
{
    using System;
    using Configuration;
    using Exceptions;
    using Extensions;
    using HareDu;
    using Jobs;
    using Monitors;
    using Quartz;
    using Security;
    using Topshelf;

    public class ChannelMonitorService :
        ServiceControl
    {
        private readonly HareDuClient _client;
        private readonly IScheduler _scheduler;
        private HareShowConfig _config;

        public ChannelMonitorService()
        {
            _scheduler = HareDuScheduler.CreateScheduler();
            _config = new HareShowConfig();

            var clientConfig = _config.GetConfigSection("HareDuSettings/Client");
            var url = clientConfig.Get("url");
            var loggerName = clientConfig.Get("logger");

            _client = HareDuFactory.New(x =>
                                            {
                                                x.ConnectTo(url);
                                                x.EnableLogging(y => y.Logger(loggerName));
                                            });
        }

        public bool Start(HostControl hostControl)
        {
            var runtime = _config.GetConfigSection("HareShowRuntimeSettings/EventTrigger");

            // TODO: get start datetime from app.config
            DateTime startDateTime;
            if (string.IsNullOrWhiteSpace(runtime.Get("startDateTime")))
            {
                startDateTime = DateTime.Now;
            }
            else
            {
                if (!DateTime.TryParse(runtime.Get("startDateTime"), out startDateTime))
                {
                    // TODO: if the start datetime is bad then throw an exception here
                }
            }

            // TODO: get interval from app.config
            TimeSpan interval;
            if (!TimeSpan.TryParse(runtime.Get("runInterval"), out interval))
            {
                // TODO: if the interval time is bad then throw an exception here
            }

            // TODO: get username and password from app.config
            var hareDuCredentials = _config.GetConfigSection("HareDuSettings/Login");

            if (string.IsNullOrWhiteSpace(hareDuCredentials.Get("username")) ||
                string.IsNullOrWhiteSpace(hareDuCredentials.Get("password")))
                throw new UserCredentialsInvalidException("Not able to connect to RabbitMQ because username or password is invalid.");

            string username = hareDuCredentials.Get("username");
            string password = hareDuCredentials.Get("password");

            _scheduler.Schedule<ChannelMonitorJob>(Guid.NewGuid(), new DateTimeOffset(startDateTime), interval, username, password);
            var monitor = new ChannelMonitor(_client);
            var security = new SecurityImpl();
            _scheduler.JobFactory = new HareShowJobFactory<IChannelMonitor, ISecurity, ChannelMonitorJob>(monitor, security);
            _scheduler.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _scheduler.Shutdown();

            return true;
        }
    }
}