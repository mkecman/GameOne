using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEditor;

[ExecuteInEditMode]
public class Connections : GameView
{
    public GameObject connectionPrefab;
    public Transform connectionContainer;

    private ObjectComponent Data;
    private List<ConnectionComponent> connections = new List<ConnectionComponent>();

    public void AddConnection( EConnection connection )
    {
        GameObject connectionInstance = Instantiate( connectionPrefab, connectionContainer );
        ConnectionComponent connectionComponent = connectionInstance.GetComponent<ConnectionComponent>();
        connectionComponent.ConnectionInstance = connection;
    }

    public void RemoveConnection( int index )
    {
        DestroyImmediate( connectionContainer.GetChild( index ).gameObject );
    }

    public void UpdateConnection( int index, EConnection connection )
    {
        ConnectionComponent comp = connectionContainer.GetChild( index ).gameObject.GetComponent<ConnectionComponent>();
        comp.ConnectionInstance = connection;
        comp.UpdateConnectionListeners();
        //Debug.Log( "Updated Conncetion: " + index );
    }
    
    private void Start()
    {
        //Observable.Interval( TimeSpan.FromSeconds( 2 ) ).Timestamp().Subscribe( x => SingleUpdate() );
        if( Data == null )
        {
            Data = gameObject.GetComponent<ObjectComponent>();
            //Data._InputConnections.ObserveEveryValueChanged( _ => _.Count ).Subscribe( _ => { ReDraw(); } ).AddTo( this );
        }
        Debug.Log( "Connections.Start--------------------------" );
        //EditorApplication.playModeStateChanged += YourPlaymodeStateChangedHandler;
    }

    private void Update()
    {
        Debug.Log( "Connections.Update " );
    }

        /*
        private void YourPlaymodeStateChangedHandler( PlayModeStateChange state )
        {
            Debug.Log( "Play mode: " + EditorApplication.isPlaying );
            ReDraw();
        }
        */

        void OnEnable()
    {
        
        //ReDraw();
        //Debug.Log( "OnEnable: " );
        //var observable = Observable.Timer( TimeSpan.FromSeconds( 1 ) ).Timestamp().Subscribe( _ => ReDraw() );
    }
    
    public void ReDraw()
    {
        //Debug.Log( "ReDraw: " + Data._InputConnections.Count );

        foreach( Transform child in connectionContainer )
        {
            ConnectionComponent connectionComponent = child.gameObject.GetComponent<ConnectionComponent>();
            connectionComponent.UpdateConnectionListeners();
        }
        
        /*
        RemoveConnections();
        for( int i = 0; i < Data._InputConnections.Count; i++ )
        {
            GameObject connectionInstance = Instantiate( connectionPrefab, connectionContainer );
            ConnectionComponent connectionComponent = connectionInstance.GetComponent<ConnectionComponent>();
            connectionComponent.UpdateConnectionModel( Data._InputConnections[ i ] );
            connections.Add( connectionComponent );
        }*/
    }

    private void OnDisable()
    {
        //RemoveConnections();
        //Debug.Log( "OnDisable => RemoveConnections: " );
    }

    private void RemoveConnections()
    {
        while( connectionContainer.childCount != 0 )
        {
            DestroyImmediate( connectionContainer.GetChild( 0 ).gameObject );
        }
        
        connections = new List<ConnectionComponent>();
    }
}
