
using System;

namespace MDI.Editor
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class EWMsgBoxHandleAttribute : Attribute
	{
		public EWMsgBoxHandleAttribute( Type targetType, int id)
		{
			this.id = id;
			this.targetType = targetType;
		}
		public int id;
		public Type targetType;
	}
}