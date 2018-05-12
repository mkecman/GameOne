using UnityEngine;
using System.Collections;

public class PanelMessage
{
    public PanelAction Action;
    public string PanelName;

    public PanelMessage( PanelAction action, string panelName = "" )
    {
        Action = action;
        PanelName = panelName;
    }
}

public enum PanelAction
{
    SHOW,
    HIDE,
    HIDEALL,
    SHOWONLY
}
