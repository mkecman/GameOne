using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EObject : MonoBehaviour
{
    public Text NameText;
    public Text ValueText;
    public Text DeltaText;

    private EMessage message = new EMessage();
    private Color32 greenColor = new Color( 17, 139, 48, 255 );
    private Color32 redColor = new Color( 139, 17, 40, 255 );

    private void Start()
    {
        Debug.Log( "ObjectComponent.Start" );
        Observable.Interval( TimeSpan.FromSeconds( 1 ) ).Subscribe( x => SingleUpdate() );
    }

    public void AddConnection( int index )
    {
        EConnection connection = _InputConnections[ index ];
        connection.Index = index;
        connection.Formula.Value = "1";
        connection.SourceName.Value = gameObject.name;
        GameMessage.Send<EMessage>( message );
    }

    public void RemoveConnection( int index )
    {
        GameMessage.Send<EMessage>( message );
    }

    public void SetConnectionTarget( int index )
    {
        _InputConnections[ index ].TargetName.Value = _InputConnections[ index ].Target.name;
        GameMessage.Send<EMessage>( message );
    }

    private void SingleUpdate()
    {
        Delta = 0;
        for( int i = 0; i < _InputConnections.Count; i++ )
        {
            Delta += _InputConnections[ i ].Delta.Value;
        }
        Value += Delta;
        PastValues.Add( Value );

        ValueText.text = Value.ToString();
        string sign = "+";
        Color color = greenColor;
        if( Delta < 0 )
        {
            sign = "-";
            color = redColor;
        }
        DeltaText.text = sign + Delta;
        DeltaText.color = color;
    }

    /**
     * 
     * PROPERTIES
     * 
     * 
     * */

    public Image Logo;

    [SerializeField]
    internal StringReactiveProperty _Name = new StringReactiveProperty();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    [SerializeField]
    internal StringReactiveProperty _Description = new StringReactiveProperty();
    public string Description
    {
        get { return _Description.Value; }
        set { _Description.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _StartValue = new DoubleReactiveProperty();
    public Double StartValue
    {
        get { return _StartValue.Value; }
        set { _StartValue.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _StopValue = new DoubleReactiveProperty();
    public Double StopValue
    {
        get { return _StopValue.Value; }
        set { _StopValue.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _Value = new DoubleReactiveProperty();
    public Double Value
    {
        get { return _Value.Value; }
        set { _Value.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _Delta = new DoubleReactiveProperty();
    public Double Delta
    {
        get { return _Delta.Value; }
        set { _Delta.Value = value; }
    }

    [SerializeField]
    internal ReactiveCollection<double> _PastValues = new ReactiveCollection<double>();
    public List<double> PastValues
    {
        get { return _PastValues.ToList<double>(); }
        set { _PastValues = new ReactiveCollection<double>( value ); }
    }

    [SerializeField]
    public List<EConnection> _InputConnections = new List<EConnection>();

    [SerializeField]
    internal List<EConnection> _OutputConnections = new List<EConnection>();

}