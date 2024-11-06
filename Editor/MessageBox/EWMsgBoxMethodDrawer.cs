
using System;
using UnityEngine;
using System.Reflection;

namespace EditorWinEx.Internal
{
	internal class EWMsgBoxMethodDrawer : EWMsgBoxDrawer
	{
		public EWMsgBoxMethodDrawer(MethodInfo method, object target, EWRectangle rectangle)
		{
			if( method != null && target != null)
			{
				m_DrawAction = Delegate.CreateDelegate( typeof( DrawActionUseObj), target, method) as DrawActionUseObj;
			}
			m_Rectangle = rectangle;
		}
		protected override bool OnInit()
		{
			return true;
		}
		protected override void OnDrawMsgBox( Rect rect, object obj)
		{
			if( m_DrawAction != null)
			{
				m_DrawAction( rect, obj);
			}
		}
		protected override EWRectangle Rectangle
		{
			get{ return m_Rectangle; }
		}
        readonly DrawActionUseObj m_DrawAction;
		EWRectangle m_Rectangle;
	}
}