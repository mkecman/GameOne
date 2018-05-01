using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CompoundIconView : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler
{
    public RawImage rawImage;

    private CompoundTooltipMessage _tooltipMessage = new CompoundTooltipMessage();

    public void Setup( CompoundJSON compound )
    {
        _tooltipMessage.Compound = compound;
        rawImage.texture = Resources.Load( "CompoundTexture/" + compound.Index ) as Texture2D;
    }

    public bool IsRaycastTarget
    {
        get { return rawImage.raycastTarget; }
        set { rawImage.raycastTarget = value; }
    }

    public void OnDrag( PointerEventData eventData )
    {
        ExecuteEvents.ExecuteHierarchy( transform.parent.gameObject, eventData, ExecuteEvents.dragHandler );
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        _tooltipMessage.Action = CompoundControlAction.REMOVE;
        GameMessage.Send( _tooltipMessage );
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        _tooltipMessage.Action = CompoundControlAction.REMOVE;
        GameMessage.Send( _tooltipMessage );

        ExecuteEvents.ExecuteHierarchy( transform.parent.gameObject, eventData, ExecuteEvents.pointerUpHandler );
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        _tooltipMessage.Action = CompoundControlAction.ADD;
        _tooltipMessage.Position = transform.position;
        GameMessage.Send( _tooltipMessage );

        ExecuteEvents.ExecuteHierarchy( transform.parent.gameObject, eventData, ExecuteEvents.pointerDownHandler );
    }

    void OnDestroy()
    {
        rawImage.texture = null;
        rawImage = null;
    }

    
}
