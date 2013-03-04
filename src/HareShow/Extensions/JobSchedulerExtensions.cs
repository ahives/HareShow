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

namespace HareShow.Extensions
{
    using System;
    using Quartz;

    public static class JobSchedulerExtensions
    {
        public static void Schedule<T>(this IScheduler scheduler, Guid jobId, DateTimeOffset startTime,
                                       TimeSpan interval, string username, string password)
            where T : IJob
        {
            IJobDetail jobDetail = CreateJobDetail<T>(jobId, username, password);
            ITrigger trigger = CreateJobTrigger(jobId, jobDetail, startTime, interval);

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        private static ITrigger CreateJobTrigger(Guid jobId, IJobDetail jobDetail, DateTimeOffset startTime,
                                                 TimeSpan interval)
        {
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

            return trigger;
        }

        private static IJobDetail CreateJobDetail<T>(Guid jobId, string username, string password)
            where T : IJob
        {
            IJobDetail jobDetail = JobBuilder.Create<T>()
                                             .WithIdentity(jobId.ToString("N"))
                                             .StoreDurably(true)
                                             .RequestRecovery(true)
                                             .UsingJobData("username", username)
                                             .UsingJobData("password", password)
                                             .Build();

            return jobDetail;
        }
    }
}