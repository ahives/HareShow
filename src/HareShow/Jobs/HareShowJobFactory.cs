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
    using System.Linq.Expressions;
    using System.Reflection;
    using Monitors;
    using Quartz;
    using Quartz.Spi;
    using Security;

    public class HareShowJobFactory<TMonitor, TSecurity, TJob> :
        IJobFactory
        where TMonitor : IMonitor
        where TSecurity : ISecurity
        where TJob : IJob
    {
        private readonly Func<TMonitor, TSecurity, TJob> _jobFactory;
        private readonly TMonitor _monitor;
        private readonly TSecurity _security;

        public HareShowJobFactory(TMonitor monitor, TSecurity security)
        {
            _monitor = monitor;
            _security = security;
            _jobFactory = CreateConstructor();
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            if (jobDetail == null)
                throw new SchedulerException("");

            TJob job = _jobFactory(_monitor, _security);

            return job;
        }

        public void ReturnJob(IJob job)
        {
        }

        private Func<TMonitor, TSecurity, TJob> CreateConstructor()
        {
            Type type = typeof(TJob);
            ConstructorInfo ctor = type.GetConstructor(new[] {typeof (TMonitor), typeof (TSecurity)});
            Func<TMonitor, TSecurity, TJob> job = CreateJob(ctor);

            return job;
        }

        private Func<TMonitor, TSecurity, TJob> CreateJob(ConstructorInfo ctor)
        {
            ParameterExpression monitorParam = Expression.Parameter(typeof (TMonitor), "monitor");
            ParameterExpression securityParam = Expression.Parameter(typeof (TSecurity), "security");
            NewExpression obj = Expression.New(ctor, monitorParam, securityParam);

            return Expression.Lambda<Func<TMonitor, TSecurity, TJob>>(obj, monitorParam, securityParam).Compile();
        }
    }
}