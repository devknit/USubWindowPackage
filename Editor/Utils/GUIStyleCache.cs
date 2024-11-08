using UnityEngine;
using System.Collections.Generic;

public class GUIStyleCache
{
	public static GUIStyle GetStyle(string style)
	{
		if( instance == null)
		{
			instance = new GUIStyleCache();
		}
		if( instance.m_StyleCache == null)
		{
			instance.m_StyleCache = new Dictionary<string, GUIStyle>();
		}
		GUIStyle st = null;
		
		if( instance.m_StyleCache.ContainsKey( style) != false)
		{
			st = instance.m_StyleCache[ style];
		}
		if( st == null)
		{
			st = style;
			instance.m_StyleCache[ style] = st;
		}
		return st;
	}
	static GUIStyleCache instance;
	Dictionary<string, GUIStyle> m_StyleCache;
}
