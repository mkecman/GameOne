public class CompoundEquipMessage
{
    public int CompoundIndex;
    public int BodySlotIndex;
    public CompoundEquipAction Action;

    public CompoundEquipMessage( int compoundIndex, int bodySlotIndex, CompoundEquipAction equipAction = CompoundEquipAction.EQUIP )
    {
        CompoundIndex = compoundIndex;
        BodySlotIndex = bodySlotIndex;
        Action = equipAction;
    }
}

public enum CompoundEquipAction
{
    EQUIP,
    UNEQUIP
}
