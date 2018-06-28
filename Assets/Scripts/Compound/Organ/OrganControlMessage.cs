public class OrganControlMessage
{
    public int CompoundIndex;
    public int BodySlotIndex;
    public OrganControlAction Action = OrganControlAction.OPEN_PANEL;
}

public enum OrganControlAction
{
    OPEN_PANEL,
    CRAFT
}
