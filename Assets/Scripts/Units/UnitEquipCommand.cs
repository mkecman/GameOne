using System;
using System.Collections.Generic;
using UniRx;

public class UnitEquipCommand : IGameInit
{
    private CompoundConfig _compounds;
    private UnitController _unitController;
    private PlanetController _planetController;
    private CompoundControlMessage _message;
    private IntReactiveProperty _unitSlotCompound;
    private ReactiveDictionary<int, IntReactiveProperty> _lifeCompounds;

    public void Init()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        _unitController = GameModel.Get<UnitController>();
        _planetController = GameModel.Get<PlanetController>();
        _message = new CompoundControlMessage();
    }

    public void Execute( int compoundIndex, int bodySlotIndex )
    {
        _unitSlotCompound = _unitController.SelectedUnit.BodySlots[ bodySlotIndex ]._CompoundIndex;
        _lifeCompounds = _planetController.SelectedPlanet.Life.Compounds;

        if( _unitSlotCompound.Value != Int32.MaxValue )
        {

            _message.Index = _unitSlotCompound.Value;
            GameMessage.Send( _message );
        }

        _unitSlotCompound.Value = compoundIndex;
        _lifeCompounds[ compoundIndex ].Value--;

        foreach( KeyValuePair<R, float> item in _compounds[ compoundIndex ].Effects )
        {
            _unitController.SelectedUnit.Resistance[ item.Key ].ChangePosition( item.Value / 100f );
        }
    }
}
