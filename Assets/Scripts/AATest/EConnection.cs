using System;
using UniRx;

[Serializable]
public class EConnection
{
    public int Index = 0;

    public EObject Source;
    public DoubleReactiveProperty SourceDelta = new DoubleReactiveProperty( 0 );
    public StringReactiveProperty SourceName = new StringReactiveProperty();
    public StringReactiveProperty SourceFormula = new StringReactiveProperty( "1" );

    public EObject Target;
    public DoubleReactiveProperty TargetDelta = new DoubleReactiveProperty( 0 );
    public StringReactiveProperty TargetName = new StringReactiveProperty();
    public StringReactiveProperty TargetFormula = new StringReactiveProperty( "1" );
}
