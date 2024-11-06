
namespace EditorWinEx.Internal
{
	internal abstract class EWComponentDrawerBase
	{
		public bool IsInitialized
		{
			get; private set;
		}
		public bool IsEnabled
		{
			get; private set;
		}
		public void Init()
		{
			if( IsInitialized != false)
			{
				return;
			}
			if( OnInit() == false)
			{
				return;
			}
			IsInitialized = true;
		}
		public void Enable()
		{
			if( IsEnabled == false)
			{
				OnEnable();
			}
			IsEnabled = true;
		}
		public void Disable()
		{
			if( IsEnabled != false)
			{
				OnDisable();
			}
			IsEnabled = false;
		}
		public void Destroy()
		{
			Disable();
			OnDestroy();
			IsInitialized = false;
		}
		protected abstract bool OnInit();
		protected abstract void OnEnable();
		protected abstract void OnDisable();
		protected abstract void OnDestroy();
	}
}