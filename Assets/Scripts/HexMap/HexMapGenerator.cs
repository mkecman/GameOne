using UnityEngine;
using System.Collections;
using AccidentalNoise;
using System;
using System.Collections.Generic;
using UniRx;

public class HexMapGenerator
{
    private GridModel<HexModel> _map;
    private Vector2[] Ranges = new Vector2[ 7 ];
    private List<ElementModel> _elements;

    public GridModel<HexModel> Generate()
    {
        GameObject go = GameObject.Find( "Map" );
        HexMap hexMap = go.GetComponent<HexMap>();
        _map = new GridModel<HexModel>( hexMap.width.Value, hexMap.height.Value );

        _elements = Config.Get<ElementConfig>().Elements;
        Vector2 defaultRange = new Vector2( float.MaxValue, float.MinValue );
        for( int i = 0; i < 7; i++ )
        {
            Ranges[ i ] = defaultRange;
        }

        GenerateMap( hexMap );
        return _map;
    }

    private void GenerateMap( HexMap m )
    {
        // Generate AltitudeMap
        ImplicitFractal AltitudeMap = GetFractal( m.AltitudeFractal );

        // Generate TemperatureMap
        ImplicitGradient gradient = new ImplicitGradient( 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 );
        ImplicitCombiner TemperatureMap = new ImplicitCombiner( CombinerType.MULTIPLY );
        TemperatureMap.AddSource( gradient );
        TemperatureMap.AddSource( AltitudeMap );

        // Generate PressureMap
        ImplicitCombiner PressureMap = new ImplicitCombiner( CombinerType.MULTIPLY );
        PressureMap.AddSource( gradient );
        PressureMap.AddSource( GetFractal( m.PressureFractal ) );

        // Generate HumidityMap
        ImplicitFractal HumidityMap = GetFractal( m.HumidityFractal );

        // Generate RadiationMap
        ImplicitFractal RadiationMap = GetFractal( m.RadiationFractal );

        HexModel hex;

        for( int x = 0; x < _map.Width; x++ )
        {
            for( int y = 0; y < _map.Height; y++ )
            {
                hex = new HexModel
                {
                    X = x,
                    Y = y,
                    Lens = m.Lens
                };
                _map.Table[ x, y ] = hex;

                // WRAP ON BOTH AXIS
                // Noise range
                float x1 = 0, x2 = 2;
                float y1 = 0, y2 = 2;
                float dx = x2 - x1;
                float dy = y2 - y1;

                // Sample noise at smaller intervals
                float s = x / (float)_map.Width;
                float t = y / (float)_map.Height;

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
                SetInitialHexValue( hex, R.Altitude, altitude );
                SetInitialHexValue( hex, R.Temperature, temperature );
                SetInitialHexValue( hex, R.Pressure, pressure );
                SetInitialHexValue( hex, R.Humidity, humidity );
                SetInitialHexValue( hex, R.Radiation, radiation );
            }
        }

        //Normalize Ranges to 0-1
        for( int x = 0; x < _map.Width; x++ )
        {
            for( int y = 0; y < _map.Height; y++ )
            {
                hex = _map.Table[ x, y ];
                SetHex( hex, R.Altitude );
                hex.Props[ R.Altitude ].Value *= 2;

                SetHex( hex, R.Temperature );
                SetHex( hex, R.Pressure );
                SetHex( hex, R.Humidity );
                SetHex( hex, R.Radiation );
                
                /*
                if( hex.Altitude >= m.SeaLevel.Value )
                    //hex.Colors[ (int)HexMapLens.Normal ] = TerrainGradient.Evaluate( (float)( ( 1 - hex.Temperature ) / ( 1 - SeaLevel.Value ) ) );
                    hex.Colors[ (int)HexMapLens.Normal ] = m.TerrainGradient.Evaluate( (float)( ( 1 / ( 1 - m.SeaLevel.Value ) ) * ( 1 - hex.Temperature ) ) );
                else
                    hex.Colors[ (int)HexMapLens.Normal ] = m.LiquidGradient.Evaluate( (float)( ( 1 - hex.Temperature ) + m.SeaLevel.Value ) );
                    */

                hex.Props[ R.Default ].Color = m.TerrainGradient.Evaluate( (float)( hex.Props[ R.Temperature ].Value ) );

                hex.Props[ R.HexScore ].Value = 0;
                hex.Props[ R.HexScore ].Color = Color.red;
            }
        }
    }

    private void SetHex( HexModel hex, R type )
    {
        //normalize value to be between 0 - 1
        hex.Props[ type ].Value = Math.Round( Mathf.InverseLerp( Ranges[ (int)type ].x, Ranges[ (int)type ].y, (float)hex.Props[ type ].Value ), 2 );
        hex.Props[ type ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ type ].Value );
    }
    
    private void SetInitialHexValue( HexModel hex, R lens, float value )
    {
        hex.Props[ lens ].Value = value;

        if( value > Ranges[ (int)lens ].y )
            Ranges[ (int)lens ].y = value;
        if( value < Ranges[ (int)lens ].x )
            Ranges[ (int)lens ].x = value;
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
