using UnityEngine;
using UnityEditor;
using System.Collections;

public struct GUITweenParam
{
    public float tweenTime;

    public bool isTweening;

    private Rect m_Rect;

    public GUITweenParam(bool isTweening = true)
    {
        this.isTweening = isTweening;
        this.tweenTime = 0;
        this.m_Rect = default(Rect);
    }

    public bool CheckRect(Rect rect)
    {
        if (m_Rect != rect)
        {
            m_Rect = rect;
            return false;
        }
        return true;
    }
}

public class GUIEx
{

    public static bool ToolbarButton(Rect rect, string text)
    {
        return GUI.Button(rect, text, "toolbarbutton");
    }

    public static GUITweenParam ScaleTweenBox(Rect rect, GUITweenParam param, string text, GUIStyle style = null)
    {
        param = ScaleTweenInternal(ref rect, param);
        if (style != null)
            GUI.Box(rect, text, style);
        else
            GUI.Box(rect, text);
        return param;
    }

    public static GUITweenParam ScaleTweenBox(Rect rect, GUITweenParam param, GUIContent content, GUIStyle style = null)
    {
        param = ScaleTweenInternal(ref rect, param);
        if (style != null)
            GUI.Box(rect, content, style);
        else
            GUI.Box(rect, content);
        return param;
    }

    private static GUITweenParam ScaleTweenInternal(ref Rect rect, GUITweenParam param)
    {
        if (!param.CheckRect(rect))
        {
            param.tweenTime = 0;
        }

        param.tweenTime += 0.03f;
        //float scaleTweenTime = ((float)(EditorApplication.timeSinceStartup - param.tweenTime) / 0.1f);
        if (param.tweenTime > 1)
        {
            param.tweenTime = 1;
            param.isTweening = false;
        }
        else
        {
            param.isTweening = true;
        }

        float x = Mathf.Lerp(rect.x + rect.width / 2, rect.x, param.tweenTime);
        float y = Mathf.Lerp(rect.y + rect.height / 2, rect.y, param.tweenTime);
        float w = Mathf.Lerp(0, rect.width, param.tweenTime);
        float h = Mathf.Lerp(0, rect.height, param.tweenTime);
        rect = new Rect(x, y, w, h);
        return param;
    }

    public static string GetIconPath(EWSubWindowIcon icon)
    {
        switch (icon)
        {
            case EWSubWindowIcon.None:
                return null;
            case EWSubWindowIcon.Animation:
                return EditorGUIUtility.isProSkin ? "d_UnityEditor.AnimationWindow" : "UnityEditor.AnimationWindow";
            case EWSubWindowIcon.Animator:
                return "UnityEditor.Graphs.AnimatorControllerTool";
            case EWSubWindowIcon.AssetStore:
                return EditorGUIUtility.isProSkin ? "d_Asset Store" : "Asset Store";
            case EWSubWindowIcon.AudioMixer:
                return EditorGUIUtility.isProSkin ? "d_Audio Mixer" : "Audio Mixer";
            case EWSubWindowIcon.Web:
                return EditorGUIUtility.isProSkin ? "d_BuildSettings.Web.Small" : "BuildSettings.Web.Small";
            case EWSubWindowIcon.Console:
                return EditorGUIUtility.isProSkin ? "d_UnityEditor.ConsoleWindow" : "UnityEditor.ConsoleWindow";
            case EWSubWindowIcon.Game:
                return EditorGUIUtility.isProSkin ? "d_UnityEditor.GameView" : "UnityEditor.GameView";
            case EWSubWindowIcon.Hierarchy:
                return EditorGUIUtility.isProSkin ? "d_UnityEditor.HierarchyWindow" : "UnityEditor.HierarchyWindow";
            case EWSubWindowIcon.Inspector:
                return EditorGUIUtility.isProSkin ? "d_UnityEditor.InspectorWindow" : "UnityEditor.InspectorWindow";
            case EWSubWindowIcon.Lighting:
                return EditorGUIUtility.isProSkin ? "d_Lighting" : "Lighting";
            case EWSubWindowIcon.Navigation:
                return EditorGUIUtility.isProSkin ? "d_Navigation" : "Navigation";
            case EWSubWindowIcon.Occlusion:
                return EditorGUIUtility.isProSkin ? "d_Occlusion" : "Occlusion";
            case EWSubWindowIcon.Profiler:
                return EditorGUIUtility.isProSkin ? "d_ZUnityEditor.ProfilerWindow" : "UnityEditor.ProfilerWindow";
            case EWSubWindowIcon.Project:
                return EditorGUIUtility.isProSkin ? "d_Project" : "Project";
            case EWSubWindowIcon.Scene:
                return EditorGUIUtility.isProSkin ? "d_UnityEditor.SceneView" : "UnityEditor.SceneView";
            case EWSubWindowIcon.BuildSetting:
                return EditorGUIUtility.isProSkin ? "d_BuildSettings.SelectedIcon" : "BuildSettings.SelectedIcon";
            case EWSubWindowIcon.Shader:
                return "Shader Icon";
            case EWSubWindowIcon.Avator:
                return "Avatar Icon";
            case EWSubWindowIcon.GameObject:
                return EditorGUIUtility.isProSkin ? "d_GameObject Icon" : "GameObject Icon";
            case EWSubWindowIcon.Camera:
                return "Camera Icon";
            case EWSubWindowIcon.JavaScript:
                return "js Script Icon";
            case EWSubWindowIcon.CSharp:
                return "cs Script Icon";
            case EWSubWindowIcon.Sprite:
                return "Sprite Icon";
            case EWSubWindowIcon.Text:
                return "TextAsset Icon";
            case EWSubWindowIcon.AnimatorController:
                return "AnimatorController Icon";
            case EWSubWindowIcon.MeshRenderer:
                return "MeshRenderer Icon";
            case EWSubWindowIcon.Terrain:
                return "Terrain Icon";
            case EWSubWindowIcon.Audio:
                return EditorGUIUtility.isProSkin ? "d_SceneviewAudio" : "SceneviewAudio";
            case EWSubWindowIcon.IPhone:
                return EditorGUIUtility.isProSkin ? "d_BuildSettings.iPhone.small" : "BuildSettings.iPhone.small";
            case EWSubWindowIcon.Font:
                return "Font Icon";
            case EWSubWindowIcon.Material:
                return "Material Icon";
            case EWSubWindowIcon.GameManager:
                return "GameManager Icon";
            case EWSubWindowIcon.Player:
                return "Animation Icon";
            case EWSubWindowIcon.Texture:
                return "Texture Icon";
            case EWSubWindowIcon.Scriptable:
                return "ScriptableObject Icon";
            case EWSubWindowIcon.Movie:
                return "MovieTexture Icon";
            case EWSubWindowIcon.CGProgram:
                return "CGProgram Icon";
            case EWSubWindowIcon.Search:
                return "Search Icon";
            case EWSubWindowIcon.Favorite:
                return "Favorite Icon";
            case EWSubWindowIcon.Android:
                return EditorGUIUtility.isProSkin ? "d_BuildSettings.Android.small" : "BuildSettings.Android.small";
            case EWSubWindowIcon.Setting:
                return EditorGUIUtility.isProSkin ? "d_SettingsIcon" : "SettingsIcon";
            case EWSubWindowIcon.TimelineSelector:
            	return EditorGUIUtility.isProSkin ? "d_TimelineSelector" : "TimelineSelector";
            default:
                return null;
        }
    }
}
