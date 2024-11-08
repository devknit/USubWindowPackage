using System;

namespace MDI.Editor
{
	[AttributeUsage( AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class EWToolBarAttribute : Attribute
	{
		public EWToolBarAttribute( string menuItem, int priority = 1000)
		{
			this.menuItem = menuItem;
			this.priority = priority;
		}
		public string menuItem;
		public int priority;
	}
}