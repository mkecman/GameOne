using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[ExecuteInEditMode]
public class ESpliter : MonoBehaviour
{
    public ENode Node;
    public string Formula;

    public List<ESpliterSlider> Values = new List<ESpliterSlider>();

    private bool _ignoreSliderChanges = false;
    
    void OnEnable()
    {
        if( Node == null )
            Node = GetComponent<ENode>();

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
                ess.Value.Subscribe( _ => OnSliderValueChanged( ess ) ).AddTo( this );
            }
            _ignoreSliderChanges = false;
        }
    }

    private void OnSliderValueChanged( ESpliterSlider slider )
    {
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
            bool _found;
            while( steps > 0 )
            {
                _found = false;
                for( int j = 0; j < Values.Count; j++ )
                {
                    if( ( ( sign == -1 && Values[ j ].Value.Value > 0 ) || ( sign == 1 && Values[ j ].Value.Value < 1 ) ) && Node._TargetConnections[ j ] != slider.Edge )
                    {
                        Values[ j ].Value.Value = Math.Round( Values[ j ].Value.Value + ( 0.01 * sign ), 2 );
                        Values[ j ].oldValue = Values[ j ].Value.Value;
                        Values[ j ].UpdateFormula( Formula );
                        _found = true;
                        steps--;
                        if( steps == 0 )
                            break;
                    }
                }
                if( _found == false )
                    break;
            }
            
            _ignoreSliderChanges = false;
        }
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
        Edge.TargetFormula.Value = formula.Replace( "_Split_", Value.Value.ToString( "F2" ) );
    }

}