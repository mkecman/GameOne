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
    private Color[] _colors;

    public void SetModel( HexModel model )
    {
        this.Model = model;
        _HexClickedMessage = new HexClickedMessage( model );

        SetHeight( Solid, Model.Props[ R.Altitude ].Value );

        disposables.Clear();
        Model.isMarked.Skip( 1 ).Subscribe( _ => UpdateMarkedColor( _ ) ).AddTo( disposables );
        Model.ObserveEveryValueChanged( _ => _.Lens ).Subscribe( _ => { SetColor(); SetSymbol(); } ).AddTo( disposables );

        Model.Props[ R.Temperature ]._Value.Subscribe( _ => OnTemperatureChange() ).AddTo( disposables );
        Model.Props[ R.Humidity ]._Value.Subscribe( _ => OnTemperatureChange() ).AddTo( disposables );

        Model.isExplored.Subscribe( _ => SetSymbol() ).AddTo( disposables );



        OnTemperatureChange();

        //SetSymbol();
        //SetClouds();
    }

    private void OnTemperatureChange()
    {
        /*
        Model.Props[ R.Default ].Color = new Color( 
            (float)( (1 - Model.Props[ R.Altitude ].Value )), 
            (float)( Model.Props[ R.Humidity ].Value ), 
            (float)( Model.Props[ R.Temperature ].Value ) );
        *//*
        Model.Props[ R.Default ].Color = Color.Lerp( 
            DefaultColorGradient.Evaluate( (float)( Model.Props[ R.Temperature ].Value ) ), 
            DefaultColorGradient2.Evaluate( (float)( Model.Props[ R.Humidity ].Value ) ), 
            0.5f );
        /**/
        Color red = new Color( (float)Model.Props[ R.Altitude ].Value, 0, 0 );
        Color green = new Color( 0, (float)Model.Props[ R.Humidity ].Value, 0 );
        Color blue = new Color( 0, 0, (float)Model.Props[ R.Temperature ].Value );
        _colors = new Color[] { red, green, blue };
        Model.Props[ R.Default ].Color = InterpolateColor( _colors, 1 );
        SetColor();
        SetSymbol();
    }

    private Color InterpolateColor( Color[] colors, double x )
    {
        double r = 0.0, g = 0.0, b = 0.0;
        double total = 0.0;
        double step = 1.0 / (double)( colors.Length - 1 );
        double mu = 0.0;
        double sigma_2 = 1;

        foreach( Color color in colors )
        {
            total += Math.Exp( -( x - mu ) * ( x - mu ) / ( 2.0 * sigma_2 ) ) / Math.Sqrt( 2.0 * Math.PI * sigma_2 );
            mu += step;
        }

        mu = 0.0;
        foreach( Color color in colors )
        {
            double percent = Math.Exp( -( x - mu ) * ( x - mu ) / ( 2.0 * sigma_2 ) ) / Math.Sqrt( 2.0 * Math.PI * sigma_2 );
            mu += step;

            r += color.r * percent / total;
            g += color.g * percent / total;
            b += color.b * percent / total;
        }

        return new Color( (float)r, (float)g, (float)b );
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
        Gas.GetComponent<MeshRenderer>().material.color = new Color32( 255, 255, 255, (byte)( Model.Props[ R.Humidity ].Value * 255 ) );
    }

    private void SetColor()
    {
        Solid.GetComponent<MeshRenderer>().material.color = Model.Props[ Model.Lens ].Color;
    }

    private void SetSymbol()
    {
        /*
        if( !Model.isExplored.Value )
            return;
            */
        switch( Model.Lens )
        {
            case R.Default:
                SymbolText.text = "<color=\"#007800\">" + Model.Props[ R.Energy ].Value
                    + "</color> <color=\"#000ff0\">" + Model.Props[ R.Science ].Value
                    + "</color>\n<color=\"#ff0000\">" + Model.Props[ R.Minerals ].Value
                    + "</color>";
                //SymbolText.text = "<color=\"#007800\">" + (int)Model.Element.Weight + "</color>\n<color=\"#ff0000\">" + ( Model.TotalScore * 100 ) + "%</color>";
                //SymbolText.text = Math.Round( Model.TotalScore * Model.Element.Weight, 2 ).ToString() + "";
                break;
            default:
                SymbolText.text = Math.Round( Model.Props[ Model.Lens ].Value, 2 ).ToString();
                break;
        }

        //SymbolText.text = Model.Element.Symbol;
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
