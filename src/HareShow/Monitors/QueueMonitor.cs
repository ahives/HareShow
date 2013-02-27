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
    using Contracts;
    using HareDu;
    using HareDu.Model;
    using HareDu.Resources;
    using Model;
    using MongoDB.Driver;

    public class QueueMonitor :
        IQueueMonitor
    {
        public QueueMonitor(HareDuClient client)
        {
            Client = client;
        }

        private HareDuClient Client { get; set; }

        public Stats Get(string username, string password)
        {
            var overview = Client
                .Factory<OverviewResources>(x => x.Credentials(username, password))
                .Get()
                .Data();
            var mapping = Mapper.CreateMap<Overview, Stats>()
                                .ForMember(x => x.Node, x => x.MapFrom(y => y.Node))
                                .ForMember(x => x.MessageStats, x => x.MapFrom(y => y.MessageStats))
                                .ForMember(x => x.QueueTotals, x => x.MapFrom(y => y.QueueTotals))
                                .ForMember(x => x.Listeners, x => x.MapFrom(y => y.Listeners));
            var heartbeat = Mapper.Map<Overview, Stats>(overview);

            return heartbeat;
        }

        public void Save(Stats stats)
        {
            string connectionString = "";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("");
            var collection = database.GetCollection<Stats>("");
            collection.Save(stats);
        }
    }
}