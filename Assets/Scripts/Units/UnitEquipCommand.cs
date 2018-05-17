using PsiPhi;
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
    private CompoundJSON _compound;
    private UnitDefenseUpdateCommand _unitDefenseUpdateCommand;

    public void Init()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        _unitController = GameModel.Get<UnitController>();
        _planetController = GameModel.Get<PlanetController>();
        _message = new CompoundControlMessage( 0, CompoundControlAction.ADD, false );
        _unitDefenseUpdateCommand = GameModel.Get<UnitDefenseUpdateCommand>();
    }

    public void ExecuteEquip( int compoundIndex, int bodySlotIndex )
    {
        _unitSlotCompoundIndex = _unitController.SelectedUnit.BodySlots[ bodySlotIndex ]._CompoundIndex;
        _lifeCompounds = _planetController.SelectedPlanet.Life.Compounds;

        Unequip();

        //EQUIP
        _unitSlotCompoundIndex.Value = compoundIndex;
        _lifeCompounds[ compoundIndex ].Value--;

        ApplyCompoundEffects( _compounds[ compoundIndex ], 1 );
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
            _message.Index = _unitSlotCompoundIndex.Value;
            GameMessage.Send( _message );

            _compound = _compounds[ _unitSlotCompoundIndex.Value ];
            _unitSlotCompoundIndex.Value = Int32.MaxValue;

            ApplyCompoundEffects( _compound, -1 );
        }
    }

    private void ApplyCompoundEffects( CompoundJSON compound, int multiplier = -1 )
    {
        if( compound.Type == CompoundType.Armor )
        {
            foreach( KeyValuePair<R, float> item in compound.Effects )
            {
                if( item.Key == R.Temperature || item.Key == R.Pressure || item.Key == R.Humidity || item.Key == R.Radiation)
                    _unitController.SelectedUnit.Resistance[ item.Key ].ChangePosition( PPMath.Round( ( multiplier * item.Value ) / 100f ) );
                else
                    _unitController.SelectedUnit.Props[ item.Key ].Value += (int)item.Value * multiplier;
            }

            _unitDefenseUpdateCommand.Execute( _unitController.SelectedUnit );
        }

        if( compound.Type == CompoundType.Weapon )
        {
            foreach( KeyValuePair<R, float> item in compound.Effects )
            {
                if( item.Key == R.Temperature || item.Key == R.Pressure || item.Key == R.Humidity || item.Key == R.Radiation )
                    _unitController.SelectedUnit.Impact[ item.Key ].Value += (int)item.Value * multiplier;
                else
                    _unitController.SelectedUnit.Props[ item.Key ].Delta += (int)item.Value * multiplier;
            }
        }

    }
}
