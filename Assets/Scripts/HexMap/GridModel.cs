public class GridModel<T>
{
    public T[][] Table;
    public int Width;
    public int Height;
    public float Min;
    public float Max;

    public GridModel( int width, int height )
    {
        Table = new T[ width ][];
        for( int x = 0; x < width; x++ )
        {
            Table[ x ] = new T[ height ];
        }
        Width = width;
        Height = height;
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public GridModel(){}
}
