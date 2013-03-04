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

namespace HareShow.Configuration
{
    using System.Collections.Specialized;
    using System.Configuration;

    public class HareShowConfig :
        IHareShowConfig
    {
        protected NameValueCollection Section { get; set; }

        public string Get(string key)
        {
            return Section.Get(key);
        }

        public void Set(string key, string value)
        {
            Section.Set(key, value);
        }

        public IHareShowConfig GetConfigSection(string sectionName)
        {
            Section = ConfigurationManager.GetSection(sectionName) as NameValueCollection;
            return this;
        }
    }
}