using UnityEngine;
using System.Collections;

public class CameraMessage
{
    public CameraAction Action = CameraAction.START_DRAG;
}

public enum CameraAction
{
    START_DRAG,
    DRAG
}
