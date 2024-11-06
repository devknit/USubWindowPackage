
using System;
using EditorWinEx.Internal;

[AttributeUsage( AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class EWMsgBoxAttribute : Attribute
{
	public EWMsgBoxAttribute( int id, float x = 0.2f, float y = 0.2f, float width = 0.6f, float height = 0.6f)
	{
		this.id = id;
		Rectangle = new EWRectangle(x, y, width, height);
	}
	public EWMsgBoxAttribute( int id, float x, float y, float z, float w, bool anchorLeft, bool anchorRight, bool anchorTop, bool anchorBottom)
	{
		this.id = id;
		Rectangle = new EWRectangle(x, y, z, w, anchorLeft, anchorRight, anchorTop, anchorBottom);
	}
	public EWRectangle Rectangle
	{
		get; private set;
	}
	public int id;
}