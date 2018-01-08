using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class EConnectionComponent : GameView
{
    public EConnection Connection;

    private GameObject _source;
    private GameObject _target;
    private RectTransform _rectTransform;

    
    void Start()
    {
        UpdateConnectionListeners();
    }

    private void Update()
    {
        UpdatePosition();
    }

    public void UpdateConnectionListeners()
    {
        disposables.Clear();
        Connection.TargetName.Subscribe( _ => OnTargetUpdated() ).AddTo( disposables );
    }

    private void OnTargetUpdated()
    {
        _source = GameObject.Find( Connection.SourceName.Value );
        _target = GameObject.Find( Connection.TargetName.Value );
        if( _source != null && _target != null )
        {
            Connection.Source = _source.GetComponent<EObject>();
            Connection.Target = _target.GetComponent<EObject>();

            Connection.Target._Value.Subscribe( _ => OnDataChanged() ).AddTo( disposables );
            Connection.Formula.Subscribe( _ => OnDataChanged() ).AddTo( disposables );
            Connection.Delta.Subscribe( _ => OnDeltaUpdate() ).AddTo( disposables );
        }
    }
    
    private void OnDataChanged()
    {
        int formulaValue = 1;
        
        try
        {
            formulaValue = Int32.Parse( Connection.Formula.Value );
        }
        catch( FormatException e )
        {

        }
        
        Connection.Delta.Value = Connection.Target.Value * formulaValue;
    }
    
    private void OnDeltaUpdate()
    {
       if( Connection.Delta.Value > 0 )
            gameObject.GetComponent<RawImage>().color = new Color32( 141, 255, 156, 255 );
       else
            gameObject.GetComponent<RawImage>().color = new Color32( 255, 141, 141, 255 );
    }

    private void Awake()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }
    
    private void UpdatePosition( float lineWidth = 20f )
    {
        Vector3 differenceVector = _target.transform.position - _source.transform.position;
        _rectTransform.sizeDelta = new Vector2( lineWidth, differenceVector.magnitude );
        float angle = Mathf.Atan2( differenceVector.x, differenceVector.y ) * Mathf.Rad2Deg;
        _rectTransform.rotation = Quaternion.Euler( 0, 0, -angle );
        _rectTransform.position = _source.transform.position;
    }

    private void OnDestroy()
    {
        if( disposables != null )
        {
            disposables.Dispose();
            disposables = null;
            Connection = null;
            _source = null;
            _target = null;
            _rectTransform = null;
        }
        
    }

}
