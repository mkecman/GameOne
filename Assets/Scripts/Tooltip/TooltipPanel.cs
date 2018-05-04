using UnityEngine;
using UnityEngine.UI;

public class TooltipPanel : GameView
{
    public Transform Tooltip;
    public Text Text;

    // Use this for initialization
    void Start()
    {
        GameMessage.Listen<TooltipMessage>( OnTooltipMessage );
    }

    private void OnTooltipMessage( TooltipMessage value )
    {
        switch( value.Action )
        {
            case TooltipAction.SHOW:
                ShowTooltip( value.Text, value.Position );
                break;
            case TooltipAction.HIDE:
                HideTooltip();
                break;
            default:
                break;
        }
    }

    private void ShowTooltip( string text, Vector3 position )
    {
        Text.text = text;
        Tooltip.position = position;
        Tooltip.gameObject.SetActive( true );
    }

    private void HideTooltip()
    {
        Tooltip.gameObject.SetActive( false );
    }
}
