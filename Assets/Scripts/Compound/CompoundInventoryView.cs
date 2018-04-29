using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class CompoundInventoryView : GameView, IDragHandler
{
    public RawImage CompoundTexture;
    public Text AmountText;

    internal void Setup( CompoundJSON compound, IntReactiveProperty amount )
    {
        disposables.Clear();
        amount.Subscribe( _ => AmountText.text = _.ToString() ).AddTo( disposables );
        CompoundTexture.texture = compound.Texture;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        CompoundTexture.texture = null;
    }

    public void OnDrag( PointerEventData eventData )
    {
        Debug.Log( "DRAGGGING!" );
    }
}
