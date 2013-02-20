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
    using Quartz;

    public class JobCreator
    {
        public static void Create<T>(IScheduler scheduler, Guid jobId, DateTimeOffset startTime, TimeSpan interval)
            where T : IJob
        {
            IJobDetail jobDetail = JobBuilder.Create<T>()
                                             .WithIdentity(jobId.ToString("N"))
                                             .StoreDurably(true)
                                             .RequestRecovery(true)
                                             .Build();
            ITrigger trigger = TriggerBuilder.Create()
                                             .WithIdentity(jobId.ToString("N"))
                                             .ForJob(jobDetail)
                                             .StartAt(startTime)
                                             .WithSimpleSchedule(x =>
                                                                     {
                                                                         x.WithInterval(interval);
                                                                         x.RepeatForever();
                                                                     })
                                             .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}