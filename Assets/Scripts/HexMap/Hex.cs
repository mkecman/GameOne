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
        Model.isMarked.Skip(1).Subscribe( _ => UpdateMarkedColor( _ ) ).AddTo( disposables );
        Model.ObserveEveryValueChanged( _ => _.Lens ).Subscribe( _ => SetColor() ).AddTo( disposables );

        SetSymbol();
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

    private void SetSymbol()
    {
        SymbolText.text = Model.Element.Symbol;

        //SymbolText.text = Model.Altitude.ToString(); //Show altitude;
        //SymbolText.text = Model.X + "," + Model.Y; //Show coordinates;

        SymbolText.gameObject.transform.position = new Vector3( SymbolText.gameObject.transform.position.x, Model.Altitude + .01f, SymbolText.gameObject.transform.position.z );
    }

    private void SetClouds()
    {
        Gas.transform.position = new Vector3( Gas.transform.position.x, 0.9f, Gas.transform.position.z );
        Gas.GetComponent<MeshRenderer>().material.color = new Color32( 255, 255, 255, (byte)(Model.Humidity * 255) );
    }

    private void SetColor()
    {
        Solid.GetComponent<MeshRenderer>().material.color = Model.Colors[ (int)Model.Lens ];
    }

    private void SetHeight( GameObject target, float height )
    {
        Mesh mesh = target.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        for( int q = 0; q < verts.Length; ++q )
        {
            if( verts[ q ].y > 0 )
                verts[ q ].y = height + 0.1f;
        }
        mesh.vertices = verts;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
