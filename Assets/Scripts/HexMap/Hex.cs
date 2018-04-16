using System;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hex : GameView
{
    public HexModel _model;
    public TextMesh SymbolText;
    public GameObject Gas;
    public GameObject Liquid;
    public GameObject Solid;
    public Gradient Gradient1;
    public Gradient Gradient2;
    public Gradient Gradient3;
    public Gradient Gradient4;

    private HexClickedMessage _HexClickedMessage;
    private GameDebug _debug;
    private List<ElementModel> _elements;
    private StringBuilder labelSB = new StringBuilder();

    private void Awake()
    {
        _HexClickedMessage = new HexClickedMessage( null );
        _debug = GameModel.Get<GameDebug>();
        _elements = GameConfig.Get<ElementConfig>().Elements;
    }

    public void SetModel( HexModel model )
    {
        this._model = model;
        _HexClickedMessage.Hex = _model;

        SetHeight( Solid, _model.Props[ R.Altitude ].Value );

        disposables.Clear();
        _model.ObserveEveryValueChanged( _ => _.Lens ).Subscribe( _ => { SetColor(); SetSymbol(); } ).AddTo( disposables );

        _model.Props[ R.HexScore ]._Value.Subscribe( _ => OnHexScoreChange() ).AddTo( disposables );
        
        _model.isMarked.Skip( 1 ).Subscribe( _ => UpdateMarkedColor( _ ) ).AddTo( disposables );
        _model.isExplored.Subscribe( _ => SetSymbol() ).AddTo( disposables );

        OnHexScoreChange();
    }

    private void OnHexScoreChange()
    {
        _model.Props[ R.Default ].Color = (
            Gradient1.Evaluate( _model.Props[ R.Temperature ].Value ) +
            Gradient2.Evaluate( _model.Props[ R.Pressure ].Value ) +
            Gradient3.Evaluate( _model.Props[ R.Humidity ].Value ) +
            Gradient4.Evaluate( _model.Props[ R.Radiation ].Value ) 
            ) / 4;

        Color temp = Gradient1.Evaluate( _model.Props[ R.Temperature ].Value );
        temp = AddColor( temp, Gradient2.Evaluate( _model.Props[ R.Pressure ].Value ) );
        temp = AddColor( temp, Gradient3.Evaluate( _model.Props[ R.Humidity ].Value ) );
        temp = AddColor( temp, Gradient4.Evaluate( _model.Props[ R.Radiation ].Value ) );

        _model.Props[ R.Default ].Color = temp;
        _model.Props[ R.Element ].Color = temp;

        SetColor();
        //SetSymbol();
    }

    private Color AddColor( Color original, Color addition )
    {
        return Color.Lerp( original, addition, addition.a );
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
        Gas.GetComponent<MeshRenderer>().material.color = new Color32( 255, 255, 255, (byte)(_model.Props[ R.Humidity ].Value * 255) );
    }

    private void SetColor()
    {
        /**/
        if( _model.isExplored.Value || _debug.isActive )
            Solid.GetComponent<MeshRenderer>().material.color = _model.Props[ _model.Lens ].Color;
        else
            Solid.GetComponent<MeshRenderer>().material.color = Color.gray;
        /**/
    }

    private void SetSymbol()
    {
        /**/
        if( !_debug.isActive && !_model.isExplored.Value )
            return;
        /**/

        if( _model.Lens == R.Default )
        {
            //SymbolText.text = Math.Round( _model.Props[ R.Minerals ].Value, 0 ).ToString();
            SymbolText.text = _elements[ (int)_model.Props[ R.Element ].Value ].Symbol;
            /*
            labelSB.Clear();
            
            if( Model.Props[ R.Energy ].Value > 0 )
                labelSB.Append( "<color=\"#007800\">" + Math.Round( Model.Props[ R.Energy ].Value, 0 ).ToString() +"</color>");
            if( Model.Props[ R.Science ].Value > 0 )
                labelSB.Append( "<color=\"#000ff0\">" + Math.Round( Model.Props[ R.Science ].Value, 0 ).ToString() + "</color>" );
            if( Model.Props[ R.Minerals ].Value > 0 )
                labelSB.Append( "<color=\"#ff0000\">" + Math.Round( Model.Props[ R.Minerals ].Value, 0 ).ToString() + "</color>" );

            SymbolText.text = labelSB.ToString();
            */
        }
        else
        if( _model.Lens == R.Element )
            SymbolText.text = ( (int)_elements[ (int)_model.Props[ R.Element ].Value ].Weight).ToString();
        else
            SymbolText.text = Math.Round( _model.Props[ _model.Lens ].Value, 2 ).ToString();
        
        //SymbolText.text = Model.X + "," + Model.Y; //Show coordinates;

        SymbolText.gameObject.transform.position = new Vector3( SymbolText.gameObject.transform.position.x, _model.Props[ R.Altitude ].Value + .01f, SymbolText.gameObject.transform.position.z );
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
