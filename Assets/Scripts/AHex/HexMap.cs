using AccidentalNoise;
using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour
{
    public GameObject HexagonPrefab;
    public GameObject Ocean;

    public Gradient TerrainGradient;
    public Gradient LiquidGradient;

    [Header( "Size Values" )]
    public IntReactiveProperty width = new IntReactiveProperty( 64 );
    public IntReactiveProperty height = new IntReactiveProperty( 40 );

    [Header( "Terrain Map Values" )]
    [RangeReactiveProperty( 0, 1 )]
    public DoubleReactiveProperty SeaLevel = new DoubleReactiveProperty( 0 );
    [RangeReactiveProperty( 1, 10 )]
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

    [Header( "Heat Map Values" )]
    [RangeReactiveProperty( 1, 10 )]
    public IntReactiveProperty HeatOctaves = new IntReactiveProperty( 1 );
    [RangeReactiveProperty( 0.1f, 2 )]
    public DoubleReactiveProperty HeatFrequency = new DoubleReactiveProperty( 0.1 );
    public IntReactiveProperty HeatSeed = new IntReactiveProperty( 0 );
    [RangeReactiveProperty( 0, 4 )]
    public IntReactiveProperty _HeatFractalType = new IntReactiveProperty( 0 );
    [RangeReactiveProperty( 0, 4 )]
    public IntReactiveProperty _HeatBasisType = new IntReactiveProperty( 0 );
    [RangeReactiveProperty( 0, 3 )]
    public IntReactiveProperty _HeatInterpolationType = new IntReactiveProperty( 0 );
    
    private HexMapModel mapModel;
    private List<ElementModel> _elements;

    private HexConfig _hexConfig;
    private bool _canRedraw = false;

    // Use this for initialization
    private void Start()
    {
        _elements = Config.Get<ElementConfig>().Elements;
        _hexConfig = Config.Get<HexConfig>();
        GameMessage.Listen<ClockTickMessage>( OnClockTick );
        AddSubscribers();
        ReDraw();
    }

    private void OnClockTick( ClockTickMessage value )
    {
        
    }

    private void AddSubscribers()
    {
        _canRedraw = false;
        width.Subscribe( _ => ReDraw() );
        height.Subscribe( _ => ReDraw() );
        TerrainOctaves.Subscribe( _ => ReDraw() );
        TerrainFrequency.Subscribe( _ => ReDraw() );
        Seed.Subscribe( _ => ReDraw() );
        _FractalType.Subscribe( _ => ReDraw() );
        _BasisType.Subscribe( _ => ReDraw() );
        _InterpolationType.Subscribe( _ => ReDraw() );

        HeatOctaves.Subscribe( _ => ReDraw() );
        HeatFrequency.Subscribe( _ => ReDraw() );
        HeatSeed.Subscribe( _ => ReDraw() );
        _HeatFractalType.Subscribe( _ => ReDraw() );
        _HeatBasisType.Subscribe( _ => ReDraw() );
        _HeatInterpolationType.Subscribe( _ => ReDraw() );

        SeaLevel.Subscribe( _ => ReDraw() );
        _canRedraw = true;
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

        // Heat Map
        ImplicitGradient gradient = new ImplicitGradient( 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 );
        ImplicitFractal heatFractal = new ImplicitFractal( (FractalType)_HeatFractalType.Value,
                                           (BasisType)_HeatBasisType.Value,
                                           (InterpolationType)_HeatInterpolationType.Value,
                                           HeatOctaves.Value,
                                           HeatFrequency.Value,
                                           HeatSeed.Value );
        ImplicitCombiner HeatMap = new ImplicitCombiner( CombinerType.MULTIPLY );
        HeatMap.AddSource( gradient );
        HeatMap.AddSource( HeightMap );

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

                float heightValue = (float)Math.Round( HeightMap.Get( nx, ny, nz, nw ), 2 );
                float heatValue = (float)Math.Round( HeatMap.Get( nx, ny, nz, nw ), 2 );
                //float moistureValue = (float)MoistureMap.Get( nx, ny, nz, nw );

                // keep track of the max and min values found
                if( heightValue > mapModel.heightMap.Max )
                    mapModel.heightMap.Max = heightValue;
                if( heightValue < mapModel.heightMap.Min )
                    mapModel.heightMap.Min = heightValue;

                if( heatValue > mapModel.heatMap.Max )
                    mapModel.heatMap.Max = heatValue;
                if( heatValue < mapModel.heatMap.Min )
                    mapModel.heatMap.Min = heatValue;
                /*
                if( moistureValue > MoistureData.Max )
                    MoistureData.Max = moistureValue;
                if( moistureValue < MoistureData.Min )
                    MoistureData.Min = moistureValue;
                    */
                mapModel.heightMap.Table[ x, y ] = heightValue;
                mapModel.heatMap.Table[ x, y ] = heatValue;
                //MoistureData.Data[ x, y ] = moistureValue;
            }
        }

        //Normalize Ranges to 0-1
        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                mapModel.heightMap.Table[ x, y ] = Mathf.InverseLerp( mapModel.heightMap.Min, mapModel.heightMap.Max, mapModel.heightMap.Table[ x, y ] );
                mapModel.heatMap.Table[ x, y ] = Mathf.InverseLerp( mapModel.heatMap.Min, mapModel.heatMap.Max, mapModel.heatMap.Table[ x, y ] );

                if( mapModel.heightMap.Table[ x, y ] >= SeaLevel.Value )
                    mapModel.colorMap.Table[ x, y ] = TerrainGradient.Evaluate( (float)( mapModel.heatMap.Table[ x, y ] * ( 1 - SeaLevel.Value ) ) );
                else
                    mapModel.colorMap.Table[ x, y ] = LiquidGradient.Evaluate( (float)( mapModel.heatMap.Table[ x, y ] + SeaLevel.Value ) );

                mapModel.elementMap.Table[ x, y ] = _elements[ (int)Math.Round( ( _elements.Count - 1 ) * mapModel.heatMap.Table[ x, y ], 0 ) ];
            }
        }

    }

    private void DrawTiles()
    {
        

        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                float xPos = x * _hexConfig.xOffset;
                // Are we on an odd row?
                if( y % 2 == 1 )
                {
                    xPos += _hexConfig.xOffset / 2f;
                }

                GameObject hex_go = (GameObject)Instantiate(
                    HexagonPrefab,
                    new Vector3( xPos, -0.1f, y * _hexConfig.zOffset ),
                    Quaternion.identity );

                hex_go.name = "Hex_" + x + "_" + y;
                // Make sure the hex is aware of its place on the map
                HexModel model = new HexModel();
                model.X = x;
                model.Y = y;
                model.Altitude = mapModel.heightMap.Table[ x, y ];
                model.Color = mapModel.colorMap.Table[ x, y ];
                model.Element = mapModel.elementMap.Table[ x, y ];
                hex_go.GetComponent<Hex>().SetModel( model );

                // For a cleaner hierachy, parent this hex to the map
                hex_go.transform.SetParent( this.transform );

                // TODO: Quill needs to explain different optimization later...
                hex_go.isStatic = true;
            }
        }

        Ocean.transform.localScale = new Vector3( ( mapModel.Width * _hexConfig.xOffset ) + 1, 1, ( mapModel.Height * _hexConfig.zOffset ) + 1 );
        Ocean.transform.position = new Vector3( ( mapModel.Width * _hexConfig.xOffset ) / 2, -.65f + ( 1.3f * (float)SeaLevel.Value ), ( ( mapModel.Height * _hexConfig.zOffset ) / 2 ) - 0.5f );
    }

    void ReDraw()
    {
        if( _canRedraw )
        {
            Debug.Log( "REDRAWING MAP!" );
            RemoveAllChildren();
            GenerateMap();
            DrawTiles();
            GameModel.Register<HexMapModel>( mapModel );
        }
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
