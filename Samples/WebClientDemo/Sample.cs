#region USING
using System;
using System.Collections.Generic;
using System.Text;
using NetMX.OpenMBean;
using NetMX;

#endregion

namespace WebClientDemo
{
   public class Sample : SampleMBean
   {
      #region MEMBERS
      private int _counter;
      private int _step;
      #endregion

      #region SampleMBean Members
      public int Step
      {
         get { return _step; }
         set { _step = value; }
      }
      public int Counter
      {
         get
         {
            return _counter;
         }			
      }
      public void Increment()
      {
         _counter += _step;
      }
      public void ResetCounter()
      {
         _counter = 0;
      }
      public void AddAmount(int amount)
      {
         _counter += amount;
      }
      #endregion
   }

   [OpenMBean]
   public interface SampleMBean
   {
      [OpenMBeanAttributeAttribute(DefaultValue = 1, MinValue = 0, MaxValue = 5)]
      int Step { get; set; }
      int Counter { get; }
      [OpenMBeanOperation(OperationImpact.Action)]
      void Increment();
      [OpenMBeanOperation(OperationImpact.Action)]
      void ResetCounter();
      [OpenMBeanOperation(OperationImpact.Action)]
      void AddAmount([OpenMBeanParameter(DefaultValue = 2, LegalValues = new object[] {1,2,3})] int amount);
   }
}