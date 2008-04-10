using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean
{
	[Serializable]
	public sealed class TabularType : OpenType
	{
		public TabularType(string typeName, string description, CompositeType rowType, IEnumerable<string> indexNames)
			: base(OpenTypeRepresentation.Tabular, typeName, description)
		{
		}

		public override bool IsValue(object value)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}