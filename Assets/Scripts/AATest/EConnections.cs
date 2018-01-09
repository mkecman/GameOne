using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

[ExecuteInEditMode]
public class EConnections : MonoBehaviour
{
    public GameObject _eContainer;
    public GameObject connectionPrefab;

    void Start()
    {
        Debug.Log( "EConnections.Start" );
        GameMessage.Listen<EMessageRedrawAllConnections>( OnEMessage );
        Redraw();
    }
    
    public void Refresh()
    {
        foreach( Transform child in gameObject.transform )
        {
            EConnectionComponent cc = child.gameObject.GetComponent<EConnectionComponent>();
            cc.UpdateConnectionListeners();
        }
    }

    public void Redraw()
    {
        RemoveConnections();
        AddConnections();
    }

    private void OnEMessage( EMessageRedrawAllConnections value )
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
        foreach( Transform child in _eContainer.transform )
        {
            EObject oc = child.gameObject.GetComponent<EObject>();
            for( int i = 0; i < oc._TargetConnections.Count; i++ )
            {
                AddConnection( oc._TargetConnections[ i ] );
            }
        }
    }

    private void AddConnection( EConnection connection )
    {
        GameObject connectionInstance = Instantiate( connectionPrefab, gameObject.transform );
        EConnectionComponent connectionComponent = connectionInstance.GetComponent<EConnectionComponent>();
        connectionComponent.Connection = connection;
    }
}
