using UnityEngine;
using System.Collections;
using System;
using System.Data;
using UniRx;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Linq;

[ExecuteInEditMode]
public class EEdgeComponent : GameView
{
    public EEdge Connection;
    public RawImage rawImage;
    public Text SourceDeltaText;
    public Text TargetDeltaText;

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
            Connection.Source = _source.GetComponent<ENode>();
            Connection.Target = _target.GetComponent<ENode>();

            //Connection.Target._Value.Throttle( TimeSpan.FromSeconds( 2 ) ).Subscribe( _ => OnDataChanged() ).AddTo( disposables );
            Connection.SourceFormula.Subscribe( _ => Connection.SourceDelta.Value = -( Connection.Source.Value - Connection.GetFormulaValue( _ ) ) ).AddTo( disposables );
            Connection.TargetFormula.Subscribe( _ => Connection.TargetDelta.Value = -( Connection.Target.Value - Connection.GetFormulaValue( _ ) ) ).AddTo( disposables );

            Connection.SourceDelta.Subscribe( _ => OnDeltaUpdate() ).AddTo( disposables );
            Connection.TargetDelta.Subscribe( _ => OnDeltaUpdate() ).AddTo( disposables );
        }
    }
    
    
    
    private void OnDeltaUpdate()
    {
       if( Connection.TargetDelta.Value > 0 )
            rawImage.color = new Color32( 141, 255, 156, 255 );
       else
            rawImage.color = new Color32( 255, 141, 141, 255 );

        SourceDeltaText.text = Connection.SourceDelta.Value.ToString( "F2" );
        TargetDeltaText.text = Connection.TargetDelta.Value.ToString( "F2" );
    }

    private void Awake()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }
    
    private void UpdatePosition( float lineWidth = 20f )
    {
        if( _source != null && _target != null )
        {
            Vector3 differenceVector = _source.transform.position - _target.transform.position;
            _rectTransform.sizeDelta = new Vector2( lineWidth, differenceVector.magnitude );
            float angle = Mathf.Atan2( differenceVector.x, differenceVector.y ) * Mathf.Rad2Deg;
            _rectTransform.rotation = Quaternion.Euler( 0, 0, -angle );
            _rectTransform.position = _target.transform.position;
            SourceDeltaText.transform.rotation = Quaternion.Euler( 0, 0, angle / 90 );
            TargetDeltaText.transform.rotation = Quaternion.Euler( 0, 0, angle / 90 );
        }
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
