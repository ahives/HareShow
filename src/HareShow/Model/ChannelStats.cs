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

namespace HareShow.Model
{
    using System;

    public class ChannelStats
    {
        public int ConsumerCount { get; set; }
        public int PrefetchCount { get; set; }
        public DateTime IdleSince { get; set; }
        public bool Confirm { get; set; }
        public int Published { get; set; }
        public int Acknowledged { get; set; }
        public int Delivered { get; set; }
        public int DeliveredOrGet { get; set; }
        public int Unacknowledged { get; set; }
        public int Unconfirmed { get; set; }
        public int Uncommitted { get; set; }
        public int AcknowledgesUncommitted { get; set; }
    }
}