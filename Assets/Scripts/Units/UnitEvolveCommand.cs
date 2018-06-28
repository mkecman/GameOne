using UnityEngine;
using System.Collections;
using PsiPhi;
using System;

public class UnitEvolveCommand : IGameInit
{
    private int _currentBodySlotIndex;
    private int _currentBodySlotCompoundIndex;
    private UnitEquipCommand _unitEquipCommand;
    private UnitController _unitController;
    private UnitModel _unit;

    public void Init()
    {
        _unitEquipCommand = GameModel.Get<UnitEquipCommand>();
        _unitController = GameModel.Get<UnitController>();
    }

    public void OpenPanel( int bodySlotIndex )
    {
        _unit = _unitController.SelectedUnit;
        _currentBodySlotIndex = bodySlotIndex;
        _currentBodySlotCompoundIndex = _unit.BodySlots[ _currentBodySlotIndex ].CompoundIndex;
        //if slot is already occupied, show its children
        if( _currentBodySlotCompoundIndex != Int32.MaxValue )
            GameModel.Set<TreeBranchData>( _unit.OrganTree.GetBranch( _currentBodySlotCompoundIndex ) );
        else
            GameModel.Set<TreeBranchData>( _unit.OrganTree );

        GameMessage.Send( new PanelMessage( PanelAction.SHOW, PanelNames.OrgansPanel ) );
    }

    public void ApplyCompound( int compoundIndex )
    {
        _unitEquipCommand.ExecuteEquip( compoundIndex, _currentBodySlotIndex, false );

        //activate branch and unlock its children
        TreeBranchData treeBranchData = _unit.OrganTree.GetBranch( compoundIndex );
        treeBranchData.State = TreeBranchState.ACTIVE;
        foreach( TreeBranchData child in treeBranchData.Children )
        {
            child.State = TreeBranchState.UNLOCKED;
        }
    }

}
