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
        PropertyText.text = Lens.ToString();
    }

    void OnEnable()
    {
        if( _tileValueRectTransform == null )
            _tileValueRectTransform = TileValue.GetComponent<RectTransform>();

        GameModel.HandleGet<HexModel>( OnHexModelChange );
    }

    void OnDisable()
    {
        GameModel.RemoveHandle<HexModel>( OnHexModelChange );
    }
    
    private void OnHexModelChange( HexModel value )
    {
        disposables.Clear();

        if( value != null && value.Unit != null )
        {
            _hexModel = value;
            _selectedUnit = value.Unit;

            _hexModel.Props[ Lens ]._Value.Subscribe( _ => UpdateView() ).AddTo( disposables );

            //delay subscription to wait for BellCurveTexture Gradient to initialize
            _selectedUnit.Resistance[ Lens ].Position.DelaySubscription( TimeSpan.FromTicks(1) ).Subscribe( _ => { Gradient.Draw( _selectedUnit.Resistance[ Lens ] ); UpdateView(); } ).AddTo( disposables );
        }
    }
    
    private void UpdateView()
    {
        _tileValueRectTransform.anchoredPosition = new Vector2( ( (float)_hexModel.Props[ Lens ].Value - 0.5f ) * Gradient.Width, 0 );
        MatchText.text = (int)Math.Round( _selectedUnit.Resistance[ Lens ].GetValueAt( _hexModel.Props[ Lens ].Value ) * 100, 0 ) + "%";
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
