using UnityEngine;
using System.Collections;
using System;

public class GameCamera : MonoBehaviour
{
    public float PanSpeed = 10f;

    private Vector3 _oldPosition;
    private Vector3 _newPosition;
    private Vector3 _viewportOrigin;

    // Use this for initialization
    void Start()
    {
        GameMessage.Listen<CameraMessage>( OnCameraMessage );
    }

    private void OnCameraMessage( CameraMessage value )
    {
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
}