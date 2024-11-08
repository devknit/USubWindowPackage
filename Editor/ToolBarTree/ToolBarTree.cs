
using System;
using System.Reflection;
using MDI.Editor.Internal;

namespace MDI.Editor
{
	public class ToolBarTree : EditorWindowComponentBase
	{
		public int count
		{
			get
			{
				if (m_Root == null)
				{
					return 0;
				}
				return m_Root.count;
			}
		}
		ToolBarTreeNode m_Root;
		
		public ToolBarTree()
		{
		}
		public void InsertItem( string text, MethodInfo method, object target, int priority)
		{
			if( method == null)
			{
				return;
			}
			if( method.GetParameters().Length != 0)
			{
				return;
			}
			if( string.IsNullOrEmpty(text) != false)
			{
				return;
			}
			if( m_Root == null)
			{
				m_Root = new ToolBarTreeNode( "", 0);
			}
			m_Root.InsertNode( text, method, target, null, null, priority);
		}
		public void InsertItem( string text, Delegate method, int priority, ConditionDelegate condition, object obj)
		{
			if( method == null)
			{
				return;
			}
			int parameterslen = method.Method.GetParameters().Length;
			
			if( parameterslen > 1)
			{
				return;
			}
			if( parameterslen == 1 && obj == null)
			{
				return;
			}
			if( parameterslen == 0 && obj != null)
			{
				return;
			}
			if( string.IsNullOrEmpty( text) != false)
			{
				return;
			}
			if( m_Root == null)
			{
				m_Root = new ToolBarTreeNode( "", 0);
			}
			m_Root.InsertNode( text, method.Method, method.Target, condition, obj, priority);
		}
		public void Sort()
		{
			if( m_Root != null)
			{
				m_Root.Sort();
			}
		}
		public void DrawToolbar()
		{
			if( m_Root != null)
			{
				m_Root.DrawToolBar();
			}
		}
		protected override void OnRegisterMethod( object container, MethodInfo method, object target)
		{
			object[] atts = method.GetCustomAttributes( typeof( EWToolBarAttribute), false);
			ParameterInfo[] parameters = method.GetParameters();
			
			if( atts != null && parameters.Length == 0)
			{
				for (int i1 = 0; i1 < atts.Length; ++i1)
				{
					EWToolBarAttribute att = (EWToolBarAttribute)atts[ i1];
					InsertItem( att.menuItem, method, target, att.priority);
				}
			}
		}
		protected override void OnRegisterClass( object container, Type type)
		{
		}
		protected override void OnInit()
		{
			Sort();
		}
	}
}