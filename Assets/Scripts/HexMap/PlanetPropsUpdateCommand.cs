using PsiPhi;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlanetPropsUpdateCommand : IGameInit
{
    private PlanetController _planetController;
    private HexUpdateCommand _hexUpdateCommand;
    private PlanetModel _planet;

    private Dictionary<R, List<WeightedValue>> _weightsList;
    private Dictionary<R, float> _totalValues;
    private HexModel hex;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _hexUpdateCommand = GameModel.Get<HexUpdateCommand>();

        _weightsList = new Dictionary<R, List<WeightedValue>>();
        _totalValues = new Dictionary<R, float>();

        InitializeDictionaries( R.Temperature );
        InitializeDictionaries( R.Pressure );
        InitializeDictionaries( R.Humidity );
        InitializeDictionaries( R.Radiation );
    }

    public void Execute()
    {
        _planet = _planetController.SelectedPlanet;

        Reset();

        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                hex = _planet.Map.Table[ x ][ y ];
                _hexUpdateCommand.Execute( hex );

                AddInitialValue( R.Temperature, Mathf.RoundToInt( hex.Props[ R.Temperature ].Value * 100 ) );
                AddInitialValue( R.Pressure, Mathf.RoundToInt( hex.Props[ R.Pressure ].Value * 100 ) );
                AddInitialValue( R.Humidity, Mathf.RoundToInt( hex.Props[ R.Humidity ].Value * 100 ) );
                AddInitialValue( R.Radiation, Mathf.RoundToInt( hex.Props[ R.Radiation ].Value * 100 ) );
            }
        }
        
        SetValues( R.Temperature );
        SetValues( R.Pressure );
        SetValues( R.Humidity );
        SetValues( R.Radiation );
    }

    private void Reset()
    {
        for( int i = 0; i <= 100; i++ )
        {
            _weightsList[ R.Temperature ][ i ].Weight = 0;
            _weightsList[ R.Pressure ][ i ].Weight = 0;
            _weightsList[ R.Humidity ][ i ].Weight = 0;
            _weightsList[ R.Radiation ][ i ].Weight = 0;
        }
        _totalValues[ R.Temperature ] = 0;
        _totalValues[ R.Pressure ] = 0;
        _totalValues[ R.Humidity ] = 0;
        _totalValues[ R.Radiation ] = 0;
    }
    
    private void InitializeDictionaries( R type )
    {
        _weightsList.Add( type, new List<WeightedValue>() );
        for( int i = 0; i <= 100; i++ )
            _weightsList[ type ].Add( new WeightedValue( PPMath.Round( i / 100f ), 0 ) );

        _totalValues.Add( type, 0 );
    }

    private void AddInitialValue( R type, int key )
    {
        _weightsList[ type ][ key ].Weight = PPMath.Round( _weightsList[ type ][ key ].Weight + .05f );
        _totalValues[ type ] += key;
    }

    private void SetValues( R type )
    {
        _planet.Props[ type ].HexDistribution = _weightsList[ type ];
        _planet.Props[ type ].AvgValue = ( _totalValues[ type ] / 100f ) / _planet.Map.Count;
    }
}
