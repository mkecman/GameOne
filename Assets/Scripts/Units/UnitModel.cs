using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System;

public class UnitModel : IDisposable
{
    [SerializeField]
    internal IntReactiveProperty _X = new IntReactiveProperty();
    public int X
    {
        get { return _X.Value; }
        set {
                Position.x = HexMapHelper.GetXPosition( value, Y );
                Position.y = Props[ R.Altitude ].Value;
                _X.Value = value;
            }
    }

    [SerializeField]
    internal IntReactiveProperty _Y = new IntReactiveProperty();
    public int Y
    {
        get { return _Y.Value; }
        set {
                Position.x = HexMapHelper.GetXPosition( X, value );
                Position.y = Props[ R.Altitude ].Value;
                Position.z = HexMapHelper.GetZPosition( value );
                _Y.Value = value;
            }
    }
    
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    //public Dictionary<int, BuildingState> Abilities = new Dictionary<int, BuildingState>();
    //public Dictionary<R, FloatReactiveProperty> AbilitiesDelta = new Dictionary<R, FloatReactiveProperty>();

    public Dictionary<R, BellCurve> Resistance = new Dictionary<R,BellCurve>();
    public Dictionary<R, IntReactiveProperty> Impact = new Dictionary<R, IntReactiveProperty>();

    public Dictionary<R,Resource> Props = new Dictionary<R,Resource>();

    public Dictionary<int, BodySlotModel> BodySlots = new Dictionary<int, BodySlotModel>();

    internal Vector3 Position = new Vector3();

    public Dictionary<int, SkillData> Skills = new Dictionary<int, SkillData>();
    public List<int> PassiveSkills = new List<int>();
    public List<int> ActiveSkills = new List<int>();

    private CompositeDisposable disposables = new CompositeDisposable();
    private BodySlotsConfig _slotsConfig;
    private LevelUpConfig _levelUpConfig;
    private List<int> _slots;
    private LevelUpModel _levelUpModel;

    public UnitModel()
    {
        Impact.Add( R.Temperature, new IntReactiveProperty() );
        Impact.Add( R.Pressure, new IntReactiveProperty() );
        Impact.Add( R.Humidity, new IntReactiveProperty() );
        Impact.Add( R.Radiation, new IntReactiveProperty() );

        _levelUpConfig = GameConfig.Get<LevelUpConfig>();
        _slotsConfig = GameConfig.Get<BodySlotsConfig>();
        _slots = _slotsConfig[ 0 ];
        for( int i = 0; i < _slots.Count; i++ )
            BodySlots.Add( i, new BodySlotModel( i, _slots[ i ] == 1 ? true : false ) );
    }

    public void Setup()
    {
        _levelUpModel = _levelUpConfig[ (int)Props[ R.Level ].Value ];

        Props[ R.Body ]._Value.Subscribe( _ => { UpdateBodyStats(); } ).AddTo( disposables );
        Props[ R.Mind ]._Value.Subscribe( _ => UpdateMindStats() ).AddTo( disposables );
        Props[ R.Soul ]._Value.Subscribe( _ => UpdateSoulStats() ).AddTo( disposables );

        //Props[ R.Experience ].MaxValue = _levelUpModel.Experience;
        Props[ R.Experience ]._Delta.Subscribe( _ => CheckLevelUp() ).AddTo( disposables );

        Props[ R.Attack ]._Delta.Subscribe( _ => UpdateMindStats() ).AddTo( disposables );
    }

    private void UpdateMindStats()
    {
        Props[ R.Attack ].Value = Props[ R.Mind ].Value + Props[ R.Attack ].Delta;
    }

    private void UpdateSoulStats()
    {
        Props[ R.Health ].MaxValue = Mathf.CeilToInt( Mathf.Pow( Props[ R.Soul ].Value, 0.65f ) * 400f );
        Props[ R.Critical ].Value = Props[ R.Soul ].Value / 300f;
    }

    private void UpdateBodyStats()
    {
        _slots = _slotsConfig[ Mathf.CeilToInt( Mathf.Pow( Props[ R.Body ].Value, 0.5f ) * 1.1236f ) ];
        for( int i = 0; i < _slots.Count; i++ )
        {
            BodySlots[ i ].IsEnabled = _slots[ i ] == 1 ? true : false;
            if( !BodySlots[ i ].IsEnabled && BodySlots[ i ].CompoundIndex != Int32.MaxValue )
                GameModel.Get<UnitEquipCommand>().ExecuteUnequip( i );
        }
    }

    private void CheckLevelUp()
    {
        int newLevel = Mathf.Clamp( Mathf.FloorToInt( ( 5f / 3f ) * Mathf.Sqrt( 5f / 6f ) * Mathf.Sqrt( Props[ R.Experience ].Delta ) ), 0, 100 );
        int levelDelta = (int)( newLevel - Props[ R.Level ].Value );
        if( levelDelta > 0 )
        {
            Props[ R.Experience ].MaxValue = GetMaxXP( newLevel );
            Props[ R.UpgradePoint ].Value += levelDelta;
            Props[ R.Level ].Value = newLevel;
        }
        Props[ R.Experience ].Value = Props[ R.Experience ].Delta - GetXPCumulative( newLevel );
        
        /*
        if( Props[ R.Experience ].Value >= Props[ R.Experience ].MaxValue )
        {
            Props[ R.Experience ].Value -= _levelUpModel.Experience;
            Props[ R.UpgradePoint ].Value += _levelUpModel.UpgradePoints;
            Props[ R.Level ].Value++;
            _levelUpModel = _levelUpConfig[ (int)Props[ R.Level ].Value ];
            Props[ R.Experience ].MaxValue = _levelUpModel.Experience;

            //Props[ R.Health ].MaxValue = _levelUpModel.Effects[ R.Health ];
            Props[ R.Health ].Value = Props[ R.Health ].MaxValue;
        }
        */
    }

    private int GetMaxXP( int level )
    {
        return GetXPCumulative( level ) - GetXPCumulative( level - 1 );
    }

    private int GetXPCumulative( int level )
    {
        return Mathf.CeilToInt( 0.432f * Mathf.Pow( level, 2 ) );
    }

    public void Dispose()
    {
        _slots = null;
        _slotsConfig = null;
        _levelUpConfig = null;
        _levelUpModel = null;
        disposables.Clear();
        disposables = null;
    }
}
