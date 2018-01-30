using AccidentalNoise;
using System;
using UniRx;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public GameObject HexagonPrefab;
    public IntReactiveProperty width = new IntReactiveProperty( 64 );
    public IntReactiveProperty height = new IntReactiveProperty( 40 );

    [RangeReactiveProperty( 1, 10 )]
    public IntReactiveProperty ExtrudeLevel = new IntReactiveProperty( 2 );
    [RangeReactiveProperty( 0, 1 )]
    public DoubleReactiveProperty SeaLevel = new DoubleReactiveProperty( 0 );
    [ RangeReactiveProperty( 1, 10 )]
    public IntReactiveProperty TerrainOctaves = new IntReactiveProperty( 1 );
    [RangeReactiveProperty( 0.1f, 2 )]
    public DoubleReactiveProperty TerrainFrequency = new DoubleReactiveProperty( 0.1 );
    public IntReactiveProperty Seed = new IntReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 4 )]
    public IntReactiveProperty _FractalType = new IntReactiveProperty( 0 );
    [RangeReactiveProperty( 0, 4 )]
    public IntReactiveProperty _BasisType = new IntReactiveProperty( 0 );
    [RangeReactiveProperty( 0, 3 )]
    public IntReactiveProperty _InterpolationType = new IntReactiveProperty( 0 );

    private HexMapModel mapModel;
    
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
        ExtrudeLevel.Subscribe( _ => ReDraw() );
        SeaLevel.Subscribe( _ => ReDraw() );
    }
    
    private void GenerateMap()
    {
        mapModel = new HexMapModel( width.Value, height.Value );
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
        
        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                // WRAP ON BOTH AXIS
                // Noise range
                float x1 = 0, x2 = 2;
                float y1 = 0, y2 = 2;
                float dx = x2 - x1;
                float dy = y2 - y1;

                // Sample noise at smaller intervals
                float s = x / (float)mapModel.Width;
                float t = y / (float)mapModel.Height;

                // Calculate our 4D coordinates
                float nx = x1 + Mathf.Cos( s * 2 * Mathf.PI ) * dx / ( 2 * Mathf.PI );
                float ny = y1 + Mathf.Cos( t * 2 * Mathf.PI ) * dy / ( 2 * Mathf.PI );
                float nz = x1 + Mathf.Sin( s * 2 * Mathf.PI ) * dx / ( 2 * Mathf.PI );
                float nw = y1 + Mathf.Sin( t * 2 * Mathf.PI ) * dy / ( 2 * Mathf.PI );

                float heightValue = (float)Math.Round((HeightMap.Get( nx, ny, nz, nw ) + 1) / 2, 1);
                //float heatValue = (float)HeatMap.Get( nx, ny, nz, nw );
                //float moistureValue = (float)MoistureMap.Get( nx, ny, nz, nw );

                // keep track of the max and min values found
                if( heightValue > mapModel.heightMap.Max )
                    mapModel.heightMap.Max = heightValue;
                if( heightValue < mapModel.heightMap.Min )
                    mapModel.heightMap.Min = heightValue;
                /*
                if( heatValue > HeatData.Max )
                    HeatData.Max = heatValue;
                if( heatValue < HeatData.Min )
                    HeatData.Min = heatValue;

                if( moistureValue > MoistureData.Max )
                    MoistureData.Max = moistureValue;
                if( moistureValue < MoistureData.Min )
                    MoistureData.Min = moistureValue;
                    */
                mapModel.heightMap.Table[ x, y ] = heightValue;
                
                //HeatData.Data[ x, y ] = heatValue;
                //MoistureData.Data[ x, y ] = moistureValue;
                
            }
        }
        Debug.Log( mapModel.heightMap.Min + ":::" + mapModel.heightMap.Max );
    }

    private void DrawTiles()
    {
        float xOffset = 0.866f;
        float zOffset = 0.749f;

        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                float xPos = x * xOffset;
                // Are we on an odd row?
                if( y % 2 == 1 )
                {
                    xPos += xOffset / 2f;
                }
                
                GameObject hex_go = (GameObject)Instantiate( 
                    HexagonPrefab, 
                    new Vector3( xPos, 0, y * zOffset ), 
                    Quaternion.identity );

                hex_go.name = "Hex_" + x + "_" + y;
                // Make sure the hex is aware of its place on the map
                HexModel model = new HexModel();
                model.X = x;
                model.Y = y;
                model.Altitude = mapModel.heightMap.Table[ x, y ];
                model.SeaLevel = (float)SeaLevel.Value;
                hex_go.GetComponent<Hex>().SetModel( model );

                // For a cleaner hierachy, parent this hex to the map
                hex_go.transform.SetParent( this.transform );

                // TODO: Quill needs to explain different optimization later...
                hex_go.isStatic = true;
            }
        }
    }

    void ReDraw()
    {
        RemoveAllChildren();
        GenerateMap();
        DrawTiles();
    }

    private void RemoveAllChildren()
    {
        GameObject go;
        while( gameObject.transform.childCount != 0 )
        {
            go = gameObject.transform.GetChild( 0 ).gameObject;
            go.transform.SetParent( null );
            DestroyImmediate( go );
        }
    }
}
