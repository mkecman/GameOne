using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;

[Serializable]
public class EEdge
{
    public int Index = 0;

    public ENode Source;
    public DoubleReactiveProperty SourceDelta = new DoubleReactiveProperty( 0 );
    public StringReactiveProperty SourceName = new StringReactiveProperty();
    public StringReactiveProperty SourceFormula = new StringReactiveProperty( "1" );

    public ENode Target;
    public DoubleReactiveProperty TargetDelta = new DoubleReactiveProperty( 0 );
    public StringReactiveProperty TargetName = new StringReactiveProperty();
    public StringReactiveProperty TargetFormula = new StringReactiveProperty( "1" );

    private double _newSourceValue;
    private double _newTargetValue;

    public void Process()
    {
        _newSourceValue = ClampToRange( GetFormulaValue( SourceFormula.Value ), Source );
        SourceDelta.Value = -( Source.Value - _newSourceValue );
        Source.Value = _newSourceValue;

        _newTargetValue = ClampToRange( GetFormulaValue( TargetFormula.Value ), Target );
        TargetDelta.Value = -( Target.Value - _newTargetValue );
        Target.Value = _newTargetValue;
    }

    private double ClampToRange( double value, ENode range )
    {
        if( value < range.MinValue )
            return range.MinValue;

        if( value > range.MaxValue )
            return range.MaxValue;

        return value;
    }

    public double GetFormulaValue( string formula )
    {
        double formulaValue = 0;

        try
        {
            formula = formula.Replace( "SourceDelta", SourceDelta.Value.ToString() );
            formula = formula.Replace( "TargetDelta", TargetDelta.Value.ToString() );

            var distinct = Regex.Matches( formula, "[^0123456789.()*/%+-]+" ).OfType<Match>().Select( _ => _.Value ).Distinct();
            GameObject go;
            foreach( string item in distinct )
            {
                go = GameObject.Find( item );
                if( go == null )
                    throw new Exception( "Can't find EObject: " + item );

                formula = formula.Replace( item, go.GetComponent<ENode>().Value.ToString() );
            }
            formulaValue = Convert.ToDouble( new DataTable().Compute( formula, null ) );
        }
        catch( Exception e )
        {
            Debug.LogWarning( e.Message );
        }

        return formulaValue;
    }
}
