using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CompoundTooltipPanel : GameView
{
    public Transform Tooltip;
    public Text Text;

    // Use this for initialization
    void Start()
    {
        GameMessage.Listen<CompoundTooltipMessage>( OnTooltipMessage );
    }

    private void OnTooltipMessage( CompoundTooltipMessage value )
    {
        switch( value.Action )
        {
            case CompoundControlAction.ADD:
                ShowTooltip( value.Compound, value.Position );
                break;
            case CompoundControlAction.REMOVE:
                HideTooltip();
                break;
            default:
                break;
        }
    }

    private void ShowTooltip( CompoundJSON compound, Vector3 position )
    {
        Text.text = compound.Name;
        Tooltip.position = position;
        Tooltip.gameObject.SetActive( true );
    }

    private void HideTooltip()
    {
        Tooltip.gameObject.SetActive( false );
    }
}
