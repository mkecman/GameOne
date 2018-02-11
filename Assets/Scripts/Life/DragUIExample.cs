using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;

public class Worker : GameView, IPointerDownHandler, IPointerUpHandler
{
    public const string WORKER_POOL = "WorkerPool";

    public Transform _goTransform;
    public Image _image;

    private bool _isDragging = false;
    private int _elementIndex;
    private Vector2 _originalPosition;
    private List<RaycastResult> _raycastResultList = new List<RaycastResult>();
    private WorkerMoveMessage _moveMessage = new WorkerMoveMessage();

    public void UpdateModel( int ElementIndex )
    {
        _elementIndex = ElementIndex;
        _moveMessage.From = _elementIndex;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        _isDragging = true;
        
        _originalPosition = _goTransform.position;
        _image.raycastTarget = false;
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        RaycastTarget workerPool = GetDropTarget( WORKER_POOL );

        if( workerPool != null )
        {
            Debug.Log( "dropped Worker!" );
            _moveMessage.To = workerPool.Id;
            GameMessage.Send<WorkerMoveMessage>( _moveMessage );
        }
        else
        {
            _goTransform.position = _originalPosition;
        }

        _image.raycastTarget = true;
        _isDragging = false;
    }
    
    void Update()
    {
        if( _isDragging )
        {
            _goTransform.position = Input.mousePosition;
        }
    }
    
    private RaycastTarget GetDropTarget( string tag )
    {
        PointerEventData pointer = new PointerEventData( EventSystem.current );
        pointer.position = Input.mousePosition;
        EventSystem.current.RaycastAll( pointer, _raycastResultList );

        if( _raycastResultList.Count <= 0 )
            return null;

        if( _raycastResultList[ 0 ].gameObject.tag == tag )
            return _raycastResultList[ 0 ].gameObject.GetComponent<RaycastTarget>();

        return null;
    }

    
}

public class WorkerMoveMessage
{
    public int From;
    public int To;
}