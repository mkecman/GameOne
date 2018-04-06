using PsiPhi;
using System.Collections.Generic;
using System;

public class PlanetPropsUpdateCommand : IGameInit
{
    private PlanetController _planetController;
    private PlanetModel _planet;
    private Dictionary<R, Dictionary<float, WeightedValue>> _weights;
    private Dictionary<R, List<WeightedValue>> _weightsList;
    private Dictionary<R, float> _totalValues;

    private WeightedValue _weightedValue;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
    }

    public void Execute()
    {
        _planet = _planetController.SelectedPlanet;
        _weights = new Dictionary<R, Dictionary<float, WeightedValue>>();
        _weightsList = new Dictionary<R, List<WeightedValue>>();
        _totalValues = new Dictionary<R, float>();
        InitializeDictionaries( R.Temperature );
        InitializeDictionaries( R.Pressure );
        InitializeDictionaries( R.Humidity );
        InitializeDictionaries( R.Radiation );

        HexModel hex;

        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                hex = _planet.Map.Table[ x ][ y ];

                AddInitialValue( R.Temperature, PPMath.Round( hex.Props[ R.Temperature ].Value ) );
                AddInitialValue( R.Pressure, PPMath.Round( hex.Props[ R.Pressure ].Value ) );
                AddInitialValue( R.Humidity, PPMath.Round( hex.Props[ R.Humidity ].Value ) );
                AddInitialValue( R.Radiation, PPMath.Round( hex.Props[ R.Radiation ].Value ) );
            }
        }

        for( int i = 0; i < 100; i++ )
        {
            float key = PPMath.Round( i / 100f );
            CreateList( R.Temperature, key );
            CreateList( R.Pressure, key );
            CreateList( R.Humidity, key );
            CreateList( R.Radiation, key );
        }

        SetValues( R.Temperature );
        SetValues( R.Pressure );
        SetValues( R.Humidity );
        SetValues( R.Radiation );
    }

    private void InitializeDictionaries( R type )
    {
        _weights.Add( type, new Dictionary<float, WeightedValue>() );
        _weightsList.Add( type, new List<WeightedValue>() );
        _totalValues.Add( type, 0 );
    }

    private void AddInitialValue( R type, float key )
    {

        if( _weights[ type ].ContainsKey( key ) )
        {
            _weightedValue = _weights[ type ][ key ];
            _weightedValue.Weight += 1;
        }
        else
            _weights[ type ].Add( key, new WeightedValue( key, 1 ) );

        _totalValues[ type ] += key;
    }

    private void CreateList( R type, float key )
    {
        if( _weights[ type ].ContainsKey( key ) )
        {
            _weightedValue = _weights[ type ][ key ];
            _weightedValue.Weight = _weights[ type ][ key ].Weight / _weights[ type ].Count;
        }
        else
            _weightedValue = new WeightedValue( key, 0 );

        _weightsList[ type ].Add( _weightedValue );
    }

    private void SetValues( R type )
    {
        _planet.Props[ type ].HexDistribution = _weightsList[ type ];
        _planet.Props[ type ].AvgValue = _totalValues[ type ] / _planet.Map.Count;
    }
}
