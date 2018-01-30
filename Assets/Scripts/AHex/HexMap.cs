using AccidentalNoise;
using UniRx;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public GameObject HexagonPrefab;
    public IntReactiveProperty width = new IntReactiveProperty( 64 );
    public IntReactiveProperty height = new IntReactiveProperty( 40 );

    [RangeReactiveProperty( 1, 10 )]
    public IntReactiveProperty TerrainOctaves = new IntReactiveProperty( 1 );
    [RangeReactiveProperty( 0.1f, 1 )]
    public DoubleReactiveProperty TerrainFrequency = new DoubleReactiveProperty( 0.1 );
    public IntReactiveProperty Seed = new IntReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 4 )]
    public IntReactiveProperty _FractalType = new IntReactiveProperty( 0 );
    [RangeReactiveProperty( 0, 4 )]
    public IntReactiveProperty _BasisType = new IntReactiveProperty( 0 );
    [RangeReactiveProperty( 0, 3 )]
    public IntReactiveProperty _InterpolationType = new IntReactiveProperty( 0 );

    private float xOffset = 0.882f;
    private float zOffset = 0.764f;

    // Use this for initialization
    private void Start()
    {
        width.Subscribe( _ => ReDraw() );
        height.Subscribe( _ => ReDraw() );
        TerrainOctaves.Subscribe( _ => ReDraw() );
        TerrainFrequency.Subscribe( _ => ReDraw() );
        Seed.Subscribe( _ => ReDraw() );
        _FractalType.Subscribe( _ => ReDraw() );
        _BasisType.Subscribe( _ => ReDraw() );
        _InterpolationType.Subscribe( _ => ReDraw() );
    }


    void ReDraw()
    {
        GameObject go;
        while( gameObject.transform.childCount != 0 )
        {
            go = gameObject.transform.GetChild( 0 ).gameObject;
            go.transform.SetParent( null );
            DestroyImmediate( go );
        }

        // Initialize the HeightMap Generator
        ImplicitFractal HeightMap = new ImplicitFractal( (FractalType)_FractalType.Value,
                                           (BasisType)_BasisType.Value,
                                           (InterpolationType)_InterpolationType.Value,
                                           TerrainOctaves.Value,
                                           TerrainFrequency.Value,
                                           Seed.Value );

        //CellularGenerator generator = new CellularGenerator();
        //generator.Seed = 1;
        //ImplicitCellular HeightMap = new ImplicitCellular( generator );

        double min = 0;
        double max = 0;

        for( int x = 0; x < width.Value; x++ )
        {
            for( int y = 0; y < height.Value; y++ )
            {
                float xPos = x * xOffset;
                // Are we on an odd row?
                if( y % 2 == 1 )
                {
                    xPos += xOffset / 2f;
                }

                //set altitude to be between 0,1 instead of -1,1
                double altitude = ( HeightMap.Get( x, y ) + 1 ) / 2;

                if( altitude < min )
                    min = altitude;
                if( altitude > max )
                    max = altitude;

                GameObject hex_go = (GameObject)Instantiate( HexagonPrefab, new Vector3( xPos, (float)altitude, y * zOffset ), Quaternion.Euler( new Vector3( 90, 0, 0 ) ) );
                hex_go.name = "Hex_" + x + "_" + y;
                // Make sure the hex is aware of its place on the map
                HexModel model = new HexModel();
                model.X = x;
                model.Y = y;
                model.Altitude = altitude;
                hex_go.GetComponent<Hex>().SetModel( model );

                // For a cleaner hierachy, parent this hex to the map
                hex_go.transform.SetParent( this.transform );

                // TODO: Quill needs to explain different optimization later...
                hex_go.isStatic = true;

            }
        }

        Debug.Log( "min: " + min + ":::::" + "max: " + max );

    }
}
