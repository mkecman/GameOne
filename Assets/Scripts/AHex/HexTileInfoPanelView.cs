using UnityEngine;
using System.Collections;
using System;

public class HexTileInfoPanelView : MonoBehaviour
{
    public UIPropertyView Altitude;
    public UIPropertyView Temperature;
    public UIPropertyView ElementName;

    private HexMapModel _hexMapModel;

    // Use this for initialization
    void Start()
    {
        GameModel.Bind<HexMapModel>( OnHexMapModelChange );
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
    }

    private void OnHexMapModelChange( HexMapModel value )
    {
        _hexMapModel = value;
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        Altitude.SetValue( _hexMapModel.hexMap.Table[ value.X, value.Y ].Altitude.ToString() );
        Temperature.SetValue( _hexMapModel.hexMap.Table[ value.X, value.Y ].Temperature.ToString() );
        ElementName.SetValue( _hexMapModel.hexMap.Table[ value.X, value.Y ].Element.Name.ToString() );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
