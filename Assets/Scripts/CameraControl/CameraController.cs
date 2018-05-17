﻿using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    public float MovementSpeed = 0.1f;
    public float MousePanSpeed = 10f;
    public float ZoomSpeed = 1f;
    
    private Vector3 _oldPosition;
    private Vector3 _viewportOrigin;
    private bool _enabled = true;

    private void Start()
    {
        GameMessage.Listen<CameraControlMessage>( OnCameraControlChange );
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

        if( Input.GetKey( KeyCode.W ) )
            gameObject.transform.Translate( Vector3.forward * MovementSpeed );
        if( Input.GetKey( KeyCode.A ) )
            gameObject.transform.Translate( Vector3.left * MovementSpeed );
        if( Input.GetKey( KeyCode.S ) )
            gameObject.transform.Translate( Vector3.back * MovementSpeed );
        if( Input.GetKey( KeyCode.D ) )
            gameObject.transform.Translate( Vector3.right * MovementSpeed );

        if( Input.GetKey( KeyCode.Q ) )
            gameObject.transform.Translate( Vector3.up * ZoomSpeed );
        if( Input.GetKey( KeyCode.E ) )
            gameObject.transform.Translate( Vector3.down * ZoomSpeed );

        if( Input.GetAxis( "Mouse ScrollWheel" ) > 0 )
            gameObject.transform.Translate( Vector3.up * ZoomSpeed );
        if( Input.GetAxis( "Mouse ScrollWheel" ) < 0 )
            gameObject.transform.Translate( Vector3.down * ZoomSpeed );

        gameObject.transform.position = new Vector3( gameObject.transform.position.x, Mathf.Clamp( gameObject.transform.position.y, 10f, 40f ), gameObject.transform.position.z );
        /*
        // Moved to GameCamera class
        if( Input.GetMouseButtonDown( 0 ) )
        {
            _oldPosition = transform.position;
            _viewportOrigin = Camera.main.ScreenToViewportPoint( Input.mousePosition );
        }

        if( Input.GetMouseButton( 0 ) )
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint( Input.mousePosition ) - _viewportOrigin;    
            pos.z = pos.y;
            pos.y = 0;
            transform.position = _oldPosition + -pos * MousePanSpeed;                                    
        }
        */
    }
}
