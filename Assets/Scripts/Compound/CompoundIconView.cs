using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CompoundIconView : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler
{
    public RawImage rawImage;

    private CompoundTooltipMessage _tooltipMessage = new CompoundTooltipMessage();
    private Transform DragPanel;
    private GameObject Copy;

    private void Awake()
    {
        DragPanel = GameObject.Find( "DragPanel" ).transform;
    }

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
        Copy.transform.position = eventData.position;
        //ExecuteEvents.ExecuteHierarchy( transform.parent.gameObject, eventData, ExecuteEvents.dragHandler );
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        _tooltipMessage.Action = CompoundControlAction.REMOVE;
        GameMessage.Send( _tooltipMessage );

        Copy = Instantiate( this.gameObject, DragPanel );
        Copy.GetComponent<CompoundIconView>().IsRaycastTarget = false;
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        _tooltipMessage.Action = CompoundControlAction.REMOVE;
        GameMessage.Send( _tooltipMessage );

        Destroy( Copy );
        Copy = null;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        _tooltipMessage.Action = CompoundControlAction.ADD;
        _tooltipMessage.Position = transform.position;
        GameMessage.Send( _tooltipMessage );
    }

    void OnDestroy()
    {
        DragPanel = null;
        Copy = null;
        rawImage.texture = null;
        rawImage = null;
    }

    
}
