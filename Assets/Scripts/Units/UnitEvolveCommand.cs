using UnityEngine;
using System.Collections;

public class UnitEvolveCommand : IGameInit
{
    private int _currentBodySlotIndex;
    private UnitEquipCommand _unitEquipCommand;

    public void Init()
    {
        _unitEquipCommand = GameModel.Get<UnitEquipCommand>();
    }

    public void OpenPanel( int bodySlotIndex )
    {
        _currentBodySlotIndex = bodySlotIndex;
        GameMessage.Send( new PanelMessage( PanelAction.SHOW, PanelNames.OrgansPanel ) );
    }

    public void Craft( int compoundIndex )
    {
        _unitEquipCommand.ExecuteEquip( compoundIndex, _currentBodySlotIndex, false );
    }

}
