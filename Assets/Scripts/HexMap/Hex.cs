using System;
using UniRx;
using UnityEngine;

public class Hex : GameView
{
    public HexModel Model;
    public TextMesh SymbolText;
    public GameObject Gas;
    public GameObject Liquid;
    public GameObject Solid;

    private HexClickedMessage _HexClickedMessage;

    public void SetModel( HexModel model )
    {
        this.Model = model;
        _HexClickedMessage = new HexClickedMessage( model );

        SetHeight( Solid, Model.Altitude );

        disposables.Clear();
        Model.isMarked.Skip( 1 ).Subscribe( _ => UpdateMarkedColor( _ ) ).AddTo( disposables );
        Model.ObserveEveryValueChanged( _ => _.Lens ).Subscribe( _ => { SetColor(); SetSymbol(); } ).AddTo( disposables );

        Model.isExplored.Subscribe( _ => SetSymbol() ).AddTo( disposables );
        //SetSymbol();
        //SetClouds();
    }



    private void UpdateMarkedColor( bool isMarked )
    {
        if( isMarked )
            Solid.GetComponent<MeshRenderer>().material.color = Color.magenta;
        else
            SetColor();
    }

    private void OnMouseDown()
    {
        GameMessage.Send<HexClickedMessage>( _HexClickedMessage );
    }

    /*
    private void OnMouseOver()
    {
        Solid.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    private void OnMouseExit()
    {
        SetColor();
    }
    */

    private void SetClouds()
    {
        Gas.transform.position = new Vector3( Gas.transform.position.x, 0.9f, Gas.transform.position.z );
        Gas.GetComponent<MeshRenderer>().material.color = new Color32( 255, 255, 255, (byte)(Model.Humidity * 255) );
    }

    private void SetColor()
    {
        Solid.GetComponent<MeshRenderer>().material.color = Model.Colors[ (int)Model.Lens ];
    }

    private void SetSymbol()
    {
        if( !Model.isExplored.Value )
            return;

        switch( Model.Lens )
        {
            case HexMapLens.Normal:
                SymbolText.text = "<color=\"#007800\">" + Model.Element.Modifier( ElementModifiers.Food ).Delta + "</color> <color=\"#000ff0\">" + Model.Element.Modifier( ElementModifiers.Science ).Delta + "</color>\n<color=\"#ff0000\">" + Model.Element.Modifier( ElementModifiers.Words ).Delta + "</color>";
                break;
            case HexMapLens.Altitude:
                SymbolText.text = Math.Round( Model.Altitude, 2 ).ToString(); //Show altitude;
                break;
            case HexMapLens.Temperature:
                SymbolText.text = Math.Round( Model.Temperature, 2 ).ToString();
                break;
            case HexMapLens.Pressure:
                SymbolText.text = Math.Round( Model.Pressure, 2 ).ToString();
                break;
            case HexMapLens.Humidity:
                SymbolText.text = Math.Round( Model.Humidity, 2 ).ToString();
                break;
            case HexMapLens.Radiation:
                SymbolText.text = Math.Round( Model.Radiation, 2 ).ToString();
                break;
            case HexMapLens.TotalScore:
                SymbolText.text = Math.Round( Model.TotalScore, 2 ).ToString();
                break;
            default:
                break;
        }

        //SymbolText.text = Model.Element.Symbol;
        //SymbolText.text = Model.X + "," + Model.Y; //Show coordinates;

        SymbolText.gameObject.transform.position = new Vector3( SymbolText.gameObject.transform.position.x, (float)Model.Altitude + .01f, SymbolText.gameObject.transform.position.z );
    }

    private void SetHeight( GameObject target, double height )
    {
        Mesh mesh = target.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        for( int q = 0; q < verts.Length; ++q )
        {
            if( verts[ q ].y > 0 )
                verts[ q ].y = (float)height + 0.1f;
        }
        mesh.vertices = verts;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
