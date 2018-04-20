using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System;

public class UnitModel
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

    public Dictionary<R,Resource> Props = new Dictionary<R,Resource>();

    internal Vector3 Position = new Vector3();

    public Dictionary<int, SkillData> Skills = new Dictionary<int, SkillData>();
    public List<int> PassiveSkills = new List<int>();
    public List<int> ActiveSkills = new List<int>();

    public void UpdateAttack()
    {
        Props[ R.Attack ].Value = Props[ R.Body ].Value * ( ( Props[ R.Speed ].Value / 100 ) + 1 );
    }
}
