public class OrganControlMessage
{
    public int CompoundIndex;
    public OrganControlAction Action;

    public OrganControlMessage( OrganControlAction action = OrganControlAction.OPEN_PANEL )
    {
        Action = action;
    }
}

public enum OrganControlAction
{
    OPEN_PANEL,
    CRAFT
}
