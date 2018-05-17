using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CompoundIconView : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler
{
    public bool CanDrag = true;
    public RawImage TextureImage;
    public Image BackgoundImage;
    public Image MaskImage;

    public Sprite[] BackgroundSprites;
    public Sprite[] MaskSprites;

    private TooltipMessage _tooltipMessage = new TooltipMessage();
    private Transform DragPanel;
    private GameObject Copy;

    private void Awake()
    {
        DragPanel = GameObject.Find( "DragPanel" ).transform;
    }

    public void Setup( CompoundJSON compound )
    {
        _tooltipMessage.Text = compound.Name + "\n";
        foreach( KeyValuePair<R,float> effect in compound.Effects )
            _tooltipMessage.Text += effect.Key + " " + effect.Value + "\n";

        TextureImage.texture = Resources.Load( "CompoundTexture/" + compound.Index ) as Texture2D;
        BackgoundImage.sprite = BackgroundSprites[ (int)compound.Type ];
        MaskImage.sprite = MaskSprites[ (int)compound.Type ];
    }

    public bool IsRaycastTarget
    {
        get { return TextureImage.raycastTarget; }
        set { TextureImage.raycastTarget = value; }
    }

    public void OnDrag( PointerEventData eventData )
    {
        if( !CanDrag )
            return;

        Copy.transform.position = eventData.position;
        //ExecuteEvents.ExecuteHierarchy( transform.parent.gameObject, eventData, ExecuteEvents.dragHandler );
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        if( !CanDrag )
            return;

        _tooltipMessage.Action = TooltipAction.HIDE;
        GameMessage.Send( _tooltipMessage );

        Copy = Instantiate( this.gameObject, DragPanel );
        Copy.GetComponent<CompoundIconView>().IsRaycastTarget = false;
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        if( !CanDrag )
            return;

        _tooltipMessage.Action = TooltipAction.HIDE;
        GameMessage.Send( _tooltipMessage );

        Destroy( Copy );
        Copy = null;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        if( !CanDrag )
            return;

        _tooltipMessage.Action = TooltipAction.SHOW;
        _tooltipMessage.Position = transform.position;
        GameMessage.Send( _tooltipMessage );
    }

    void OnDestroy()
    {
        DragPanel = null;
        Copy = null;
        TextureImage.texture = null;
        TextureImage = null;
    }

    
}
