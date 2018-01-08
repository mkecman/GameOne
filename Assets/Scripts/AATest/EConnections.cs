using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class EConnections : MonoBehaviour
{
    public EContainer _eContainer;
    public GameObject connectionPrefab;

    void Start()
    {
        Debug.Log( "EConnections.Start" );
        GameMessage.Listen<EMessage>( OnEMessage );
        //Refresh();
    }

    private void OnEMessage( EMessage value )
    {
        Refresh();
    }

    void OnDestroy()
    {
        Debug.Log( "EConnections.OnDestroy" );
        //RemoveConnections();
    }

    private void RemoveConnections()
    {
        Debug.Log( "EConnections.RemoveConnections: " + gameObject.transform.childCount );
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
        Debug.Log( "EConnections.AddConnections: " );
        foreach( Transform child in _eContainer.transform )
        {
            ObjectComponent oc = child.gameObject.GetComponent<ObjectComponent>();
            for( int i = 0; i < oc._InputConnections.Count; i++ )
            {
                AddConnection( oc._InputConnections[ i ] );
            }
        }
    }

    public void AddConnection( EConnection connection )
    {
        GameObject connectionInstance = Instantiate( connectionPrefab, gameObject.transform );
        ConnectionComponent connectionComponent = connectionInstance.GetComponent<ConnectionComponent>();
        connectionComponent.ConnectionInstance = connection;
    }

    public void Refresh()
    {
        RemoveConnections();
        AddConnections();
    }
}
