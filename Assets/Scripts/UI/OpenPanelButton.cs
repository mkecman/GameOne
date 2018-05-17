using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class OpenPanelButton : GameView
{
    public GameObject Panel;
    public Button Button;
    private CameraControlMessage _cameraControl;

    // Use this for initialization
    void Start()
    {
        _cameraControl = new CameraControlMessage( false );
        Button.onClick.AddListener( OnButtonClick );
    }

    private void OnButtonClick()
    {
        //_cameraControl.Enable = Panel.activeSelf;
        //GameMessage.Send( _cameraControl );

        if( Panel.activeSelf )
            Panel.SetActive( false );
        else
            Panel.SetActive( true );
    }
}
