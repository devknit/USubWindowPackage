
using UnityEngine;
using EditorWinEx.Internal;
using System;

[Serializable]
public abstract class EWMsgBoxCustomDrawer : CustomEWComponentDrawerBase
{
    public abstract EWRectangle Recttangle
    {
        get;
    }
    public Action closeAction;
    
    public void CloseMsgBox()
    {
        if( closeAction != null)
        {
            closeAction();
        }
    }
    public override void OnDestroy()
    {
    }
    public override void OnDisable()
    {
    }
    public override void OnEnable()
    {
    }
    public override void Init()
    {
    }
    public virtual void DrawMsgBox( Rect rect, object obj)
    {
    }
}
