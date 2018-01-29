using UnityEngine;
using System.Collections;
using System;

public class EOscilatorSource : MonoBehaviour
{
    public double Amplitude = 0.5;
    public double FullCycle = 60;
    public double VerticalOfset = 0.5;
    public double Phase = 0;

    private ENode _node;
    private double _2pi = 2 * Math.PI;
    private double _time = 0;

    void Start()
    {
        _node = gameObject.GetComponent<ENode>();
        GameMessage.Listen<ClockTickMessage>( OnClockTick );
    }

    private void OnClockTick( ClockTickMessage value )
    {
        _node.Value = Amplitude * Math.Sin( ( _2pi * _time + Phase ) / FullCycle ) + VerticalOfset;
        _time++;
    }
    
}
