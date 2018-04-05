using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanetPropGraph : GameView, IPointerClickHandler
{
    public R Lens;
    public WeightedTexture Texture;
    public RawImage TileValue;
    public Text PropertyText;
    public Text MatchText;

    private RectTransform _tileValueRectTransform;
    private PlanetModel _planet;

    void Start()
    {
        PropertyText.text = Lens.ToString();
    }

    void OnEnable()
    {
        if( _tileValueRectTransform == null )
            _tileValueRectTransform = TileValue.GetComponent<RectTransform>();

        GameModel.HandleGet<PlanetModel>( OnPlanetModel );
    }

    void OnDisable()
    {
        GameModel.RemoveHandle<PlanetModel>( OnPlanetModel );
    }

    private void OnPlanetModel( PlanetModel value )
    {
        disposables.Clear();
        _planet = value;
        _planet.Props[ Lens ]._AvgValue.Subscribe( _ => UpdateView() ).AddTo( disposables );
    }

    private void UpdateView()
    {
        if( _planet.Props[ Lens ].HexDistribution != null )
        {
            Texture.Draw( _planet.Props[ Lens ].HexDistribution );
            _tileValueRectTransform.anchoredPosition = new Vector2( ( _planet.Props[ Lens ].AvgValue - 0.5f ) * Texture.Width, 0 );
            MatchText.text = _planet.Props[ Lens ].AvgValue.ToString( "F2" );
        }
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
