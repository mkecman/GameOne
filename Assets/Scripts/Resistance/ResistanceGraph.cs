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
    private CompositeDisposable planetDisposable = new CompositeDisposable();

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
        disposables.Clear();
        planetDisposable.Clear();
    }

    private void OnPlanetModel( PlanetModel value )
    {
        _planet = value;
        planetDisposable.Clear();
        _planet.Props[ Lens ]._AvgValue.Subscribe( _ => UpdateView() ).AddTo( planetDisposable );
    }

    private void OnHexModelChange( HexModel value )
    {
        disposables.Clear();

        if( value != null )
        {
            _hexModel = value;

            if( value.Unit != null )
            {
                _selectedUnit = value.Unit;
                _selectedUnit.Resistance[ Lens ].Consumption.Subscribe( _ => { UpdateView(); } ).AddTo( disposables );
            }
            else
                _selectedUnit = null;

            //_hexModel.Props[ Lens ]._Value.Subscribe( _ => UpdateView() ).AddTo( disposables );
        }
        else
        {
            _hexModel = null;
            _selectedUnit = null;
        }
    }

    private void UpdateView()
    {
        if( _selectedUnit != null )
        {
            Gradient.Draw( _selectedUnit.Resistance[ Lens ] );
            UpdateUI( (int)Math.Round( _selectedUnit.Resistance[ Lens ].GetFloatAt( _hexModel.Props[ Lens ].Value ) * 100, 0 ) + "%", _hexModel.Props[ Lens ].Value );
        }
        else
            if( _hexModel == null )
            {
                Gradient.Draw( _planet.Props[ Lens ].HexDistribution );
                UpdateUI( _planet.Props[ Lens ].AvgValue.ToString( "N2" ), _planet.Props[ Lens ].AvgValue );
            }
            else
            {
                Gradient.Draw( _planet.Props[ Lens ].HexDistribution );
                UpdateUI( _planet.Props[ Lens ].AvgValue.ToString( "N2" ), _hexModel.Props[ Lens ].Value);
            }
    }

    private void UpdateUI( string match, float value )
    {
        MatchText.text = match;
        _tileValueRectTransform.anchoredPosition = new Vector2( ( value - 0.5f ) * Gradient.Width, 0 );
    }
    
    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
