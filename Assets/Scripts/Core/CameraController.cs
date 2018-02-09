using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float MovementSpeed = 0.1f;
    public float MousePanSpeed = 10f;
    
    private Vector3 _oldPosition;
    private Vector3 _viewportOrigin;


    // Update is called once per frame
    void Update()
    {
        if( Input.GetKey( KeyCode.W ) )
            gameObject.transform.Translate( Vector3.forward * MovementSpeed );
        if( Input.GetKey( KeyCode.A ) )
            gameObject.transform.Translate( Vector3.left * MovementSpeed );
        if( Input.GetKey( KeyCode.S ) )
            gameObject.transform.Translate( Vector3.back * MovementSpeed );
        if( Input.GetKey( KeyCode.D ) )
            gameObject.transform.Translate( Vector3.right * MovementSpeed );

        if( Input.GetKey( KeyCode.Q ) )
            gameObject.transform.Translate( Vector3.up * MovementSpeed );
        if( Input.GetKey( KeyCode.E ) )
            gameObject.transform.Translate( Vector3.down * MovementSpeed );

        if( Input.GetAxis( "Mouse ScrollWheel" ) > 0 )
            gameObject.transform.Translate( Vector3.up * MovementSpeed * 3 );
        if( Input.GetAxis( "Mouse ScrollWheel" ) < 0 )
            gameObject.transform.Translate( Vector3.down * MovementSpeed  * 3 );

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
        
    }
}
