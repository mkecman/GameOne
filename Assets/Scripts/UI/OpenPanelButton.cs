using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class OpenPanelButton : GameView
{
    public GameObject Panel;
    public Button Button;
    // Use this for initialization
    void Start()
    {
        Button.onClick.AddListener( OnButtonClick );
    }

    private void OnButtonClick()
    {
        if( Panel.activeSelf )
            Panel.SetActive( false );
        else
            Panel.SetActive( true );
    }
}
