﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NetMX.Relation;
using NetMX.Remote.Jsr262.Structures;
using WSMan.NET;

namespace NetMX.Remote.Jsr262
{
   public interface IDeserializable
   {
      object Deserialize();
   }

   public partial class NamedGenericValueType
   {
      public NamedGenericValueType()
      {
      }
      public NamedGenericValueType(string name, object value)
         : base(value)
      {
         this.name = name;
      }
   }

   public partial class ParameterType
   {
      public ParameterType()
      {
      }
      public ParameterType(string name, object value)
         : base(value)
      {
         this.name = name;
      }
   }

   public partial class MapType : IDeserializable
   {
      public MapType()
      {
      }
      public MapType(IDictionary value)
      {
         List<MapTypeEntry> mapTypeEntries = new List<MapTypeEntry>();
         foreach (DictionaryEntry entry in value)
         {
            mapTypeEntries.Add(new MapTypeEntry
                                  {
                                     Key = new GenericValueType(entry.Key),
                                     Value = new GenericValueType(entry.Value)
                                  });
         }
         Entry = mapTypeEntries.ToArray();
      }

      public object Deserialize()
      {
         Hashtable results = new Hashtable();
         foreach (MapTypeEntry entry in Entry)
         {
            results.Add(entry.Key.Deserialize(), entry.Value.Deserialize());
         }
         return results;
      }
   }

   public partial class MultipleValueType : IDeserializable
   {
      public MultipleValueType()
      {
      }

      public MultipleValueType(ICollection values)
      {
         List<GenericValueType> valueTypes = new List<GenericValueType>();
         foreach (object value in values)
         {
            valueTypes.Add(new GenericValueType(value));
         }
         Value = valueTypes.ToArray();
      }
      public object Deserialize()
      {
         ArrayList results = new ArrayList();
         foreach (GenericValueType valueType in Value)
         {
            results.Add(valueType.Deserialize());
         }
         return results;
      }
   }   

   public partial class ManagedResourceRoleInfo : IDeserializable
   {
      public ManagedResourceRoleInfo()
      {
         
      }
      public ManagedResourceRoleInfo(RoleInfo roleInfo)
      {
         accessField = "";
         if (roleInfo.Readable)
         {
            accessField += "r";
         }
         if (roleInfo.Writable)
         {
            accessField += "w";
         }
         minDegreeField = roleInfo.MinDegree;
         maxDegreeField = roleInfo.MaxDegree;
         nameField = roleInfo.Name;
         descriptionField = roleInfo.Description;
         managedResourceClassNameField = roleInfo.RefMBeanClassName;
      }

      public object Deserialize()
      {
         return new RoleInfo(nameField, managedResourceClassNameField,
                             accessField.IndexOf('r') != -1,
                             accessField.IndexOf('w') != -1,
                             minDegreeField,
                             maxDegreeField,
                             descriptionField);
      }
   }

   public partial class ManagedResourceRole
   {
      public ManagedResourceRole()
      {         
      }
      public ManagedResourceRole(Role role)
      {
         name = role.Name;
         Value = role.Value.Select(x => new EndpointReference(ObjectNameSelector.CreateEndpointAddress(x))).ToArray();
      }
      public Role Deserialize()
      {
         return new Role(name, Value.Select(x => x.Address.ExtractObjectName()));
      }
   }

   public partial class ManagedResourceRoleUnresolved
   {
      public ManagedResourceRoleUnresolved()
      {         
      }
      public ManagedResourceRoleUnresolved(RoleUnresolved role)
      {
         name = role.RoleName;
         Value = role.RoleValue.Select(x => new EndpointReference(ObjectNameSelector.CreateEndpointAddress(x))).ToArray();
         problemSpecified = true;
         problem = (int)role.ProblemType;
      }
      public RoleUnresolved Deserialize()
      {
         return new RoleUnresolved(name, Value.Select(x => x.Address.ExtractObjectName()), (RoleStatus)problem);
      }
   }

   public partial class ManagedResourceRoleResult : IDeserializable
   {
      public ManagedResourceRoleResult()
      {         
      }
      public ManagedResourceRoleResult(RoleResult roleResult)
      {
         ManagedResourceRoleList = new ManagedResourceRoleList
                                      {
                                         ManagedResourceRole =
                                            roleResult.Roles.Select(x => new ManagedResourceRole(x)).ToArray()
                                      };
         ManagedResourceRoleUnresolvedList = new ManagedResourceRoleUnresolvedList
                                                {
                                                   ManagedResourceRoleUnresolved =
                                                      roleResult.UnresolvedRoles.Select(
                                                      x => new ManagedResourceRoleUnresolved(x)).ToArray()
                                                };
      }

      public object Deserialize()
      {
         return new RoleResult(
            ManagedResourceRoleList.ManagedResourceRole.Select(x => x.Deserialize()),
            ManagedResourceRoleUnresolvedList.ManagedResourceRoleUnresolved.Select(x => x.Deserialize()));
      }
   }       
}
