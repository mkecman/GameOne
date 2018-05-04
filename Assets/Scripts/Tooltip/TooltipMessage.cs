using UnityEngine;

public class TooltipMessage
{
    public string Text;
    public Vector3 Position;
    public TooltipAction Action = TooltipAction.SHOW;
}

public enum TooltipAction
{
    SHOW,
    HIDE
}
