using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Hex : MonoBehaviour
{
    public HexModel Model;
    public TextMesh SymbolText;
    public GameObject Gas;
    public GameObject Liquid;
    public GameObject Solid;
    public GameObject UnitPrefab;

    public void SetModel( HexModel model )
    {
        this.Model = model;

        SetHeight( Solid, Model.Altitude );
        SetColor();
        SetSymbol();
        //SetLiquidAltitude();
        //SetClouds();
    }

    private void OnMouseEnter()
    {
        Solid.GetComponent<MeshRenderer>().material.color = Color.magenta;
    }

    private void OnMouseExit()
    {
        SetColor();
    }

    private void OnMouseDown()
    {
        GameObject unit = (GameObject)Instantiate(
                    UnitPrefab,
                    new Vector3( transform.position.x, Model.Altitude, transform.position.z ),
                    Quaternion.identity );
        unit.transform.SetParent( this.transform );
    }

    private void SetSymbol()
    {
        SymbolText.text = Model.Element.Symbol;
        SymbolText.gameObject.transform.position = new Vector3( SymbolText.gameObject.transform.position.x, Model.Altitude + .01f, SymbolText.gameObject.transform.position.z );
    }

    private void SetClouds()
    {
        Gas.transform.position = new Vector3( Gas.transform.position.x, 0.9f, Gas.transform.position.z );
        Gas.GetComponent<MeshRenderer>().material.color = new Color32( 255, 255, 255, (byte)RandomUtil.FromRangeInt(0,255) );
    }
    
    private void SetColor()
    {
        Solid.GetComponent<MeshRenderer>().material.color = Model.Color;
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
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
