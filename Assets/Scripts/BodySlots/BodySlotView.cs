using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BodySlotView : GameView
{
    public Image BackgroundImage;
    public List<Color> StateColors;

    private bool _isEnabled;

    public bool IsEnabled
    {
        get { return _isEnabled; }
        set { _isEnabled = value; SetState(); }
    }
    
    private void SetState()
    {
        BackgroundImage.color = _isEnabled ? StateColors[ 1 ] : StateColors[ 0 ];
    }
}
