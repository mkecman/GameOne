using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResistanceGraph : GameView, IPointerClickHandler
{
    public R Lens;
    public GradientTextureView Gradient;
    public RawImage TileValue;
    public Text PropertyText;
    public Text MatchText;

    private RectTransform _tileValueRectTransform;
    private UnitModel _selectedUnit;
    private HexModel _hexModel;
    private PlanetModel _planet;
    private RectTransform _gradientRect;

    void Awake()
    {
        PropertyText.text = Lens.ToString();
        _gradientRect = Gradient.GetComponent<RectTransform>();
        _tileValueRectTransform = TileValue.GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModel );
        GameModel.HandleGet<HexModel>( OnHexModelChange );
    }

    void OnDisable()
    {
        GameModel.RemoveHandle<PlanetModel>( OnPlanetModel );
        GameModel.RemoveHandle<HexModel>( OnHexModelChange );
        disposables.Clear();
    }

    private void OnPlanetModel( PlanetModel value )
    {
        _planet = value;
        OnHexModelChange( null );
    }

    private void OnHexModelChange( HexModel hex )
    {
        disposables.Clear();

        if( hex != null )
        {
            _hexModel = hex;

            if( _hexModel.Unit != null )
            {
                _selectedUnit = _hexModel.Unit;
                _selectedUnit.Resistance[ Lens ].Consumption.Subscribe( _ =>
                {
                    Gradient.Draw( _selectedUnit.Resistance[ Lens ] );
                    UpdateUI( _selectedUnit.Resistance[ Lens ].GetIntAt( _hexModel.Props[ Lens ].Value ) + "%", _hexModel.Props[ Lens ].Value );
                } ).AddTo( disposables );
            }
            else
            {
                _selectedUnit = null;
                _hexModel.Props[ Lens ]._Value.Subscribe( _ =>
                {
                    Gradient.Draw( _planet.Props[ Lens ].HexDistribution );
                    UpdateUI( _hexModel.Props[ Lens ].Value.ToString( "N2" ), _hexModel.Props[ Lens ].Value );
                } ).AddTo( disposables );
            }

        }
        else
        {
            _hexModel = null;
            _selectedUnit = null;
            _planet.Props[ Lens ]._AvgValue.Subscribe( _ =>
            {
                Gradient.Draw( _planet.Props[ Lens ].HexDistribution );
                UpdateUI( _planet.Props[ Lens ].AvgValue.ToString( "N2" ), (float)_planet.Props[ Lens ].AvgValue );
            } ).AddTo( disposables );
        }

    }

    private void UpdateUI( string match, float value )
    {
        MatchText.text = match;
        _tileValueRectTransform.anchoredPosition = new Vector2( ( value - 0.5f ) * _gradientRect.rect.width, 0 );
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
