using System;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class EWSubWindowHandleAttribute : Attribute
{
	public EWSubWindowHandleAttribute( Type containerType, SubWindowStyle windowStyle = SubWindowStyle.Default, bool active = true)
	{
		this.containerType = containerType;
		this.windowStyle = windowStyle;
		this.active = active;
	}
	public Type containerType;
	public SubWindowStyle windowStyle;
	public bool active;
}
