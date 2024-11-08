
using UnityEngine;
using System;

namespace MDI.Editor
{
	[Serializable]
	public abstract class CustomEWComponentDrawerBase : IMessageDispatcher
	{
		public object Container
		{
			get { return container; }
		}
		[NonSerialized]
		protected object container;
		
		public void SetContainer( object container)
		{
			if( this.container != null)
			{
				Debug.LogError( "Error，Containers are only allowed to be set during initialization!");
				return;
			}
			this.container = container;
		}
		public abstract void Init();
		public abstract void OnEnable();
		public abstract void OnDisable();
		public abstract void OnDestroy();
		
		public Type GetContainerType()
		{
			if( container == null)
			{
				return null;
			}
			return container.GetType();
		}
	}
}