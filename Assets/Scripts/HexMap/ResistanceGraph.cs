using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class ResistanceGraph : GameView, IPointerClickHandler
{
    public R Lens;
    public BellCurveTexture Gradient;
    public RawImage TileValue;
    public Text PropertyText;
    public Text MatchText;

    private RectTransform _tileValueRectTransform;


    private BellCurve _BellCurve = new BellCurve( .01f, 0f, 0.01f );

    void Start()
    {
        _tileValueRectTransform = TileValue.GetComponent<RectTransform>();
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );

    }

    private void UpdateView( double xPos )
    {
        _tileValueRectTransform.anchoredPosition = new Vector2( ( (float)xPos - 0.5f ) * Gradient.Width, 0 );
        MatchText.text = (int)Math.Round( _BellCurve.GetValueAt( xPos ) * 100, 0 ) + "%";
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        disposables.Clear();
        value.Hex.Props[ Lens ]._Value.Subscribe( _ => UpdateView( _ ) ).AddTo( disposables );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _BellCurve = value.Life.Resistance[ Lens ];
        Gradient.Draw( _BellCurve );
        
        PropertyText.text = Lens.ToString();
        MatchText.text = "0%";
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
