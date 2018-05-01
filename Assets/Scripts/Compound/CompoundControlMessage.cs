public class CompoundControlMessage
{
    public int Index;
    public CompoundControlAction Action = CompoundControlAction.ADD;
}

public enum CompoundControlAction
{
    ADD,
    REMOVE
}
