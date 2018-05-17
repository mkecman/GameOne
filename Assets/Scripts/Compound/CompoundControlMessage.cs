public class CompoundControlMessage
{
    public int Index;
    public CompoundControlAction Action = CompoundControlAction.ADD;
    public bool SpendCurrency = true;

    public CompoundControlMessage( int index, CompoundControlAction action, bool spendCurrency = true )
    {
        Index = index;
        Action = action;
        SpendCurrency = spendCurrency;
    }
}

public enum CompoundControlAction
{
    ADD,
    REMOVE
}
