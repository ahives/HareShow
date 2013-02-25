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
    using Autofac;
    using Contracts;
    using Topshelf.Runtime;

    public class ObjectContainer :
        IObjectContainer
    {
        public ObjectContainer(HostSettings hostSettings)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance(hostSettings);
            containerBuilder.RegisterType<QueueMonitorService>();
            containerBuilder.RegisterInstance<IObjectContainer>(this);

            Container = containerBuilder.Build();
        }

        public IContainer Container { get; private set; }
    }
}