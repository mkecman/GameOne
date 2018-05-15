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

    public string Name;

    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    //public Dictionary<int, BuildingState> Abilities = new Dictionary<int, BuildingState>();
    //public Dictionary<R, FloatReactiveProperty> AbilitiesDelta = new Dictionary<R, FloatReactiveProperty>();

    public Dictionary<R, BellCurve> Resistance = new Dictionary<R,BellCurve>();

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

    public void Setup()
    {
        _slotsConfig = GameConfig.Get<BodySlotsConfig>();
        _levelUpConfig = GameConfig.Get<LevelUpConfig>();
        _levelUpModel = _levelUpConfig[ (int)Props[ R.Level ].Value ];

        Props[ R.Body ]._Value.Subscribe( _ => UpdateBody() ).AddTo( disposables );
        Props[ R.Speed ]._Value.Subscribe( _ => UpdateAttack() ).AddTo( disposables );
        Props[ R.Experience ]._Value.Subscribe( _ => CheckLevelUp() ).AddTo( disposables );
    }

    private void CheckLevelUp()
    {
        if( Props[ R.Experience ].Value >= _levelUpModel.Experience )
        {
            Props[ R.Experience ].Value -= _levelUpModel.Experience;
            Props[ R.UpgradePoint ].Value += _levelUpModel.UpgradePoints;
            Props[ R.Level ].Value++;
            _levelUpModel = _levelUpConfig[ (int)Props[ R.Level ].Value ];

            Props[ R.Health ].MaxValue = _levelUpModel.Effects[ R.Health ];
            Props[ R.Health ].Value = Props[ R.Health ].MaxValue;
        }
    }

    public void Dispose()
    {
        _slots = null;
        _slotsConfig = null;
        disposables.Clear();
        disposables = null;
    }

    private void UpdateBody()
    {
        _slots = _slotsConfig[ (int)( Props[ R.Body ].Value / 8.34f ) ];
        for( int i = 0; i < _slots.Count; i++ )
        {
            if( BodySlots.ContainsKey( i ) )
            {
                BodySlots[ i ].IsEnabled = _slots[ i ] == 1 ? true : false;
                if( !BodySlots[ i ].IsEnabled )
                    GameModel.Get<UnitEquipCommand>().ExecuteUnequip( i );
            }
            else
                BodySlots.Add( i, new BodySlotModel( i, _slots[ i ] == 1 ? true : false ) );
        }



        UpdateAttack();
    }

    private void UpdateAttack()
    {
        Props[ R.Attack ].Value = Props[ R.Body ].Value * ( ( Props[ R.Speed ].Value / 100 ) + 1 );
    }
}
