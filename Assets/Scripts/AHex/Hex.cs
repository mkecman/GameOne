using UnityEngine;
using System.Collections;
using UniRx;

public class Hex : MonoBehaviour
{
    public int X, Y;

    public Mesh tileMesh;

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
