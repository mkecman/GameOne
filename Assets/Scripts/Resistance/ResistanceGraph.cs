using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResistanceGraph : GameView, IPointerClickHandler
{
    public R Lens;
    public BellCurveTexture Gradient;
    public RawImage TileValue;
    public Text PropertyText;
    public Text MatchText;

    private RectTransform _tileValueRectTransform;
    private UnitModel _selectedUnit;
    private HexModel _hexModel;
    private LifeModel _life;

    void Start()
    {
        PropertyText.text = Lens.ToString();
    }

    void OnEnable()
    {
        if( _tileValueRectTransform == null )
            _tileValueRectTransform = TileValue.GetComponent<RectTransform>();

        GameModel.HandleGet<PlanetModel>( OnPlanetModel );
        GameModel.HandleGet<HexModel>( OnHexModelChange );
    }

    void OnDisable()
    {
        GameModel.RemoveHandle<PlanetModel>( OnPlanetModel );
        GameModel.RemoveHandle<HexModel>( OnHexModelChange );
    }

    private void OnPlanetModel( PlanetModel value )
    {
        _life = value.Life;
    }

    private void OnHexModelChange( HexModel value )
    {
        disposables.Clear();

        if( value != null )
        {
            _hexModel = value;
            _hexModel.Props[ Lens ]._Value.DelayFrame( 1 ).Subscribe( _ => UpdateView() ).AddTo( disposables );

            if( value.Unit != null )
            {
                _selectedUnit = value.Unit;
                //delay subscription to wait for BellCurveTexture Gradient to initialize
                _selectedUnit.Resistance[ Lens ].Position.DelayFrame( 1 ).Subscribe( _ => { UpdateView(); } ).AddTo( disposables );
            }
            else
                _selectedUnit = null;
        }
        else
            _hexModel = null;
    }

    private void UpdateView()
    {
        if( _selectedUnit != null )
            UpdateWith( _selectedUnit.Resistance[ Lens ], _hexModel.Props[ Lens ].Value );
        else
            UpdateWith( _life.Resistance[ Lens ], _hexModel.Props[ Lens ].Value );
    }

    private void UpdateWith( BellCurve bellCurve, double value )
    {
        Gradient.Draw( bellCurve );
        _tileValueRectTransform.anchoredPosition = new Vector2( ( (float)value - 0.5f ) * Gradient.Width, 0 );
        MatchText.text = (int)Math.Round( bellCurve.GetValueAt( value ) * 100, 0 ) + "%";
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
