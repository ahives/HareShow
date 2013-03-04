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
    using System.Collections.Generic;
    using AutoMapper;
    using HareDu;
    using HareDu.Model;
    using HareDu.Resources;
    using Model;

    public class ChannelMonitor :
        IChannelMonitor
    {
        public ChannelMonitor(HareDuClient client)
        {
            Client = client;
        }

        private HareDuClient Client { get; set; }

        public IEnumerable<ChannelStats> Get(string username, string password)
        {
            var channels = Client
                .Factory<ConnectionResources>(x => x.Credentials(username, password))
                .Channel
                .GetAll()
                .Data();

            var mapping = Mapper.CreateMap<Channel, ChannelStats>()
                                .ForMember(x => x.Acknowledged, x => x.MapFrom(y => y.MessageStats.Acknowledged))
                                .ForMember(x => x.AcknowledgesUncommitted,
                                           x => x.MapFrom(y => y.AcknowledgesUncommitted))
                                .ForMember(x => x.Confirm, x => x.MapFrom(y => y.Confirm))
                                .ForMember(x => x.ConsumerCount, x => x.MapFrom(y => y.ConsumerCount))
                                .ForMember(x => x.Delivered, x => x.MapFrom(y => y.MessageStats.Delivered))
                                .ForMember(x => x.DeliveredOrGet, x => x.MapFrom(y => y.MessageStats.DeliveredOrGet))
                                .ForMember(x => x.IdleSince, x => x.MapFrom(y => y.IdleSince))
                                .ForMember(x => x.PrefetchCount, x => x.MapFrom(y => y.PrefetchCount))
                                .ForMember(x => x.Published, x => x.MapFrom(y => y.MessageStats.Published))
                                .ForMember(x => x.Unacknowledged, x => x.MapFrom(y => y.Unacknowledged))
                                .ForMember(x => x.Uncommitted, x => x.MapFrom(y => y.Uncommitted))
                                .ForMember(x => x.Unconfirmed, x => x.MapFrom(y => y.Unconfirmed));
            var channelStats = Mapper.Map<IEnumerable<Channel>, IEnumerable<ChannelStats>>(channels);

            return channelStats;
        }
    }
}