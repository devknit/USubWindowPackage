using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections.Generic;

namespace MDI.Editor.Internal
{
	internal class SubWindowNode
	{
		public virtual int Count
		{
			get{ return m_Childs.Count; }
		}
		public int Depth
		{
			get { return depth; }
		}
		public bool IsHorizontal
		{
			get { return isHorizontal; }
		}
		public SubWindowNode( bool horizontal, int depth)
		{
			this.isHorizontal = horizontal;
			this.depth = depth;
		}
		public virtual void DrawGUI( Rect rect, System.Action repaintAction)
		{
			float offset = 0;
			//Rect resizeRect = default(Rect);
			if( isHorizontal != false)
			{
				for( int i0 = 0; i0 < m_Childs.Count; ++i0)
				{
					if( i0 > 0)
					{
						Resize( i0 - 1, i0, new Rect( rect.x + offset - 2, rect.y, 4, rect.height));
					}
					int w = (int)(rect.width * m_Childs[ i0].weight);
					m_Childs[ i0].DrawGUI( new Rect( rect.x + offset, rect.y, w, rect.height), repaintAction);
					
					if( i0 >= 0 && i0 < m_Childs.Count)
					{
						offset += w;
					}
				}
			}
			else
			{
				for( int i0 = 0; i0 < m_Childs.Count; ++i0)
				{
					if( i0 > 0)
					{
						Resize( i0 - 1, i0, new Rect( rect.x, rect.y + offset - 2, rect.width, 4));
					}
					int h = (int)(rect.height * m_Childs[ i0].weight);
					m_Childs[ i0].DrawGUI( new Rect( rect.x, rect.y + offset, rect.width, h), repaintAction);
					
					if( i0 >= 0 && i0 < m_Childs.Count)
					{
						offset += h;
					}
				}
			}
			DoResize(repaintAction);
			this.rect = rect;
		}
		public virtual SubWindow DragWindow( Vector2 position)
		{
			for( int i0 = 0; i0 < m_Childs.Count; ++i0)
			{
				var r = m_Childs[ i0].DragWindow( position);
				
				if( r != null)
				{
					return r;
				}
			}
			return null;
		}
		public void DropWindow( Vector2 position, int depth, SubWindow window, System.Action<SubWindow> preDropAction, System.Action postDropAction)
		{
			TriggerAnchorArea( position, depth, window, preDropAction, postDropAction);
		}
		public void DrawAnchorArea( Vector2 position, int depth, SubWindow window)
		{
			TriggerAnchorArea( position, depth, window, null, null);
		}
		public virtual bool ContainWindow( SubWindow window)
		{
			if( m_Childs.Count == 1)
			{
				return m_Childs[ 0].ContainWindow( window);
			}
			return false;
		}
		public virtual void Insert( SubWindowNode node, int index)
		{
			if( this.m_Childs.Count == 0)
			{
				node.weight = 1;
				node.depth = depth + 1;
				node.isHorizontal = !isHorizontal;
				this.m_Childs.Add( node);
				return;
			}
			float w = 1.0f / (m_Childs.Count + 1);
			float ew = w / m_Childs.Count;
			node.weight = w;
			node.depth = depth + 1;
			node.isHorizontal = !isHorizontal;
			
			for( int i0 = 0; i0 < m_Childs.Count; ++i0)
			{
				m_Childs[ i0].weight -= ew;
			}
			if( index < 0 || index >= m_Childs.Count)
			{
				m_Childs.Add( node);
			}
			else
			{
				m_Childs.Insert( index, node);
			}
		}
		public virtual bool RemoveWindow( SubWindow window)
		{
			for( int i0 = 0; i0 < m_Childs.Count; ++i0)
			{
				bool result = m_Childs[ i0].RemoveWindow( window);
				
				if( result != false)
				{
					return true;
				}
			}
			return false;
		}
		public virtual void ClearEmptyNode()
		{
			for( int i0 = 0; i0 < m_Childs.Count; ++i0)
			{
				m_Childs[ i0].ClearEmptyNode();
				
				if( m_Childs[ i0].Count == 0)
				{
					float w = 1.0f / m_Childs.Count;
					m_Childs.RemoveAt( i0);
					float ew = w / m_Childs.Count;
					
					for( int i1 = 0; i1 < m_Childs.Count; ++i1)
					{
						m_Childs[ i1].weight += ew;
					}
				}
			}
		}
		public virtual void AddWindow( SubWindow window, int index)
		{
			Insert( new SubWindowLeaf( window, !isHorizontal, depth + 1), index);
		}
		public virtual void Recalculate( int depth, bool isHorizontal)
		{
			this.depth = depth;
			this.isHorizontal = isHorizontal;
			
			if( m_Childs.Count == 0)
			{
				return;
			}
			float weightSum = 0;
			
			for( int i0 = 0; i0 < m_Childs.Count; ++i0)
			{
				if( m_Childs[ i0].weight < kMinWeight)
				{
					m_Childs[ i0].weight = kMinWeight;
				}
				else if( m_Childs[ i0].weight > kMaxWeight)
				{
					m_Childs[ i0].weight = kMaxWeight;
				}
				weightSum += m_Childs[ i0].weight;
				m_Childs[ i0].Recalculate( depth + 1, !isHorizontal);
			}
			if( weightSum > 1.0f + Mathf.Epsilon || weightSum < 1.0f - Mathf.Epsilon)
			{
				float m = (1 - weightSum) / m_Childs.Count;
				
				for( int i0 = 0; i0 < m_Childs.Count; i0++)
				{
					m_Childs[ i0].weight += m;
				}
			}
		}
		public virtual void WriteToLayoutCfg( XmlElement element, XmlDocument document, int index)
		{
			XmlElement currentElement = document.CreateElement( "SubWindowNode");
			currentElement.SetAttribute( "Weight", weight.ToString());
			currentElement.SetAttribute( "Depth", depth.ToString());
			currentElement.SetAttribute( "Horizontal", isHorizontal.ToString());
			currentElement.SetAttribute( "Index", index.ToString());
			element.AppendChild( currentElement);
			
			if( m_Childs != null)
			{
				for( int i0 = 0; i0 < m_Childs.Count; ++i0)
				{
					m_Childs[ i0].WriteToLayoutCfg( currentElement, document, i0);
				}
			}
		}
		public virtual void CreateFromLayoutCfg( XmlElement node, List<SubWindow> windowList, System.Action<SubWindow> onWindowClose)
		{
			string weightStr = node.GetAttribute( "Weight");
			string depthStr = node.GetAttribute( "Depth");
			string horizontalStr = node.GetAttribute( "Horizontal");
			weight = float.Parse( weightStr);
			depth = int.Parse( depthStr);
			isHorizontal = bool.Parse( horizontalStr);
			XmlNodeList nodes = node.ChildNodes;
			
			if( nodes.Count == 0)
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
				if( n.Name == "SubWindowNode")
				{
					var swnode = new SubWindowNode( true, 0);
					swnode.CreateFromLayoutCfg( n, windowList, onWindowClose);
					m_Childs.Add( swnode);
				}
				else if( n.Name == "SubWindowLeaf")
				{
					var swleaf = new SubWindowLeaf( null, true, 0);
					swleaf.CreateFromLayoutCfg( n, windowList, onWindowClose);
					m_Childs.Add( swleaf);
				}
			}
		}
		protected virtual bool TriggerAnchorArea( Vector2 position, int depth, SubWindow window, System.Action<SubWindow> preDropAction, System.Action postDropAction)
		{
			if (depth >= kMaxNodeDepth)
			{
				return false;
			}
			Rect r = default;
			float offset = 0;
			
			for( int i0 = 0; i0 < m_Childs.Count; i0++)
			{
				if( m_Childs[ i0].TriggerAnchorArea( position, depth + 1, window, preDropAction, postDropAction))
				{
					return true;
				}
				if( isHorizontal != false)
				{
					r = new Rect( rect.x + offset, rect.y, rect.width * m_Childs[ i0].weight * 0.2f, rect.height);
					
					if( r.Contains( position) != false)
					{
						if( preDropAction == null)
						{
							tweenParam = GUIEx.ScaleTweenBox( r, tweenParam, string.Empty, GUIStyleCache.GetStyle( "SelectionRect"));
						}
						else
						{
							DropWindow( window, preDropAction, postDropAction, true, i0);
						}
						return true;
					}
					r = new Rect( 
						rect.x + offset + rect.width * m_Childs[ i0].weight * 0.8f, 
						rect.y, rect.width * m_Childs[ i0].weight * 0.2f, rect.height);
					
					if( r.Contains( position) != false)
					{
						if( preDropAction == null)
						{
							tweenParam = GUIEx.ScaleTweenBox( r, tweenParam, string.Empty, GUIStyleCache.GetStyle( "SelectionRect"));
						}
						else
						{
							DropWindow(window, preDropAction, postDropAction, true, i0 + 1);
						}
						return true;
					}
					r = new Rect( rect.x + offset, rect.y + rect.height * 0.8f, rect.width * m_Childs[ i0].weight, rect.height * 0.2f);
					
					if( r.Contains( position) != false && m_Childs.Count > 1)
					{
						if( preDropAction == null)
						{
							tweenParam = GUIEx.ScaleTweenBox( r, tweenParam, string.Empty, GUIStyleCache.GetStyle( "SelectionRect"));
						}
						else
						{
							DropWindow( window, preDropAction, postDropAction, false, i0);
						}
						return true;
					}
					offset += m_Childs[ i0].weight*rect.width;
				}
				else
				{
					r = new Rect(rect.x, rect.y + offset, rect.width, rect.height * m_Childs[ i0].weight*0.2f);
					
					if( r.Contains( position) != false)
					{
						if (preDropAction == null)
						{
							tweenParam = GUIEx.ScaleTweenBox( r, tweenParam, string.Empty, GUIStyleCache.GetStyle( "SelectionRect"));
						}
						else
						{
							DropWindow( window, preDropAction, postDropAction, true, i0);
						}
						return true;
					}
					r = new Rect( 
						rect.x, rect.y + offset + rect.height * m_Childs[ i0].weight * 0.8f, 
						rect.width, rect.height * m_Childs[ i0].weight * 0.2f);
					
					if( r.Contains( position) != false)
					{
						if( preDropAction == null)
						{
							tweenParam = GUIEx.ScaleTweenBox( r, tweenParam, string.Empty, GUIStyleCache.GetStyle( "SelectionRect"));
						}
						else
						{
							DropWindow( window, preDropAction, postDropAction, true, i0 + 1);
						}
						return true;
					}
					r = new Rect( rect.x + rect.width * 0.8f, rect.y + offset, rect.width * 0.2f, rect.height*m_Childs[ i0].weight);
					
					if( r.Contains( position) != false && m_Childs.Count > 1)
					{
						if( preDropAction == null)
						{
							tweenParam = GUIEx.ScaleTweenBox( r, tweenParam, string.Empty, GUIStyleCache.GetStyle( "SelectionRect"));
						}
						else
						{
							DropWindow( window, preDropAction, postDropAction, false, i0);
						}
						return true;
					}
					offset += m_Childs[ i0].weight*rect.height;
				}
			}
			return false;
		}
		void DropWindow( SubWindow window, System.Action<SubWindow> preDropAction, System.Action postDropAction, bool betweenChilds, int dropIndex)
		{
			if( window == null)
			{
				return;
			}
			if( preDropAction == null)
			{
				return;
			}
			if( betweenChilds != false)
			{
				if( preDropAction != null)
				{
					preDropAction(window);
					AddWindow( window, dropIndex);
					postDropAction();
				}
			}
			else
			{
				if( preDropAction != null && dropIndex >= 0 && dropIndex < m_Childs.Count)
				{
					SubWindowNode child = m_Childs[ dropIndex];
					
					if( child.ContainWindow( window))
					{
						return;
					}
					preDropAction( window);
					m_Childs.RemoveAt( dropIndex);
					
                    var node = new SubWindowNode( !isHorizontal, depth + 1)
                    {
                        weight = child.weight
                    };
                    child.isHorizontal = isHorizontal;
					child.depth = node.depth + 1;
					node.Insert( child, 0);
					Insert( node, dropIndex);
					node.AddWindow( window, -1);
					postDropAction();
				}
			}
		}
		void Resize( int first, int second, Rect rect)
		{
			if( isHorizontal != false)
			{
				EditorGUIUtility.AddCursorRect( rect, MouseCursor.ResizeHorizontal);
			}
			else
			{
				EditorGUIUtility.AddCursorRect( rect, MouseCursor.ResizeVertical);
			}
			if( Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				if( rect.Contains( Event.current.mousePosition) != false)
				{
					Event.current.Use();
					m_IsDragging = true;
					m_CurrentResizeFirstId = first;
					m_CurrentResizeSecondId = second;
				}
			}
		}
		void DoResize( System.Action repaintAct)
		{
			if( m_IsDragging != false)
			{
				if( Event.current.type == EventType.MouseUp && Event.current.button == 0)
				{
					m_IsDragging = false;
					Event.current.Use();
				}
				if( Event.current.type == EventType.MouseDrag && Event.current.button == 0)
				{
					float delta = 0;
					
					if( isHorizontal != false)
					{
						delta = Event.current.delta.x / rect.width;
					}
					else
					{
						delta = Event.current.delta.y / rect.height;
					}
					float addW = m_Childs[ m_CurrentResizeFirstId].weight + delta;
					float musW = m_Childs[ m_CurrentResizeSecondId].weight - delta;
					
					if( addW >= kMinWeight && addW <= kMaxWeight && musW >= kMinWeight && musW <= kMaxWeight)
					{
						m_Childs[ m_CurrentResizeFirstId].weight = addW;
						m_Childs[ m_CurrentResizeSecondId].weight = musW;
					}
					if (repaintAct != null)
					{
						repaintAct();
					}
				}
			}
		}
		protected const int kMaxNodeDepth = 4;
        readonly List<SubWindowNode> m_Childs = new();
		public float weight = 1;
		protected int depth;
		protected bool isHorizontal;
		protected Rect rect;
		protected GUITweenParam tweenParam;
		//Rect m_OriginRect;
		//float m_RectTweenTime;
		bool m_IsDragging;
		int m_CurrentResizeFirstId;
		int m_CurrentResizeSecondId;
		const float kMaxWeight = 0.9f;
		const float kMinWeight = 0.1f;
	}
}