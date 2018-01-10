using UnityEngine;
using System.Collections;
using System;

public class EChart : MonoBehaviour
{
    public string PropertyName;
    public int DataPoints = 100;
    private SimplestPlot chart;
    private float Counter = 0;

    private System.Random MyRandom;
    private float[] XValues;
    private float[] Y1Values;

    private Vector2 Resolution;

    void Start()
    {
        chart = GetComponent<SimplestPlot>();

        MyRandom = new System.Random();
        XValues = new float[ DataPoints ];
        Y1Values = new float[ DataPoints ];

        chart.SetResolution( new Vector2( 300, 300 ) );
        chart.BackGroundColor = new Color( 0.1f, 0.1f, 0.1f, 0.4f );
        chart.TextColor = Color.yellow;
        //for( int Cnt = 0; Cnt < 2; Cnt++ )
        //{
        chart.SeriesPlotY.Add( new SimplestPlot.SeriesClass() );
        chart.SeriesPlotY[ 0 ].MyColor = Color.white;
        //}

        Resolution = chart.GetResolution();
        
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
            EObject eObject = go.GetComponent<EObject>();
            int count = eObject._PastValues.Count;
            for( int i = 0; i < DataPoints; i++ )
            {
                XValues[ i ] = i;
                int nextIndex = ( count - DataPoints ) + i;
                if( nextIndex < count && nextIndex >= 0 )
                    Y1Values[ i ] = Convert.ToSingle( eObject._PastValues[ nextIndex ] );
                else
                    Y1Values[ i ] = 0;
            }
        }
    }
}
