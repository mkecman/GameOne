using UnityEngine;
using System.Collections;
using UniRx;
using System;

[Serializable]
public class FractalModel
{
    [RangeReactiveProperty( 1, 10 )]
    public IntReactiveProperty Octaves = new IntReactiveProperty( 10 );
    [RangeReactiveProperty( 0.1f, 2 )]
    public DoubleReactiveProperty Frequency = new DoubleReactiveProperty( 1.0 );
    public IntReactiveProperty Seed = new IntReactiveProperty( 0 );
    [RangeReactiveProperty( 0, 4 )]
    public IntReactiveProperty FractalType = new IntReactiveProperty( 2 );
    [RangeReactiveProperty( 0, 4 )]
    public IntReactiveProperty BasisType = new IntReactiveProperty( 3 );
    [RangeReactiveProperty( 0, 3 )]
    public IntReactiveProperty InterpolationType = new IntReactiveProperty( 3 );
}


