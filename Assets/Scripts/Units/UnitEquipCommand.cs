using System;
using System.Collections.Generic;
using UniRx;

public class UnitEquipCommand : IGameInit
{
    private CompoundConfig _compounds;
    private UnitController _unitController;
    private PlanetController _planetController;
    private CompoundControlMessage _message;
    private IntReactiveProperty _unitSlotCompoundIndex;
    private ReactiveDictionary<int, IntReactiveProperty> _lifeCompounds;

    public void Init()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        _unitController = GameModel.Get<UnitController>();
        _planetController = GameModel.Get<PlanetController>();
        _message = new CompoundControlMessage();
        _message.SpendCurrency = false;
    }

    public void ExecuteEquip( int compoundIndex, int bodySlotIndex )
    {
        _unitSlotCompoundIndex = _unitController.SelectedUnit.BodySlots[ bodySlotIndex ]._CompoundIndex;
        _lifeCompounds = _planetController.SelectedPlanet.Life.Compounds;

        Unequip();

        //EQUIP
        _unitSlotCompoundIndex.Value = compoundIndex;
        _lifeCompounds[ compoundIndex ].Value--;

        foreach( KeyValuePair<R, float> item in _compounds[ compoundIndex ].Effects )
        {
            _unitController.SelectedUnit.Resistance[ item.Key ].ChangePosition( item.Value / 100f );
        }
    }

    public void ExecuteUnequip( int bodySlotIndex )
    {
        _unitSlotCompoundIndex = _unitController.SelectedUnit.BodySlots[ bodySlotIndex ]._CompoundIndex;
        _lifeCompounds = _planetController.SelectedPlanet.Life.Compounds;

        Unequip();
    }

    private void Unequip()
    {
        if( _unitSlotCompoundIndex.Value != Int32.MaxValue )
        {
            foreach( KeyValuePair<R, float> item in _compounds[ _unitSlotCompoundIndex.Value ].Effects )
            {
                _unitController.SelectedUnit.Resistance[ item.Key ].ChangePosition( -item.Value / 100f );
            }

            _message.Index = _unitSlotCompoundIndex.Value;
            GameMessage.Send( _message );

            _unitSlotCompoundIndex.Value = Int32.MaxValue;
        }
    }
}
