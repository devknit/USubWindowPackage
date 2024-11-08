using System;

namespace MDI.Editor
{
	public enum EWSubWindowIcon
	{
		None,
		BuildSetting,
		Hierarchy,
		Scene,
		Inspector,
		Game,
		Console,
		Project,
		Animation,
		Profiler,
		AudioMixer,
		AssetStore,
		Animator,
		Lighting,
		Occlusion,
		Navigation,
		Web,
		IPhone,
		Android,
		Shader,
		Avator,
		GameObject,
		Camera,
		JavaScript,
		CSharp,
		Sprite,
		Text,
		AnimatorController,
		Terrain,
		MeshRenderer,
		Font,
		Material,
		GameManager,
		Texture,
		Scriptable,
		CGProgram,
		Favorite,
		Search,
		Player,
		Movie,
		Audio,
		Setting,
		TimelineSelector,
	}
	public enum EWSubWindowToolbarType
	{
		None,
		Normal,
		Mini,
	}
	[AttributeUsage( AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class EWSubWindowAttribute : Attribute
	{
		public EWSubWindowAttribute( string title, EWSubWindowIcon icon = EWSubWindowIcon.None, bool active = true, SubWindowStyle windowStyle = SubWindowStyle.Default, EWSubWindowToolbarType toolbar = EWSubWindowToolbarType.None, SubWindowHelpBoxType helpBox = SubWindowHelpBoxType.None)
		{
			this.title = title;
			this.active = active;
			this.windowStyle = windowStyle;
			this.toolbar = toolbar;
			this.helpBox = helpBox;
			this.iconPath = GUIEx.GetIconPath( icon);
		}
		public EWSubWindowAttribute( string title, string icon, bool active = true, SubWindowStyle windowStyle = SubWindowStyle.Default, EWSubWindowToolbarType toolbar = EWSubWindowToolbarType.None, SubWindowHelpBoxType helpBox = SubWindowHelpBoxType.None)
		{
			this.title = title;
			this.active = active;
			this.windowStyle = windowStyle;
			this.toolbar = toolbar;
			this.helpBox = helpBox;
			this.iconPath = icon;
		}
		public string title;
		public bool active;
		public SubWindowStyle windowStyle;
		public string iconPath;
		public EWSubWindowToolbarType toolbar;
		public SubWindowHelpBoxType helpBox;
	}
}