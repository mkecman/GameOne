public class CompoundControlMessage
{
    public int Index;
    public CompoundControlAction Action = CompoundControlAction.ADD;
    public bool SpendCurrency = true;
}

public enum CompoundControlAction
{
    ADD,
    REMOVE
}
