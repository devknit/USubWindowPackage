
using UnityEngine;
using System.Xml;
using System.Collections.Generic;

namespace EditorWinEx.Internal
{
	internal class SubWindowLeaf : SubWindowNode
	{
		public override int Count
		{
			get{ return m_SubWindows.Count; }
		}
		public SubWindowLeaf( SubWindow window, bool isHorizontal, int depth) : base( isHorizontal, depth)
		{
			if( window != null)
			{
				window.Open();
				m_SubWindows.Add( window);
			}
			if( window == null)
			{
                m_TweenParam = new GUITweenParam( false)
                {
                    tweenTime = 1
                };
            }
			else
			{
				m_TweenParam = new GUITweenParam( true);
			}
		}
		public override void DrawGUI( Rect rect, System.Action repaintAction)
		{
			rect = new Rect( rect.x + 2, rect.y, rect.width - 4, rect.height - 2);
			
			if( m_TweenParam.isTweening != false)
			{
				m_TweenParam = GUIEx.ScaleTweenBox( rect, m_TweenParam, 
					m_SubWindows[ m_SelectSubWindow].Title, GUIStyleCache.GetStyle( "dragtabdropwindow"));
				
				if( repaintAction != null)
				{
					repaintAction();
				}
				return;
			}
			GUI.BeginGroup( rect, GUIStyleCache.GetStyle( "PreBackground"));
			GUI.Box( new Rect( 0, 0, rect.width, 18), string.Empty, GUIStyleCache.GetStyle( "dockarea"));
			
			if( m_SelectSubWindow >= 0 && m_SelectSubWindow < m_SubWindows.Count)
			{
				GUI.Label( new Rect( m_SelectSubWindow * 110, 0, rect.width - m_SelectSubWindow * 110, 18),
					m_SubWindows[ m_SelectSubWindow].Title, GUIStyleCache.GetStyle( "dragtabdropwindow"));
			}
			for( int i0 = 0; i0 < m_SubWindows.Count; ++i0)
			{
				if( m_SelectSubWindow != i0)
				{
					if( GUI.Button( new Rect( i0 * 110, 0, 110, 17), m_SubWindows[ i0].Title, GUIStyleCache.GetStyle( "dragtab")) != false)
					{
						m_SelectSubWindow = i0;
					}
				}
			}
			GUI.Box( new Rect( 0, 18, rect.width, rect.height - 18), string.Empty, GUIStyleCache.GetStyle( "hostview"));
			
			if( m_SelectSubWindow >= 0 && m_SelectSubWindow < m_SubWindows.Count)
			{
				GUI.BeginGroup( new Rect( 0, 18, rect.width, rect.height - 18));
				m_SubWindows[ m_SelectSubWindow].DrawSubWindow( new Rect( 0, 0, rect.width, rect.height - 18));
				GUI.EndGroup();
				
				if( repaintAction != null)
				{
					repaintAction();
				}
			}
			if( m_SelectSubWindow >= 0 && m_SelectSubWindow < m_SubWindows.Count)
			{
				m_SubWindows[ m_SelectSubWindow].DrawToolBarExt( new Rect( rect.width - 100, 0, 100, 18));
			}
			GUI.EndGroup();
			this.rect = rect;
		}
		public override bool ContainWindow( SubWindow window)
		{
			return m_SubWindows.Contains( window);
		}
		public override void AddWindow( SubWindow window, int index)
		{
			m_SubWindows.Add( window);
			m_SelectSubWindow = m_SubWindows.Count - 1;
		}
		public override void Insert( SubWindowNode node, int index)
		{
		}
		public override bool RemoveWindow( SubWindow window)
		{
			for( int i0 = 0; i0 < m_SubWindows.Count; ++i0)
			{
				if( m_SubWindows[ i0] == window)
				{
					m_SubWindows.Remove( window);
					m_SelectSubWindow = 0;
					return true;
				}
			}
			return false;
		}
		public override void ClearEmptyNode()
		{
		}
		public override void WriteToLayoutCfg( XmlElement element, XmlDocument document, int index)
		{
			XmlElement currentElement = document.CreateElement( "SubWindowLeaf");
			currentElement.SetAttribute( "Weight", weight.ToString());
			currentElement.SetAttribute( "Depth", depth.ToString());
			currentElement.SetAttribute( "Horizontal", isHorizontal.ToString());
			currentElement.SetAttribute( "Index", index.ToString());
			element.AppendChild( currentElement);
			
			if( m_SubWindows != null)
			{
				for( int i0 = 0; i0 < m_SubWindows.Count; ++i0)
				{
					XmlElement windowElement = document.CreateElement( "SubWindow");
					windowElement.SetAttribute( "ID", m_SubWindows[ i0].GetIndentifier());
					windowElement.SetAttribute( "Index", i0.ToString());
					currentElement.AppendChild( windowElement);
				}
			}
		}
		public override void CreateFromLayoutCfg( XmlElement node, List<SubWindow> windowList, System.Action<SubWindow> onWindowClose)
		{
			string weightStr = node.GetAttribute( "Weight");
			string depthStr = node.GetAttribute( "Depth");
			string horizontalStr = node.GetAttribute( "Horizontal");
			weight = float.Parse( weightStr);
			depth = int.Parse( depthStr);
			isHorizontal = bool.Parse( horizontalStr);
			XmlNodeList nodes = node.ChildNodes;
			
			if (nodes.Count == 0)
			{
				return;
			}
			var sortnode = new XmlElement[ nodes.Count];
			
			foreach( var n in nodes)
			{
				var nd = n as XmlElement;
				string indexstr = nd.GetAttribute( "Index");
				int index = int.Parse( indexstr);
				sortnode[ index] = nd;
			}
			foreach( var n in sortnode)
			{
				string id = n.GetAttribute( "ID");
				var window = windowList.Find( w => w.GetIndentifier() == id);
				
				if( window == null)
				{
					continue;
				}
				window.Open();
				window.AddCloseEventListener( onWindowClose);
				m_SubWindows.Add( window);
			}
		}
		public override SubWindow DragWindow( Vector2 position)
		{
			if( m_TweenParam.isTweening != false)
			{
				return null;
			}
			if( m_SelectSubWindow < 0 || m_SelectSubWindow >= m_SubWindows.Count)
			{
				return null;
			}
			var rect = new Rect( this.rect.x + m_SelectSubWindow * 100, this.rect.y, 100, 17);
			
			if( rect.Contains( position) != false)
			{
				return m_SubWindows[ m_SelectSubWindow];
			}
			return null;
		}
		public override void Recalculate( int depth, bool isHorizontal)
		{
			this.depth = depth;
			this.isHorizontal = isHorizontal;
		}
		protected override bool TriggerAnchorArea( Vector2 position, int depth, SubWindow window, System.Action<SubWindow> preDropAction, System.Action postDropAction)
		{
			if( m_TweenParam.isTweening)
			{
				return false;
			}
			if( depth >= kMaxNodeDepth)
			{
				return false;
			}
			if( m_SubWindows.Count >= kMaxSubWindowCount)
			{
				return false;
			}
			if( this.m_SubWindows.Contains(window) != false)
			{
				return false;
			}
			var rect = new Rect( this.rect.x, this.rect.y, this.rect.width, 17);
			
			if( rect.Contains( position) != false)
			{
				if( preDropAction == null)
				{
					tweenParam = GUIEx.ScaleTweenBox( rect, tweenParam, string.Empty, GUIStyleCache.GetStyle( "SelectionRect"));
				}
				else
				{
					if( preDropAction != null)
					{
						preDropAction( window);
						this.AddWindow( window, 0);
						postDropAction();
					}
				}
				return true;
			}
			return false;
		}
		const int kMaxSubWindowCount = 3;
        readonly List<SubWindow> m_SubWindows = new();
		int m_SelectSubWindow;
		GUITweenParam m_TweenParam;
	}
}