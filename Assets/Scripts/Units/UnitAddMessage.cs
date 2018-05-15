public class UnitAddMessage
{
    public int UnitTypeIndex;
    public int X, Y;

    public UnitAddMessage( int unitTypeIndex, int x, int y )
    {
        UnitTypeIndex = unitTypeIndex;
        X = x;
        Y = y;
    }
}
