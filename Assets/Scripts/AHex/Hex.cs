using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Hex : MonoBehaviour
{
    public HexModel Model;

    public void SetModel( HexModel model )
    {
        this.Model = model;
        SetAltitudeColor();
    }

    private void SetAltitudeColor()
    {
        byte colorIncrement = (byte)Math.Round( 255 * Model.Altitude, 0);
        GetComponent<MeshRenderer>().material.color = new Color32( colorIncrement, colorIncrement, colorIncrement, 255 );
    }

    private void OnMouseDown()
    {
        Debug.Log( "Pushing Down" );
        Push( -.5f );
    }
    private void OnMouseUp()
    {
        Debug.Log( "Pushing Up" );
        Push( .5f );
    }
    
    public void Push( float value )
    {
        Mesh tileMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] verts = tileMesh.vertices;
        for( int q = 0; q < verts.Length; ++q )
        {
           if( verts[ q ].z < 0 )
            verts[ q ].z += value;
        }
        tileMesh.vertices = verts;
        tileMesh.RecalculateNormals();
        //altitude += value;
    }
}
