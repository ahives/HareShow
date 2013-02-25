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
    using System.Linq.Expressions;
    using System.Reflection;
    using Contracts;
    using Quartz;
    using Quartz.Spi;

    public class HareShowJobFactory<T> :
        IJobFactory
        where T : IMonitor
    {
        private readonly Func<T, IJob> _jobFactory;
        private readonly T _monitor;

        public HareShowJobFactory(T monitor)
        {
            _monitor = monitor;
            _jobFactory = CreateConstructor();
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            if (jobDetail == null)
                throw new SchedulerException("");

            IJob job = _jobFactory(_monitor);

            return job;
        }

        public void ReturnJob(IJob job)
        {
        }

        private Func<T, IJob> CreateConstructor()
        {
            Type type = typeof (QueueMonitorJob);
            ConstructorInfo ctor = type.GetConstructor(new[] {typeof (T)});
            Func<T, IJob> job = CreateJob(ctor);

            return job;
        }

        private Func<T, IJob> CreateJob(ConstructorInfo ctor)
        {
            ParameterExpression monitor = Expression.Parameter(typeof (T), "monitor");
            NewExpression obj = Expression.New(ctor, monitor);

            return Expression.Lambda<Func<T, IJob>>(obj, monitor).Compile();
        }
    }
}