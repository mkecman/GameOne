﻿using UnityEngine;
using System.Collections;
using System;

public class EChart : MonoBehaviour
{
    public string PropertyName;
    public int DataPoints = 100;
    private SimplestPlot chart;
    private float Counter = 0;

    private float[] XValues;
    private float[] Y1Values;

    void Start()
    {
        Debug.Log( "EChart Start" );
        chart = GetComponent<SimplestPlot>();

        XValues = new float[ DataPoints ];
        Y1Values = new float[ DataPoints ];

        chart.SetResolution( new Vector2( 300, 300 ) );
        chart.BackGroundColor = new Color( 0.1f, 0.1f, 0.1f, 0.4f );
        chart.TextColor = Color.yellow;
        chart.SeriesPlotY.Add( new SimplestPlot.SeriesClass() );
        chart.SeriesPlotY[ 0 ].MyColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        Counter++;
        PrepareArrays();
        chart.SeriesPlotY[ 0 ].YValues = Y1Values;
        chart.SeriesPlotX = XValues;
        
        chart.UpdatePlot();
    }

    private void PrepareArrays()
    {
        GameObject go = GameObject.Find( PropertyName );
        if( go != null )
        {
            ENode eObject = go.GetComponent<ENode>();
            int count = eObject._PastValues.Count;
            int nextIndex = ( count - DataPoints );
            XValues[ 0 ] = nextIndex;
            Y1Values[ 0 ] = -1;

            nextIndex = ( count - DataPoints ) + (DataPoints - 1);
            XValues[ DataPoints - 1 ] = nextIndex;
            Y1Values[ DataPoints-1 ] = 1;
            for( int i = 1; i < DataPoints-1; i++ )
            {
                nextIndex = ( count - DataPoints ) + i;
                XValues[ i ] = nextIndex;
                if( nextIndex < count && nextIndex >= 0 )
                    Y1Values[ i ] = Convert.ToSingle( eObject._PastValues[ nextIndex ] );
                else
                    Y1Values[ i ] = 0;
            }
        }
    }
}
