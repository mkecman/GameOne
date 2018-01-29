using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

[ExecuteInEditMode]
public class EEdges : MonoBehaviour
{
    public GameObject Container;
    public GameObject ConnectionPrefab;

    void Start()
    {
        Debug.Log( "EEdges.Start" );
        GameMessage.Listen<EEdgesRedrawAll>( OnEMessage );
        Redraw();
    }
    
    public void Refresh()
    {
        foreach( Transform child in gameObject.transform )
        {
            EEdgeComponent cc = child.gameObject.GetComponent<EEdgeComponent>();
            cc.UpdateConnectionListeners();
        }
    }

    public void Redraw()
    {
        RemoveConnections();
        AddConnections();
    }

    private void OnEMessage( EEdgesRedrawAll value )
    {
        Redraw();
    }

    private void RemoveConnections()
    {
        GameObject go;
        while( gameObject.transform.childCount != 0 )
        {
            go = gameObject.transform.GetChild( 0 ).gameObject;
            go.transform.SetParent( null );
            DestroyImmediate( go );
        }
    }

    private void AddConnections()
    {
        foreach( Transform child in Container.transform )
        {
            ENode oc = child.gameObject.GetComponent<ENode>();
            for( int i = 0; i < oc._SourceConnections.Count; i++ )
            {
                AddConnection( oc._SourceConnections[ i ] );
            }
            for( int i = 0; i < oc._TargetConnections.Count; i++ )
            {
                AddConnection( oc._TargetConnections[ i ] );
            }
        }
    }

    private void AddConnection( EEdge connection )
    {
        GameObject connectionInstance = Instantiate( ConnectionPrefab, gameObject.transform );
        EEdgeComponent connectionComponent = connectionInstance.GetComponent<EEdgeComponent>();
        connectionComponent.Connection = connection;
    }
}
