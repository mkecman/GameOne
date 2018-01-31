using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Hex : MonoBehaviour
{
    public HexModel Model;
    public GameObject Gas;
    public GameObject Liquid;
    public GameObject Solid;

    public void SetModel( HexModel model )
    {
        this.Model = model;

        SetHeight( Solid, Model.Altitude );
        SetAltitudeColor();
        //SetLiquidAltitude();
        //SetClouds();
    }

    private void SetClouds()
    {
        Gas.transform.position = new Vector3( Gas.transform.position.x, 0.9f, Gas.transform.position.z );
        Gas.GetComponent<MeshRenderer>().material.color = new Color32( 255, 255, 255, (byte)RandomUtil.FromRangeInt(0,130) );
    }

    private void SetLiquidAltitude()
    {
        if( Model.SeaLevel < Model.Altitude )
        {
            Liquid.SetActive( false );
            return;
        }
        Liquid.transform.position = new Vector3( Liquid.transform.position.x, Model.Altitude+0.05f, Liquid.transform.position.z );
        SetHeight( Liquid, Model.SeaLevel - Model.Altitude );
    }

    private void SetAltitudeColor()
    {
        Solid.GetComponent<MeshRenderer>().material.color = Model.Color;
        //byte colorIncrement = (byte)(255 * Model.Altitude);
        //Solid.GetComponent<MeshRenderer>().material.color = new Color32( (byte)(colorIncrement+50), colorIncrement, colorIncrement, 255 );
        //Solid.GetComponent<MeshRenderer>().material.color = Color.Lerp( Color.red, Color.blue, Model.Altitude );
    }
    
    private void SetHeight( GameObject target, float height )
    {
        Mesh mesh = target.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        for( int q = 0; q < verts.Length; ++q )
        {
            if( verts[ q ].y > 0 )
                verts[ q ].y = height;
        }
        mesh.vertices = verts;
        mesh.RecalculateNormals();
    }
}
