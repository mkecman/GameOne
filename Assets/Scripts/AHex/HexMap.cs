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
    private HexMapModel mapModel;
    private List<ElementModel> _elements;

    private HexConfig _hexConfig;
    
    // Use this for initialization
    private void Start()
    {
        _elements = Config.Get<ElementConfig>().Elements;
        _hexConfig = Config.Get<HexConfig>();
        GameModel.Bind<LifeModel>( OnLifeModelChange );
    }

    private void OnLifeModelChange( LifeModel value )
    {
        _lifeModel = value;
        ReDraw();
    }

    private void GenerateMap()
    {
        mapModel = new HexMapModel( width.Value, height.Value );
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

                float altitude = (float)AltitudeMap.Get( nx, ny, nz, nw );
                float temperature = (float)( 1 - TemperatureMap.Get( nx, ny, nz, nw ) );
                float pressure = (float)PressureMap.Get( nx, ny, nz, nw );
                float humidity = (float)HumidityMap.Get( nx, ny, nz, nw );
                float radiation = (float)RadiationMap.Get( nx, ny, nz, nw );

                // keep track of the max and min values found
                SetMinMax( altitude, mapModel.AltitudeMap );
                SetMinMax( temperature, mapModel.TemperatureMap );
                SetMinMax( pressure, mapModel.PressureMap );
                SetMinMax( humidity, mapModel.HumidityMap );
                SetMinMax( radiation, mapModel.RadiationMap );
                
                mapModel.AltitudeMap.Table[ x, y ] = altitude;
                mapModel.TemperatureMap.Table[ x, y ] = temperature;
                mapModel.PressureMap.Table[ x, y ] = pressure;
                mapModel.HumidityMap.Table[ x, y ] = humidity;
                mapModel.RadiationMap.Table[ x, y ] = radiation;

            }
        }

        //Normalize Ranges to 0-1
        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                Normalize( mapModel.AltitudeMap, x, y );
                Normalize( mapModel.TemperatureMap, x, y );
                Normalize( mapModel.PressureMap, x, y );
                Normalize( mapModel.HumidityMap, x, y );
                Normalize( mapModel.RadiationMap, x, y );
                
                if( mapModel.AltitudeMap.Table[ x, y ] >= SeaLevel.Value )
                    mapModel.ColorMap.Table[ x, y ] = TerrainGradient.Evaluate( (float)( ( 1 - mapModel.TemperatureMap.Table[ x, y ] ) * ( 1 - SeaLevel.Value ) ) );
                else
                    mapModel.ColorMap.Table[ x, y ] = LiquidGradient.Evaluate( (float)( ( 1 - mapModel.TemperatureMap.Table[ x, y ] ) + SeaLevel.Value ) );

                mapModel.ElementMap.Table[ x, y ] = _elements[ (int)Math.Round( ( _elements.Count - 2 ) * mapModel.TemperatureMap.Table[ x, y ], 0 ) + 1 ];
            }
        }

    }

    private void Normalize( GridModel<float> map, int x, int y )
    {
        map.Table[ x, y ] = (float)Math.Round( Mathf.InverseLerp( map.Min, map.Max, map.Table[ x, y ] ), 2 );
    }

    private void SetMinMax( float value, GridModel<float> map )
    {
        if( value > map.Max )
            map.Max = value;
        if( value < map.Min )
            map.Min = value;
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
                // Make sure the hex is aware of its place on the map
                HexModel model = new HexModel();
                model.X = x;
                model.Y = y;
                model.Altitude = mapModel.AltitudeMap.Table[ x, y ];
                model.Temperature = mapModel.TemperatureMap.Table[ x, y ];
                model.Pressure = mapModel.PressureMap.Table[ x, y ];
                model.Humidity = mapModel.HumidityMap.Table[ x, y ];
                model.Radiation = mapModel.RadiationMap.Table[ x, y ];
                model.Color = mapModel.ColorMap.Table[ x, y ];
                model.Element = mapModel.ElementMap.Table[ x, y ];
                model.TotalScore = ( _lifeModel.TemperatureBC.GetValueAt( model.Temperature ) +
                                     _lifeModel.PressureBC.GetValueAt( model.Pressure ) +
                                     _lifeModel.HumidityBC.GetValueAt( model.Humidity ) +
                                     _lifeModel.RadiationBC.GetValueAt( model.Radiation ) ) / 4;
                model.Lens = Lens;
                mapModel.HexMap.Table[ x, y ] = model;

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

    public void ReDraw()
    {
        Debug.Log( "REDRAWING MAP!" );
        RemoveAllChildren();
        GenerateMap();
        DrawTiles();
        GameModel.Register<HexMapModel>( mapModel );
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
