
using UnityEngine;
using EditorWinEx;
using EditorWinEx.Internal;
using System;
using System.Reflection;
using System.Collections.Generic;

public delegate void DrawActionUseObj( Rect rect, object obj);

public class EditorWindowMsgBox : EditorWindowComponentBase
{
	public bool IsShowing
	{
		get { return m_IsShowing; }
	}
	public void AddMsgBox( int id, MethodInfo method, object target, EWRectangle rectangle)
	{
		if( m_MsgBoxs.ContainsKey( id) != false)
		{
			Debug.LogError("Error, MsgBox method that already contains the ID:" + id);
			return;
		}
		var msgbox = new EWMsgBoxMethodDrawer( method, target, rectangle);
		msgbox.Init();
		m_MsgBoxs.Add( id, msgbox);
	}
	public void AddMsgBox( int id, EWMsgBoxCustomDrawer drawer)
	{
		if( m_MsgBoxs.ContainsKey( id) != false)
		{
			Debug.LogError("Error, MsgBox method that already contains the ID:" + id);
			return;
		}
		var msgbox = new EWMsgBoxObjectDrawer( drawer);
		msgbox.Init();
		m_MsgBoxs.Add( id, msgbox);
	}
	public void DrawMsgBox( Rect rect)
	{
		if( m_IsShowing == false)
		{
			return;
		}
		if( m_MsgBoxs.ContainsKey( m_CurrentShowId) == false)
		{
			return;
		}
		var msgBox = m_MsgBoxs[ m_CurrentShowId];
		msgBox.DrawMsgBox( rect, m_Obj);
		return;
	}
	public void ShowMsgBox( int id, object obj)
	{
		if( m_MsgBoxs.ContainsKey(id) != false)
		{
			m_Obj = obj;
			m_CurrentShowId = id;
			m_IsShowing = true;
			m_MsgBoxs[ id].Enable();
		}
	}
	public void HideMsgBox()
	{
		m_IsShowing = false;
		
		if( m_MsgBoxs.ContainsKey( m_CurrentShowId) != false)
		{
			m_MsgBoxs[ m_CurrentShowId].Disable();
		}
	}
	protected override void OnRegisterMethod( object container, MethodInfo method, object target)
	{
		object[] atts = method.GetCustomAttributes(typeof(EWMsgBoxAttribute), false);
		ParameterInfo[] parameters = method.GetParameters();
		
		if( atts != null && parameters.Length == 2 && parameters[ 0].ParameterType == typeof( Rect) && parameters[ 1].ParameterType == typeof( object))
		{
			for( int i1 = 0; i1 < atts.Length; ++i1)
			{
				EWMsgBoxAttribute att = (EWMsgBoxAttribute)atts[ i1];
				AddMsgBox( att.id, method, target, att.Rectangle);
			}
		}
	}
	protected override void OnRegisterClass( object container, Type type)
	{
		if( container == null)
		{
			return;
		}
		if( type.IsSubclassOf( typeof( EWMsgBoxCustomDrawer)) == false)
		{
			return;
		}
		object[] atts = type.GetCustomAttributes( typeof( EWMsgBoxHandleAttribute), false);
		
		for( int i0 = 0; i0 < atts.Length; ++i0)
		{
			EWMsgBoxHandleAttribute att = (EWMsgBoxHandleAttribute)atts[ i0];
			if( att == null)
			{
				continue;
			}
			if( att.targetType != container.GetType())
			{
				continue;
			}
			EWMsgBoxCustomDrawer drawer = (EWMsgBoxCustomDrawer)Activator.CreateInstance( type);
			
			if( drawer == null)
			{
				continue;
			}
			drawer.SetContainer( container);
			drawer.closeAction = HideMsgBox;
			AddMsgBox( att.id, drawer);
		}
	}
	protected override void OnInit()
	{
	}
	protected override void OnDestroy()
	{
		foreach( var kvp in m_MsgBoxs)
		{
			if( kvp.Value != null)
			{
				kvp.Value.Destroy();
			}
		}
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		
		foreach( var kvp in m_MsgBoxs)
		{
			if( kvp.Value != null)
			{
				kvp.Value.Serialize();
			}
		}
	}
    readonly Dictionary<int, EWMsgBoxDrawer> m_MsgBoxs = new();
	bool m_IsShowing;
	int m_CurrentShowId = -1;
	object m_Obj;
}
