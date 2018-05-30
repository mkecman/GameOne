using System;
using UnityEngine;

public class CoreSimulator : MonoBehaviour
{
    public int TimeDelta;
    private int _timeDelta;
    public SimulatorTimeEnum TimePreset = SimulatorTimeEnum.min_1;

    private PlanetModel _planet;
    private int _minUnitLifetime;
    private UniverseConfig _universeConfig;
    private UnitDefenseUpdateCommand _unitDefenseCommand;
    private PlanetPropsUpdateCommand _planetUpdateCommand;
    private HexScoreUpdateCommand _hexScoreUpdateCommand;
    private MineSkill _mineSkill;
    private LiveSkill _liveSkill;
    private Clock _clock;
    private DateTime _startStopWatch;

    // Use this for initialization
    void Start()
    {
        _universeConfig = GameConfig.Get<UniverseConfig>();
        _unitDefenseCommand = GameModel.Get<UnitDefenseUpdateCommand>();
        _planetUpdateCommand = GameModel.Get<PlanetPropsUpdateCommand>();
        _hexScoreUpdateCommand = GameModel.Get<HexScoreUpdateCommand>();
        _mineSkill = GameModel.Get<MineSkill>();
        _liveSkill = GameModel.Get<LiveSkill>();
        _clock = GameModel.Get<Clock>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameMessage.Listen<ClockTickMessage>( OnClockTick );

    }

    public void Run( int time )
    {
        _timeDelta = time;
        StartRunning();
    }

    private void OnClockTick( ClockTickMessage value )
    {
        Run( value.stepsPerTick );
    }

    private void StartRunning()
    {
        _startStopWatch = DateTime.Now;
        _clock.StopTimer();
        RunTime();
        _clock.message.elapsedTicksSinceStart += ( _timeDelta - 1 ); //-1 because Clock already increments elapsedTicksSinceStart
    }

    private void RunTime()
    {
        //find shortest unit's lifetime
        _minUnitLifetime = Int32.MaxValue;
        int unitLifetime = 0;
        foreach( UnitModel unit in _planet.Life.Units )
        {
            if( unit.Props[ R.Health ].Value <= 0 )
                continue;

            _unitDefenseCommand.ExecuteAverageArmorInTime( unit, _timeDelta );
            unitLifetime = (int)( unit.Props[ R.Health ].Value / unit.Props[ R.Armor ].Value ) + 1;
            if( _minUnitLifetime > unitLifetime )
                _minUnitLifetime = unitLifetime;
        }

        if( _minUnitLifetime < _timeDelta )
        {
            //if unit will die before we run out of time, update units for unit's lifetime value
            UpdateUnits( _minUnitLifetime );
            _timeDelta -= _minUnitLifetime;
            RunTime();
        }
        else
        {
            //else just run full time update
            UpdateUnits( _timeDelta );
            _hexScoreUpdateCommand.Execute();
            _clock.StartTimer();

            //uncomment to output time it takes for full simulation to be over
            //Debug.Log( "UpdateTime: " + ( DateTime.Now - _startStopWatch ).ToString() );
        }
    }

    private void UpdateUnits( int minUnitLifetime )
    {
        foreach( UnitModel unit in _planet.Life.Units )
        {
            //if( unit.Props[ R.Health ].Value > 0 )
            //{
            _liveSkill.ExecuteTime( minUnitLifetime, unit );
            _mineSkill.ExecuteTime( minUnitLifetime, unit );
            //}
        }

        _planetUpdateCommand.Execute();
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _planet = value;
    }

}

public enum SimulatorTimeEnum
{
    min_1 = 60,
    min_5 = 300,
    min_10 = 600,
    min_30 = 1800,
    min_60 = 3600,
    hrs_2 = 7200,
    hrs_8 = 28800,
    hrs_12 = 43200,
    hrs_24 = 86400
}
