using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[ExecuteInEditMode]
public class ESpliter : MonoBehaviour
{
    public ENode Node;
    public string Formula;

    public List<ESpliterSlider> Values;

    private bool _ignoreSliderChanges = false;


    void OnEnable()
    {
        if( Values.Count == 0 )
        {
            Values = new List<ESpliterSlider>();
            double count = Node._TargetConnections.Count;
            double startValue = 1;
            double splitValue = Math.Round( startValue / count, 2, MidpointRounding.AwayFromZero );
            double remainder = ( (100*startValue) % count ) / 100;

            _ignoreSliderChanges = true;
            for( int i = 0; i < count; i++ )
            {
                ESpliterSlider slider = new ESpliterSlider( Node._TargetConnections[ i ] );

                if( i == 0 )
                    slider.Value.Value = splitValue + remainder;
                else
                    slider.Value.Value = splitValue;

                slider.oldValue = slider.Value.Value;
                startValue -= splitValue;
                slider.Value.Subscribe( _ => OnSliderValueChanged( slider ) ).AddTo( this );
                Values.Add( slider );
            }
            _ignoreSliderChanges = false;
        }
        else
        {
            _ignoreSliderChanges = true;
            for( int i = 0; i < Values.Count; i++ )
            {
                ESpliterSlider ess = Values[ i ];
                ess.Edge = Node._TargetConnections[ i ];
                Debug.Log( ess.Edge.TargetFormula );
                ess.Value.Subscribe( _ => OnSliderValueChanged( ess ) ).AddTo( this );
            }
            _ignoreSliderChanges = false;
        }
    }

    private void OnSliderValueChanged( ESpliterSlider slider )
    {
        Debug.Log( slider.Edge.Index + "==" + slider.Value + "; TFormula: " + slider.Edge.TargetFormula );
        if( !_ignoreSliderChanges )
        {
            _ignoreSliderChanges = true;

            slider.Value.Value = Math.Round( slider.Value.Value, 2, MidpointRounding.AwayFromZero );
            if( slider.Value.Value == slider.oldValue )
            {
                _ignoreSliderChanges = false;
                return;
            }

            slider.UpdateFormula( Formula );

            double sign = 1;
            double delta = Math.Round( slider.oldValue - slider.Value.Value, 2 );
            if( delta < 0 )
                sign = -1;
            slider.oldValue = slider.Value.Value;

            int steps = (int)Math.Abs( delta / 0.01 );
            while( steps > 0 )
                for( int j = 0; j < Values.Count; j++ )
                {
                    if( ( ( sign == -1 && Values[ j ].Value.Value > 0 ) || ( sign == 1 && Values[ j ].Value.Value < 1 ) ) && Node._TargetConnections[ j ] != slider.Edge )
                    {
                        Values[ j ].Value.Value = Math.Round( Values[ j ].Value.Value + ( 0.01 * sign ), 2 );
                        Values[ j ].oldValue = Values[ j ].Value.Value;
                        Values[ j ].UpdateFormula( Formula );
                        steps--;
                        if( steps == 0 )
                            break;
                    }
                }
            
            _ignoreSliderChanges = false;
        }

        double total = 0;
        for( int i = 0; i < Values.Count; i++ )
        {
            total += Values[ i ].Value.Value;
        }
        Debug.Log( "TOTAL: " + total );
    }
}

[Serializable]
public class ESpliterSlider
{
    [RangeReactiveProperty( 0, 1 )]
    public DoubleReactiveProperty Value = new DoubleReactiveProperty();

    public double oldValue;

    public EEdge Edge;

    public ESpliterSlider( EEdge edge )
    {
        Edge = edge;
    }

    internal void UpdateFormula( string formula )
    {
        Edge.TargetFormula.Value = formula.Replace( "Split", Value.Value.ToString( "F2" ) );
    }

}