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

namespace HareShow.Monitors
{
    using AutoMapper;
    using HareDu;
    using HareDu.Model;
    using HareDu.Resources;
    using Model;

    public class QueueMonitor :
        IQueueMonitor
    {
        public QueueMonitor(HareDuClient client)
        {
            Client = client;
        }

        private HareDuClient Client { get; set; }

        public QueueStats Get(string username, string password)
        {
            var overview = Client
                .Factory<OverviewResources>(x => x.Credentials(username, password))
                .Get()
                .Data();
            var mapping = Mapper.CreateMap<Overview, QueueStats>()
                                .ForMember(x => x.Messages, x => x.MapFrom(y => y.QueueTotals.Messages))
                                .ForMember(x => x.MessagesReady, x => x.MapFrom(y => y.QueueTotals.MessagesReady))
                                .ForMember(x => x.MessagesUnacknowledged,
                                           x => x.MapFrom(y => y.QueueTotals.MessagesUnacknowledged));
            var queueStats = Mapper.Map<Overview, QueueStats>(overview);

            return queueStats;
        }
    }
}