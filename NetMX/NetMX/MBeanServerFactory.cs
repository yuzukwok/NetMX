#region USING
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Text;
using Simon.Configuration;
using Simon.Configuration.Provider;
#endregion

namespace NetMX
{
   [ConfigurationSection("netMX", DefaultProvider = true)]
   public sealed class MBeanServerFactory : ServiceBase<MBeanServerBuilder>
   {
      private static readonly MBeanServerFactory _instance = new MBeanServerFactory();

      public static IMBeanServer CreateMBeanServer()
      {
         return _instance.Default.NewMBeanServer(null);
      }
      public static IMBeanServer CreateMBeanServer(string instanceName)
      {
         return _instance.Default.NewMBeanServer(instanceName);
      }
   }
}
