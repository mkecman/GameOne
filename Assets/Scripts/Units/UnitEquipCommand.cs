using UniRx;

public class UnitEquipCommand : IGameInit
{
    private CompoundConfig _compounds;
    private UnitController _unitController;

    public void Init()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        _unitController = GameModel.Get<UnitController>();
    }

    public void Execute( int compoundIndex, int bodySlotIndex )
    {
        _unitController.SelectedUnit.BodySlots[ bodySlotIndex ].CompoundIndex = compoundIndex;
    }
}
