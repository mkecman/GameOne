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
    private UnitModel _selectedUnit;
    private HexModel _hexModel;

    void Start()
    {
        _tileValueRectTransform = TileValue.GetComponent<RectTransform>();
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
        GameModel.HandleGet<UnitModel>( OnUnitModelChange );

        PropertyText.text = Lens.ToString();
        MatchText.text = "0%";
    }
    
    private void UpdateView()
    {
        _tileValueRectTransform.anchoredPosition = new Vector2( ( (float)_hexModel.Props[ Lens ].Value - 0.5f ) * Gradient.Width, 0 );
        MatchText.text = (int)Math.Round( _selectedUnit.Resistance[ Lens ].GetValueAt( _hexModel.Props[ Lens ].Value ) * 100, 0 ) + "%";
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        _hexModel = value.Hex;
        UpdateSubscription();
    }

    private void OnUnitModelChange( UnitModel value )
    {
        _selectedUnit = value;
        UpdateSubscription();
    }

    private void UpdateSubscription()
    {
        disposables.Clear();
        if( _hexModel != null && _selectedUnit != null )
        {
            _hexModel.Props[ Lens ]._Value.Subscribe( _ => UpdateView() ).AddTo( disposables );
            _selectedUnit.Resistance[ Lens ].Position.Subscribe( _ => { Gradient.Draw( _selectedUnit.Resistance[ Lens ] ); UpdateView(); } ).AddTo( disposables );
        }
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
