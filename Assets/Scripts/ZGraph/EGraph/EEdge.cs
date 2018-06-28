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
    private double _newSourceDelta;
    private double _newTargetValue;
    //private double _newTargetDelta;

    public void Process()
    {
        _newSourceValue = GetFormulaValue( SourceFormula.Value );
        _newSourceDelta = -( Source.Value - _newSourceValue );
        _newTargetValue = GetFormulaValue( TargetFormula.Value );

        //if in range
        if( _newSourceValue >= Source.MinValue && _newSourceValue <= Source.MaxValue
            && _newTargetValue >= Target.MinValue && _newTargetValue <= Target.MaxValue )
        {
            SourceDelta.Value = _newSourceDelta;
            Source.Value = _newSourceValue;

            TargetDelta.Value = -( Target.Value - _newTargetValue );
            Target.Value = _newTargetValue;
        }
        ////////////////////////
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
            formula = formula.Replace( "_SourceDelta_", _newSourceDelta.ToString() );
            formula = formula.Replace( "_Source_", Source.Value.ToString() );
            formula = formula.Replace( "_Target_", Target.Value.ToString() );
            //formula = formula.Replace( "_TargetDelta_", _newTargetDelta.ToString() );

            var distinct = Regex.Matches( formula, "[^0123456789.()*/%+-]+" ).OfType<Match>().Select( _ => _.Value ).Distinct();
            GameObject go;
            foreach( string item in distinct )
            {
                go = GameObject.Find( item );
                if( go == null )
                    throw new Exception( "Can't find EObject: " + item );

                //replace occurence of item (whole word search) with its value
                //formula = Regex.Replace( formula, @"\b" + item + "\b", go.GetComponent<ENode>().Value.ToString() );
                formula = SafeReplace( formula, item, go.GetComponent<ENode>().Value.ToString(), true );
            }
            formulaValue = Convert.ToDouble( new System.Data.DataTable().Compute( formula, null ) );
            //string formula = "IIF(1<0,'yes','no')"; //example of IF function 
            //https://msdn.microsoft.com/en-us/library/system.data.datacolumn.expression(v=vs.110).aspx
        }
        catch( Exception e )
        {
            Debug.LogWarning( e.Message );
        }

        return formulaValue;
    }

    public string SafeReplace( string input, string find, string replace, bool matchWholeWord )
    {
        string textToFind = matchWholeWord ? string.Format( @"\b{0}\b", find ) : find;
        return Regex.Replace( input, textToFind, replace );
    }

    public void SetSource( ENode node )
    {
        SourceFormula.Value = node.gameObject.name + "-1";
        SourceName.Value = node.gameObject.name;
        Source = node;
    }

    public void SetTarget( ENode node )
    {
        TargetFormula.Value = node.gameObject.name + "+1";
        TargetName.Value = node.gameObject.name;
        Target = node;
    }
}
