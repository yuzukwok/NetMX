﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetMX.Remote.Jsr262
{
   public sealed class Jsr262ConnectorProvider : NetMXConnectorProvider
   {      
      public override INetMXConnector NewNetMXConnector(Uri serviceUrl)
      {
         return new Jsr262Connector(serviceUrl);
      }
   }
}