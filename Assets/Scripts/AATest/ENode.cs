using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[Serializable, ExecuteInEditMode]
public class ENode : MonoBehaviour
{
    public Text NameText;
    public Text ValueText;
    public Text DeltaText;

    private EEdgesRedrawAll message = new EEdgesRedrawAll();
    private Color32 greenColor = new Color32( 17, 139, 48, 255 );
    private Color32 redColor = new Color32( 139, 17, 40, 255 );
    

    private void Start()
    {
        Debug.Log( "ObjectComponent.Start" );

        _PastValues.Add( 0 );
        _Name.Subscribe( _ => { NameText.text = _; gameObject.name = _; } );
        _Value.Subscribe( _ => ValueText.text = _.ToString( "F2" ) );
        _Delta.Subscribe( _ => UpdateDeltaText() );

        if( AutoTrigger )
            GameMessage.Listen<ClockTickMessage>( Process );
    }

    public void Process( ClockTickMessage value = null )
    {
        for( int i = 0; i < _TargetConnections.Count; i++ )
        {
            _TargetConnections[ i ].Process();
        }

        Delta = -( _PastValues[ _PastValues.Count - 1 ] - Value );
        _PastValues.Add( Value );
    }
    
    public void AddConnection( int index )
    {
        EEdge connection = _TargetConnections[ index ];
        connection.Index = index;
        connection.SourceFormula.Value = gameObject.name + "-1";
        connection.SourceName.Value = gameObject.name;
        connection.Source = this;
        GameMessage.Send<EEdgesRedrawAll>( message );
    }
    
    public void RemoveConnection()
    {
        GameMessage.Send<EEdgesRedrawAll>( message );
    }

    public void RemoveConnectionTarget( EEdge connection  )
    {
        if( connection.Target != null && connection.Target._SourceConnections.Contains( connection ) )
            connection.Target._SourceConnections.Remove( connection );
    }

    public void SetConnectionTarget( EEdge connection )
    {
        connection.TargetName.Value = connection.Target.name;
        connection.TargetFormula.Value = connection.Target.name + "+1";
        connection.Target._SourceConnections.Add( connection );
        GameMessage.Send<EEdgesRedrawAll>( message );
    }
    
    private void UpdateDeltaText()
    {
        string sign = "+";
        Color color = greenColor;
        if( Delta < 0 )
        {
            sign = "";
            color = redColor;
        }
        DeltaText.text = sign + Delta.ToString( "F2" );
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
    internal BoolReactiveProperty _AutoTrigger = new BoolReactiveProperty( true );
    public bool AutoTrigger
    {
        get { return _AutoTrigger.Value; }
        set { _AutoTrigger.Value = value; }
    }


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
    internal DoubleReactiveProperty _MinValue = new DoubleReactiveProperty(0);
    public Double MinValue
    {
        get { return _MinValue.Value; }
        set { _MinValue.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _MaxValue = new DoubleReactiveProperty(1000);
    public Double MaxValue
    {
        get { return _MaxValue.Value; }
        set { _MaxValue.Value = value; }
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
    
    [SerializeField]
    public List<EEdge> _SourceConnections = new List<EEdge>();

    [SerializeField]
    public List<EEdge> _TargetConnections = new List<EEdge>();

}