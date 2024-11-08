
using System;

public enum SubWindowStyle
{
	Default,
	Preview,
	Grid,
}
namespace EditorWinEx
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class SubWindowStyleAttribute : Attribute
	{
		public SubWindowStyleAttribute( SubWindowStyle type)
		{
			subWindowStyle = type;
		}
		public SubWindowStyle subWindowStyle;
	}
}