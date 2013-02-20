﻿// Copyright 2013-2014 Albert L. Hives
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
    using Topshelf;
    using Topshelf.Runtime;

    internal class Program
    {
        private static int Main(string[] args)
        {
            return (int) HostFactory.Run(x =>
                                             {
                                                 x.SetServiceName("");
                                                 x.SetDisplayName("");
                                                 x.SetDescription("");
                                                 x.UseLog4Net("log4net.config");
                                                 x.Service(CreateHareShowService);
                                             });
        }

        private static ServiceControl CreateHareShowService(HostSettings arg)
        {
            throw new System.NotImplementedException();
        }
    }
}