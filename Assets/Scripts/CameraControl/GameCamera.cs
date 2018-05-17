using UnityEngine;
using System.Collections;
using System;

public class GameCamera : MonoBehaviour
{
    public float PanSpeed = 10f;
    public float ZoomSpeed = 1f;

    private Vector3 _oldPosition;
    private Vector3 _newPosition;
    private Vector3 _viewportOrigin;

    private bool _enabled = true;
    private bool _PanEnabled = true;
    private TooltipMessage _toolTipMessage = new TooltipMessage();

    // Use this for initialization
    void Start()
    {
        _toolTipMessage.Action = TooltipAction.SHOW;
        _toolTipMessage.Position = new Vector3( 600, 600 );

        GameMessage.Listen<CameraControlMessage>( OnCameraControlChange );
        GameMessage.Listen<CameraMessage>( OnCameraMessage );

    }

    private void OnCameraMessage( CameraMessage value )
    {
        if( !_PanEnabled )
            return; 

        if( value.Action == CameraAction.START_DRAG )
        {
            _oldPosition = transform.position;
            _viewportOrigin = Camera.main.ScreenToViewportPoint( Input.mousePosition );
        }
        else
        {
            _newPosition = Camera.main.ScreenToViewportPoint( Input.mousePosition ) - _viewportOrigin;
            _newPosition.z = _newPosition.y;
            _newPosition.y = 0;
            transform.position = _oldPosition + -_newPosition * PanSpeed;
        }
    }

    private void OnCameraControlChange( CameraControlMessage value )
    {
        _enabled = value.Enable;
    }

    // Update is called once per frame
    void Update()
    {
        if( !_enabled )
            return;

        if( Input.GetAxis( "Mouse ScrollWheel" ) > 0 )
            gameObject.transform.Translate( Vector3.up * ZoomSpeed );
        if( Input.GetAxis( "Mouse ScrollWheel" ) < 0 )
            gameObject.transform.Translate( Vector3.down * ZoomSpeed );

        if( Input.touchCount >= 2 )
        {
            _PanEnabled = false;
            // Store both touches.
            Touch touchZero = Input.GetTouch( 0 );
            Touch touchOne = Input.GetTouch( 1 );

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = ( touchZeroPrevPos - touchOnePrevPos ).magnitude;
            float touchDeltaMag = ( touchZero.position - touchOne.position ).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            _toolTipMessage.Text = deltaMagnitudeDiff.ToString( "N2" );

            // Otherwise change the field of view based on the change in distance between the touches.
            gameObject.transform.Translate( Vector3.up * deltaMagnitudeDiff * .01f );
            gameObject.transform.position = new Vector3( gameObject.transform.position.x, Mathf.Clamp( gameObject.transform.position.y, 10f, 50f ), gameObject.transform.position.z );
            //Camera.main.fieldOfView += deltaMagnitudeDiff * .01f;
            //Camera.main.fieldOfView = Mathf.Clamp( Camera.main.fieldOfView, 5f, 20f );

            //float distance = Vector2.Distance( Input.GetTouch( 0 ).position, Input.GetTouch( 1 ).position );
            //gameObject.transform.position = new Vector3( gameObject.transform.position.x, Mathf.Clamp( distance / 20, 10f, 50f ), gameObject.transform.position.z );
            //_toolTipMessage.Text = distance.ToString() + "\n" + Input.GetTouch( 0 ).position.x + ":" + Input.GetTouch( 0 ).position.y + "\n" + Input.GetTouch( 1 ).position.x + ":" + Input.GetTouch( 1 ).position.y;
            //GameMessage.Send( _toolTipMessage );
        }
        else
        {
            _PanEnabled = true;
        }

    }
}