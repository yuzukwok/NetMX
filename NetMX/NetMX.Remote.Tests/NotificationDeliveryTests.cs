﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace NetMX.Remote.Tests
{
   [TestFixture]
   public abstract class NotificationDeliveryTests
   {      
      [Test]
      public void When_publishing_notification_original_handback_object_is_passed_to_callback_method()
      {
         //Arrange
          object handback = new object();
          ManualResetEvent notificationFlag = new ManualResetEvent(false);
         _remoteServer.AddNotificationListener("Tests:key=value", (n, h) =>
                                                                     {
                                                                        Assert.AreSame(handback, h);
                                                                        notificationFlag.Set();
                                                                     }, null, handback);
         
         //Act
         _bean.AddAmount(3);

         //Assert
         if (!notificationFlag.WaitOne())
         {
            Assert.Fail();
         }
      }

      [Test]
      public void When_subscribing_twice_with_different_handbacks_both_subscriptions_created()
      {
         //Arrange
         object handback = new object();
         ManualResetEvent notificationFlag = new ManualResetEvent(false);
         object handback2 = new object();
         ManualResetEvent notificationFlag2 = new ManualResetEvent(false);
         _remoteServer.AddNotificationListener("Tests:key=value", (n, h) =>
                                                                     {
                                                                        Assert.AreSame(handback, h);
                                                                        notificationFlag.Set();
                                                                     }, null, handback);
         _remoteServer.AddNotificationListener("Tests:key=value", (n, h) =>
                                                                     {
                                                                        Assert.AreSame(handback2, h);
                                                                        notificationFlag2.Set();
                                                                     }, null, handback2);

         //Act
         _bean.AddAmount(3);
         
         //Assert
         if (!notificationFlag.WaitOne() || !notificationFlag2.WaitOne())
         {
            Assert.Fail();
         }
      }

      [Test]
      public void When_subscribing_twice_with_same_handback_both_subscriptions_created()
      {
         //Arrange
         object handback = new object();
         ManualResetEvent notificationFlag = new ManualResetEvent(false);
         ManualResetEvent notificationFlag2 = new ManualResetEvent(false);
         _remoteServer.AddNotificationListener("Tests:key=value", (n, h) =>
                                                                     {
                                                                        Assert.AreSame(handback, h);
                                                                        notificationFlag.Set();
                                                                     }, null, handback);
         _remoteServer.AddNotificationListener("Tests:key=value", (n, h) =>
                                                                     {
                                                                        Assert.AreSame(handback, h);
                                                                        notificationFlag2.Set();
                                                                     }, null, handback);

         //Act
         _bean.AddAmount(3);

         //Assert
         if (!notificationFlag.WaitOne() || !notificationFlag2.WaitOne())
         {
            Assert.Fail();
         }
      }

      [Test]
      public void When_publishing_notification_type_is_set_corretly()
      {
         //Arrange
         ManualResetEvent notificationFlag = new ManualResetEvent(false);
         _remoteServer.AddNotificationListener("Tests:key=value", (n, h) =>
                                                                     {
                                                                        Assert.AreEqual("sample.counter",n.Type);
                                                                        notificationFlag.Set();
                                                                     }, null, null);

         //Act
         _bean.AddAmount(3);

         //Assert
         if (!notificationFlag.WaitOne())
         {
            Assert.Fail();
         }
      }

      [Test]
      public void When_publishing_notification_message_is_set_corretly()
      {
         //Arrange
         ManualResetEvent notificationFlag = new ManualResetEvent(false);
         _remoteServer.AddNotificationListener("Tests:key=value", (n, h) =>
                                                                     {
                                                                        Assert.AreEqual("Counter changed", n.Message);
                                                                        notificationFlag.Set();
                                                                     }, null, null);

         //Act
         _bean.AddAmount(3);

         //Assert
         if (!notificationFlag.WaitOne())
         {
            Assert.Fail();
         }
      }

      [Test]
      public void When_publishing_notification_user_data_is_set_corretly()
      {
         //Arrange
         ManualResetEvent notificationFlag = new ManualResetEvent(false);
         _remoteServer.AddNotificationListener("Tests:key=value", (n, h) =>
                                                                     {
                                                                        Assert.AreEqual(3, n.UserData);
                                                                        notificationFlag.Set();
                                                                     }, null, null);

         //Act
         _bean.AddAmount(3);

         //Assert
         if (!notificationFlag.WaitOne())
         {
            Assert.Fail();
         }
      }

      private SimpleStandard _bean;
      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      private INetMXConnector _connector;
      private IMBeanServerConnection _remoteServer;

      [SetUp]
      public void SetUp()
      {
         _server = MBeanServerFactory.CreateMBeanServer();
         _bean = new SimpleStandard();
         ObjectName name = new ObjectName("Tests:key=value");
         _server.RegisterMBean(_bean, name);
         Uri serviceUrl = GetUri();

         _connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, _server);
         _connectorServer.Start();
         _connector = NetMXConnectorFactory.Connect(serviceUrl, null);
         _remoteServer = _connector.MBeanServerConnection;
      }

      protected abstract Uri GetUri();

      [TearDown]
      public void TearDown()
      {
         _connector.Dispose();
         _connectorServer.Dispose();
      }
   }
}