
using UnityEngine;
using System;
using System.Reflection;

namespace MDI.Editor.Internal
{
	[Serializable]
	internal class EWSerializationObject
	{
		EWSerializationObject( object obj)
		{
			m_Obj = obj;
			m_ObjectAssemblyName = obj.GetType().Assembly.FullName;
			m_ObjectClassName = obj.GetType().FullName;
		}
		public static EWSerializationObject CreateInstance( object handle)
		{
			if (handle == null)
			{
				return null;
			}
			return new EWSerializationObject(handle);
		}
		public void SaveObject(string windowID)
		{
			if( m_Obj == null)
			{
				return;
			}
			string id = windowID + "." + m_ObjectAssemblyName + "." + m_ObjectClassName;
			EditorPrefsEx.SetObject(id, this.m_Obj);
		}
		public void LoadObject( string windowID)
		{
			if( m_Obj != null)
			{
				return;
			}
			string id = windowID + "." + m_ObjectAssemblyName + "." + m_ObjectClassName;
			
			if( EditorPrefsEx.HasKey( id) == false)
			{
				return;
			}
			Assembly assembly = Assembly.Load( m_ObjectAssemblyName);
			
			if( assembly != null)
			{
				Type type = assembly.GetType( m_ObjectClassName);
				
				if( type != null)
				{
					m_Obj = EditorPrefsEx.GetObject( id, type);
				}
			}
		}
		public void ClearObject( string windowID)
		{
			string id = windowID + "." + m_ObjectAssemblyName + "." + m_ObjectClassName;
			EditorPrefsEx.DeleteKey(id);
		}
		public object Obj
		{
			get { return m_Obj; }
		}
		object m_Obj;
		[SerializeField]
		string m_ObjectClassName;
		[SerializeField]
		string m_ObjectAssemblyName;
	}
}