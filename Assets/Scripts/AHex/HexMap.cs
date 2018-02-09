using AccidentalNoise;
using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour
{
    public HexMapLens Lens;

    public GameObject HexagonPrefab;
    public GameObject Ocean;

    public Gradient TerrainGradient;
    public Gradient LiquidGradient;

    [Header( "Size Values" )]
    public IntReactiveProperty width = new IntReactiveProperty( 64 );
    public IntReactiveProperty height = new IntReactiveProperty( 40 );

    [Header( "Liquid Level" )]
    [RangeReactiveProperty( 0, 1 )]
    public DoubleReactiveProperty SeaLevel = new DoubleReactiveProperty( 0 );

    [Header( "Maps" )]
    public FractalModel AltitudeFractal;
    //public FractalModel TemperatureFractal;
    public FractalModel PressureFractal;
    public FractalModel HumidityFractal;
    public FractalModel RadiationFractal;


    private LifeModel _lifeModel;
    private GridModel<HexModel> mapModel;
    private List<ElementModel> _elements;

    private HexConfig _hexConfig;
    private Vector2[] Ranges = new Vector2[ 7 ];

    // Use this for initialization
    private void Start()
    {
        _elements = Config.Get<ElementConfig>().Elements;
        _hexConfig = Config.Get<HexConfig>();
        Vector2 defaultRange = new Vector2( float.MaxValue, float.MinValue );
        for( int i = 0; i < 7; i++ )
        {
            Ranges[ i ] = defaultRange;
        }
        //Ranges[ HexMapLens.Altitude ] = 
        GameModel.Bind<LifeModel>( OnLifeModelChange );
    }

    private void OnLifeModelChange( LifeModel value )
    {
        _lifeModel = value;
        ReDraw();
    }

    private void GenerateMap()
    {
        mapModel = new GridModel<HexModel>( width.Value, height.Value );
        // Generate AltitudeMap
        ImplicitFractal AltitudeMap = GetFractal( AltitudeFractal );

        // Generate TemperatureMap
        ImplicitGradient gradient = new ImplicitGradient( 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 );
        ImplicitCombiner TemperatureMap = new ImplicitCombiner( CombinerType.MULTIPLY );
        TemperatureMap.AddSource( gradient );
        TemperatureMap.AddSource( AltitudeMap );

        // Generate PressureMap
        ImplicitCombiner PressureMap = new ImplicitCombiner( CombinerType.MULTIPLY );
        PressureMap.AddSource( gradient );
        PressureMap.AddSource( GetFractal( PressureFractal ) );

        // Generate HumidityMap
        ImplicitFractal HumidityMap = GetFractal( HumidityFractal );

        // Generate RadiationMap
        ImplicitFractal RadiationMap = GetFractal( RadiationFractal );

        HexModel hex;

        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                hex = new HexModel
                {
                    X = x,
                    Y = y,
                    Lens = Lens
                };
                mapModel.Table[ x, y ] = hex;

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

                float altitude = (float)AltitudeMap.Get( nx, ny, nz, nw );
                float temperature = (float)( 1 - TemperatureMap.Get( nx, ny, nz, nw ) );
                float pressure = (float)PressureMap.Get( nx, ny, nz, nw );
                float humidity = (float)HumidityMap.Get( nx, ny, nz, nw );
                float radiation = (float)RadiationMap.Get( nx, ny, nz, nw );

                hex.Altitude = altitude;
                hex.Temperature = temperature;
                hex.Pressure = pressure;
                hex.Humidity = humidity;
                hex.Radiation = radiation;

                // keep track of the max and min values found
                SetMinMax( altitude, HexMapLens.Altitude );
                SetMinMax( temperature, HexMapLens.Temperature );
                SetMinMax( pressure, HexMapLens.Pressure );
                SetMinMax( humidity, HexMapLens.Humidity );
                SetMinMax( radiation, HexMapLens.Radiation );
            }
        }

        //Normalize Ranges to 0-1
        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                hex = mapModel.Table[ x, y ];

                hex.Altitude = Normalize( HexMapLens.Altitude, hex.Altitude );
                hex.Temperature = Normalize( HexMapLens.Temperature, hex.Temperature );
                hex.Pressure = Normalize( HexMapLens.Pressure, hex.Pressure );
                hex.Humidity = Normalize( HexMapLens.Humidity, hex.Humidity );
                hex.Radiation = Normalize( HexMapLens.Radiation, hex.Radiation );
                
                if( hex.Altitude >= SeaLevel.Value )
                    hex.Colors[ (int)HexMapLens.Normal ] = TerrainGradient.Evaluate( (float)( ( 1 - hex.Temperature ) * ( 1 - SeaLevel.Value ) ) );
                else
                    hex.Colors[ (int)HexMapLens.Normal ] = LiquidGradient.Evaluate( (float)( ( 1 - hex.Temperature ) + SeaLevel.Value ) );

                hex.Element = _elements[ (int)Math.Round( ( _elements.Count - 2 ) * hex.Temperature, 0 ) + 1 ];
            }
        }

    }

    private float Normalize( HexMapLens lens, float value )
    {
        return (float)Math.Round( Mathf.InverseLerp( Ranges[ (int)lens ].x, Ranges[ (int)lens ].y, value ), 2 );
    }
        
    private void SetMinMax( float value, HexMapLens lens )
    {
        if( value > Ranges[ (int)lens ].y )
            Ranges[ (int)lens ].y = value;
        if( value < Ranges[ (int)lens ].x )
            Ranges[ (int)lens ].x = value;
    }

    private void DrawTiles()
    {
        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                GameObject hex_go = (GameObject)Instantiate(
                    HexagonPrefab,
                    new Vector3( HexMapHelper.GetXPosition(x,y), -0.1f, HexMapHelper.GetZPosition( y ) ),
                    Quaternion.identity );

                hex_go.name = "Hex_" + x + "_" + y;
                hex_go.GetComponent<Hex>().SetModel( mapModel.Table[ x, y ] );
                hex_go.transform.SetParent( this.transform );
                // TODO: Quill needs to explain different optimization later...
                hex_go.isStatic = true;
            }
        }

        Ocean.transform.localScale = new Vector3( ( mapModel.Width * _hexConfig.xOffset ) + 1, 1, ( mapModel.Height * _hexConfig.zOffset ) + 1 );
        Ocean.transform.position = new Vector3( ( mapModel.Width * _hexConfig.xOffset ) / 2, -.65f + ( 1.3f * (float)SeaLevel.Value ), ( ( mapModel.Height * _hexConfig.zOffset ) / 2 ) - 0.5f );
    }

    public void ReDraw()
    {
        Debug.Log( "REDRAWING MAP!" );
        RemoveAllChildren();
        GenerateMap();
        DrawTiles();
        GameModel.Register( mapModel );
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

    private ImplicitFractal GetFractal( FractalModel model )
    {
        return new ImplicitFractal( (FractalType)model.FractalType.Value,
                                           (BasisType)model.BasisType.Value,
                                           (InterpolationType)model.InterpolationType.Value,
                                           model.Octaves.Value,
                                           model.Frequency.Value,
                                           model.Seed.Value );
    }
}

public enum HexMapLens
{
    Normal,
    Altitude,
    Temperature,
    Pressure,
    Humidity,
    Radiation,
    TotalScore
}
