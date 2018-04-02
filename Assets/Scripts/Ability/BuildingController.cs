using System.Collections.Generic;

public class BuildingController : AbstractController
{
    private PlanetModel _planet;
    private BuildingPaymentService _pay;
    private HexUpdateCommand _hexUpdateCommand;

    private HexModel hm;
    private BuildingModel bm;
    private int hexCount;

    private Dictionary<R,double> tempProps = new Dictionary<R,double>();

    public BuildingController()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        tempProps.Add( R.Temperature, 0 );
        tempProps.Add( R.Pressure, 0 );
        tempProps.Add( R.Humidity, 0 );
        tempProps.Add( R.Radiation, 0 );
    }

    private void OnClockTick( ClockTickMessage value )
    {
        tempProps[ R.Temperature ] = 0;
        tempProps[ R.Pressure ] = 0;
        tempProps[ R.Humidity ] = 0;
        tempProps[ R.Radiation ] = 0;

        for( int i = 0; i < _planet.Life.Buildings.Count; i++ )
        {
            bm = _planet.Life.Buildings[ i ];
            if( bm.State == BuildingState.ACTIVE )
            {
                if( _pay.BuyMaintenance( - bm.Effects[ R.Minerals ] ) )
                {
                    CollectEffectValue( R.Temperature, tempProps, bm.Effects );
                    CollectEffectValue( R.Pressure, tempProps, bm.Effects );
                    CollectEffectValue( R.Humidity, tempProps, bm.Effects );
                    CollectEffectValue( R.Radiation, tempProps, bm.Effects );
                }
                else
                {
                    bm.State = BuildingState.INACTIVE;
                }

            }
        }

        tempProps[ R.Temperature ] /= hexCount;
        tempProps[ R.Pressure ] /= hexCount;
        tempProps[ R.Humidity ] /= hexCount;
        tempProps[ R.Radiation ] /= hexCount;

        for( int width = 0; width < _planet.Map.Width; width++ )
        {
            for( int height = 0; height < _planet.Map.Height; height++ )
            {
                hm = _planet.Map.Table[ width ][ height ];
                hm.Props[ R.Temperature ].Value += tempProps[ R.Temperature ];
                hm.Props[ R.Pressure ].Value += tempProps[ R.Pressure ];
                hm.Props[ R.Humidity ].Value += tempProps[ R.Humidity ];
                hm.Props[ R.Radiation ].Value += tempProps[ R.Radiation ];
                if( hm.Unit != null )
                {
                    _hexUpdateCommand.Execute( hm.Unit.Resistance, hm );
                    hm.Unit.Props[ R.Health ].Value = hm.Props[ R.HexScore ].Value;
                }
                else
                    _hexUpdateCommand.Execute( _planet.Life.Resistance, hm );
            }
        }
    }

    private void CollectEffectValue( R type, Dictionary<R,double> tempProps, Dictionary<R, double> effects )
    {
        if( effects.ContainsKey( type ) )
            tempProps[ type ] += effects[ type ];
    }

    private void OnBuildingMessage( BuildingMessage value )
    {
        switch( value.State )
        {
            case BuildingState.UNLOCKED:
                Unlock( value.Index );
                Select( value.Index );
                break;
            case BuildingState.ACTIVE:
                Activate( value.X, value.Y );
                break;
            case BuildingState.INACTIVE:
                Deactivate( value.X, value.Y );
                break;
            case BuildingState.BUILDING:
                Build( value.Index, value.X, value.Y );
                break;
            case BuildingState.DEMOLISH:
                Demolish( value.X, value.Y );
                break;
            case BuildingState.SELECTED:
                Select( value.Index );
                break;
            default:
                break;
        }

        //GameModel.Set<BuildingModel>( _planet.Map.Table[ value.X, value.Y ].Building );
    }

    private void Select( int index )
    {
        //_planet.Life.BuildingsState[ index ].State = BuildingState.SELECTED;
        GameModel.Set<BuildingModel>( _planet.Life.BuildingsState[ index ] );
    }

    private void Build( int index, int x, int y )
    {
        if( _pay.BuyBuild( index ) )
        {
            BuildingModel building = GameModel.Copy( _planet.Life.BuildingsState[ index ] );
            building.State = BuildingState.ACTIVE;
            building.X = x;
            building.Y = y;
            building.Altitude = _planet.Map.Table[ x ][ y ].Props[ R.Altitude ].Value;
            _planet.Life.Buildings.Add( building );
            _planet.Map.Table[ x ][ y ].Building = building;
            GameModel.Set<HexModel>( _planet.Map.Table[ x ][ y ] );
        }
    }

    private void Demolish( int x, int y )
    {
        _planet.Life.Buildings.Remove( _planet.Map.Table[ x ][ y ].Building );
        _planet.Map.Table[ x ][ y ].Building = null;
        GameModel.Set<HexModel>( _planet.Map.Table[ x ][ y ] );
    }

    private void Activate( int x, int y )
    {
        _planet.Map.Table[ x ][ y ].Building.State = BuildingState.ACTIVE;
    }

    private void Deactivate( int x, int y )
    {
        _planet.Map.Table[ x ][ y ].Building.State = BuildingState.INACTIVE;
    }

    private void Unlock( int index )
    {
        if( _pay.BuyUnlock( index ) )
        {
            _planet.Life.BuildingsState[ index ].State = BuildingState.UNLOCKED;
            GameMessage.Send<BuildingUnlockMessage>( new BuildingUnlockMessage( index ) );
        }
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _planet = value;
        hexCount = _planet.Map.Width * _planet.Map.Height;

        BuildingModel building;
        for( int i = 0; i < _planet.Life.Buildings.Count; i++ )
        {
            building = _planet.Life.Buildings[ i ];
            _planet.Map.Table[ building.X ][ building.Y ].Building = building;
        }

        if( _pay == null )
            _pay = GameModel.Get<BuildingPaymentService>();

        if( _hexUpdateCommand == null )
            _hexUpdateCommand = GameModel.Get<HexUpdateCommand>();

        GameMessage.Listen<ClockTickMessage>( OnClockTick );
        GameMessage.Listen<BuildingMessage>( OnBuildingMessage );
    }

}
