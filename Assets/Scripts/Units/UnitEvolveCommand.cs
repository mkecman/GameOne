using PsiPhi;

public class UnitEvolveCommand : IGameInit
{
    private UnitEquipCommand _unitEquipCommand;
    private UnitController _unitController;
    private CompoundPaymentService _pay;
    private CompoundConfig _compounds;

    public void Init()
    {
        _unitEquipCommand = GameModel.Get<UnitEquipCommand>();
        _unitController = GameModel.Get<UnitController>();
        _pay = GameModel.Get<CompoundPaymentService>();
        _compounds = GameConfig.Get<CompoundConfig>();
    }

    public void OpenPanel()
    {
        GameModel.Set<TreeBranchData>( _unitController.SelectedUnit.OrganTree );
        GameMessage.Send( new PanelMessage( PanelAction.SHOW, PanelNames.OrgansPanel ) );
    }

    public void ApplyCompound( int compoundIndex )
    {
        if( _pay.BuyCompound( compoundIndex ) )
        {
            _unitEquipCommand.ApplyCompoundEffects( compoundIndex, 1 );

            //activate branch and unlock its children
            TreeBranchData treeBranchData = _unitController.SelectedUnit.OrganTree.GetBranch( compoundIndex );
            treeBranchData.State = TreeBranchState.ACTIVE;

            foreach( TreeBranchData child in treeBranchData.Children )
            {
                if( _compounds[ child.Index ].CanCraft )
                    child.State = TreeBranchState.AVAILABLE;
                else
                    child.State = TreeBranchState.UNLOCKED;
            }
        }
    }

}
