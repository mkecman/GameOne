using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hex : GameView
{
    public HexModel Model;
    public TextMesh SymbolText;
    public GameObject Gas;
    public GameObject Liquid;
    public GameObject Solid;
    public Gradient DefaultColorGradient;
    public Gradient DefaultColorGradient2;

    private HexClickedMessage _HexClickedMessage;

    public void SetModel( HexModel model )
    {
        this.Model = model;
        _HexClickedMessage = new HexClickedMessage( model );

        SetHeight( Solid, Model.Props[ R.Altitude ].Value );

        disposables.Clear();
        Model.isMarked.Skip( 1 ).Subscribe( _ => UpdateMarkedColor( _ ) ).AddTo( disposables );
        Model.ObserveEveryValueChanged( _ => _.Lens ).Subscribe( _ => { SetColor(); SetSymbol(); } ).AddTo( disposables );

        Model.Props[ R.HexScore ]._Value.Subscribe( _ => OnHexScoreChange() ).AddTo( disposables );
        
        Model.isExplored.Subscribe( _ => SetSymbol() ).AddTo( disposables );

        OnHexScoreChange();

        //SetSymbol();
        //SetClouds();
    }

    private void OnHexScoreChange()
    {
        Model.Props[ R.Default ].Color = Color.Lerp(
            DefaultColorGradient.Evaluate( (float)( Model.Props[ R.Temperature ].Value ) ),
            DefaultColorGradient2.Evaluate( (float)( Model.Props[ R.Humidity ].Value ) ),
            0.5f );
        /*
        Model.Props[ R.Default ].Color = Color.Lerp( 
            DefaultColorGradient.Evaluate( (float)( Model.Props[ R.Temperature ].Value ) ), 
            DefaultColorGradient2.Evaluate( (float)( Model.Props[ R.Humidity ].Value ) ), 
            0.5f );
        */
        SetColor();
        SetSymbol();
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
        if( !EventSystem.current.IsPointerOverGameObject() )
            GameMessage.Send( _HexClickedMessage );
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
        Gas.GetComponent<MeshRenderer>().material.color = new Color32( 255, 255, 255, (byte)(Model.Props[ R.Humidity ].Value * 255) );
    }

    private void SetColor()
    {
        Solid.GetComponent<MeshRenderer>().material.color = Model.Props[ Model.Lens ].Color;
    }

    private void SetSymbol()
    {
        /**/
        if( !Model.isExplored.Value )
            return;
            /**/

        if( Model.Lens == R.Default )
            SymbolText.text = "<color=\"#007800\">" + Model.Props[ R.Energy ].Value
                    + "</color> <color=\"#000ff0\">" + Model.Props[ R.Science ].Value
                    + "</color>\n<color=\"#ff0000\">" + Model.Props[ R.Minerals ].Value
                    + "</color>";
        else
            SymbolText.text = Math.Round( Model.Props[ Model.Lens ].Value, 2 ).ToString();
        
        //SymbolText.text = Model.X + "," + Model.Y; //Show coordinates;

        SymbolText.gameObject.transform.position = new Vector3( SymbolText.gameObject.transform.position.x, (float)Model.Props[ R.Altitude ].Value + .01f, SymbolText.gameObject.transform.position.z );
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
