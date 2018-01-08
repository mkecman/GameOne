using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class ConnectionComponent : GameView
{
    public EConnection ConnectionInstance;
    private RectTransform _rectTransform;
    public GameObject SourceTransform;
    public GameObject TargetTransform;


    private void OnDisable()
    {
        Debug.Log( "ConnectionComponent.OnDisable" );
    }
    private void OnEnable()
    {
        Debug.Log( "ConnectionComponent.OnEnable" );
    }
    void Start()
    {
        Debug.Log( "ConnectionComponent.Start" );
        UpdateConnectionListeners();
    }
    
    public void UpdateConnectionListeners()
    {
        ConnectionInstance.Formula.Subscribe( _ => OnDataChanged() ).AddTo( disposables );
        ConnectionInstance.Delta.Subscribe( _ => OnDeltaUpdate() ).AddTo( disposables );
        ConnectionInstance.TargetName.Subscribe( _ => OnTargetUpdated() ).AddTo( disposables );
    }

    private void OnTargetUpdated()
    {
        SourceTransform = GameObject.Find( ConnectionInstance.SourceName.Value );
        TargetTransform = GameObject.Find( ConnectionInstance.TargetName.Value );
        Debug.Log( SourceTransform.GetInstanceID().ToString() + TargetTransform.GetInstanceID().ToString() );
        if( SourceTransform != null && TargetTransform != null )
        {
            GameObject.Find( ConnectionInstance.TargetName.Value ).GetComponent<ObjectComponent>()._Value.Subscribe( _ => OnDataChanged() ).AddTo( disposables );
            IDisposable disposable = 
            SourceTransform.transform.ObserveEveryValueChanged( _ => _.position ).Subscribe( _ => UpdatePosition() );
            TargetTransform.transform.ObserveEveryValueChanged( _ => _.position ).Subscribe( _ => UpdatePosition() ).AddTo( disposables );
            gameObject.transform.ObserveEveryValueChanged( _ => _.position ).Subscribe( _ => UpdatePosition(), _ => MyOnComplete() ).AddTo( disposables );
            Debug.Log( "OnTargetUpdated****************************************" + disposable.ToString() );
            
        }
    }

    private void MyOnComplete()
    {
        Debug.Log( "---------------------------------------------------" );
    }

    private void OnDataChanged()
    {
        int formulaValue = 1;
        
        try
        {
            formulaValue = Int32.Parse( ConnectionInstance.Formula.Value );
        }
        catch( FormatException e )
        {

        }
        
        ConnectionInstance.Delta.Value = ConnectionInstance.Target.Value * formulaValue;
    }
    
    private void OnDeltaUpdate()
    {
       if( ConnectionInstance.Delta.Value > 0 )
            gameObject.GetComponent<RawImage>().color = new Color32( 141, 255, 156, 255 );
       else
            gameObject.GetComponent<RawImage>().color = new Color32( 255, 141, 141, 255 );
    }

    private void Awake()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        Debug.Log( "ConnectionComponent.Awake" );
    }
    
    private void UpdatePosition( float lineWidth = 20f )
    {
        Debug.Log( "ConnectionComponent.UpdatePosition" + TargetTransform.transform.position.x );
        Vector3 differenceVector = TargetTransform.transform.position - SourceTransform.transform.position;
        _rectTransform.sizeDelta = new Vector2( lineWidth, differenceVector.magnitude );
        float angle = Mathf.Atan2( differenceVector.x, differenceVector.y ) * Mathf.Rad2Deg;
        _rectTransform.rotation = Quaternion.Euler( 0, 0, -angle );
        _rectTransform.position = SourceTransform.transform.position;
    }

    private void OnDestroy()
    {
        Debug.Log( "ConnectionComponent.OnDestroy" );
        if( disposables != null )
        {
            disposables.Dispose();
            disposables = null;
            ConnectionInstance = null;
            SourceTransform = null;
            TargetTransform = null;
            _rectTransform = null;
        }
        
    }

}
