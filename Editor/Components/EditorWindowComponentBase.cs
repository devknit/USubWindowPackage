
using System;
using System.Reflection;

namespace EditorWinEx
{
	public abstract class EditorWindowComponentBase
	{
		public bool IsInitialized
		{
			get; private set;
		}
		public void RegisterMethod( object container, MethodInfo method, object target)
		{
			if( IsInitialized == false)
			{
				OnRegisterMethod( container, method, target);
			}
		}
		public void RegisterClass( object container, Type type)
		{
			if( IsInitialized == false)
			{
				OnRegisterClass( container, type);
			}
		}
		public void Init()
		{
			if( IsInitialized == false)
			{
				OnInit();
				IsInitialized = true;
			}
		}
		public void Destroy()
		{
			if( IsInitialized != false)
			{
				OnDestroy();
				IsInitialized = false;
			}
		}
		public void Disable()
		{
			if( IsInitialized != false)
			{
				OnDisable();
			}
		}
		protected abstract void OnRegisterMethod( object container, MethodInfo method, object target);
		protected abstract void OnRegisterClass( object container, Type type);
		
		protected virtual void OnInit()
		{
		}
		protected virtual void OnDestroy()
		{
		}
		protected virtual void OnDisable()
		{
		}
	}
}