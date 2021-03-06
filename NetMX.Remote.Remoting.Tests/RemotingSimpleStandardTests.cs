﻿using System;
using NetMX.Remote.Remoting.Internal;
using NetMX.Remote.Tests;
using NUnit.Framework;

namespace NetMX.Remote.Remoting.Tests
{
    [TestFixture]
    public class RemotingSimpleStandardTests : SimpleStandardTests
    {
        protected override Uri GetUri()
        {
            return new Uri("tcp://localhost:1234/MBeanServer.tcp");
        }

        protected override INetMXConnectorServerFactory GetConnectorServerFactory()
        {
            return new RemotingConnectorServerFactory(100, new NullSecurityProvider());
        }

        protected override INetMXConnectorFactory GetConnectorFactory()
        {
            return new RemotingConnectorFactory(new NotificationFetcherConfig(false, TimeSpan.FromSeconds(1)));
        }
    }
}