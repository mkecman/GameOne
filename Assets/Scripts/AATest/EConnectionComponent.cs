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
public class EConnectionComponent : GameView
{
    public EConnection Connection;
    public RawImage rawImage;
    public Text SourceDeltaText;
    public Text TargetDeltaText;

    private GameObject _source;
    private GameObject _target;
    private RectTransform _rectTransform;
    private double _newSourceValue;
    private double _newTargetValue;


    void Start()
    {
        GameMessage.Listen<ClockTickMessage>( Process );
        UpdateConnectionListeners();
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void Process( ClockTickMessage message )
    {
        _newSourceValue = getFormulaValue( Connection.SourceFormula.Value );
        Connection.SourceDelta.Value = -( Connection.Source.Value - _newSourceValue );
        _newTargetValue = getFormulaValue( Connection.TargetFormula.Value );
        Connection.TargetDelta.Value = -( Connection.Target.Value - _newTargetValue );

        Connection.Source.Delta = -( Connection.Source.Value - _newSourceValue );
        Connection.Target.Delta = -( Connection.Target.Value - _newTargetValue );

        Connection.Source.Value = _newSourceValue;
        Connection.Target.Value = _newTargetValue;

        Connection.Source._PastValues.Add( _newSourceValue );
        Connection.Target._PastValues.Add( _newTargetValue );
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

            //Connection.Target._Value.Throttle( TimeSpan.FromSeconds( 2 ) ).Subscribe( _ => OnDataChanged() ).AddTo( disposables );
            Connection.SourceFormula.Subscribe( _ => Connection.SourceDelta.Value = -( Connection.Source.Value - getFormulaValue( _ ) ) ).AddTo( disposables );
            Connection.TargetFormula.Subscribe( _ => Connection.TargetDelta.Value = -( Connection.Target.Value - getFormulaValue( _ ) ) ).AddTo( disposables );

            Connection.SourceDelta.Subscribe( _ => OnDeltaUpdate() ).AddTo( disposables );
            Connection.TargetDelta.Subscribe( _ => OnDeltaUpdate() ).AddTo( disposables );
        }
    }
    
    private double getFormulaValue( string formula )
    {
        double formulaValue = 0;
        
        try
        {
            formula = formula.Replace( "SourceDelta", Connection.SourceDelta.Value.ToString() );
            formula = formula.Replace( "TargetDelta", Connection.TargetDelta.Value.ToString() );

            var distinct = Regex.Matches( formula, "[^0123456789.()*/%+-]+" ).OfType<Match>().Select( _ => _.Value).Distinct();
            GameObject go;
            foreach( string item in distinct )
            {
                go = GameObject.Find( item );
                if( go == null )
                    throw new Exception( "Can't find EObject: " + item );

                formula = formula.Replace( item, go.GetComponent<EObject>().Value.ToString() );
            }
        formulaValue = Convert.ToDouble( new DataTable().Compute( formula, null ) );
        }
        catch( Exception e )
        {
            Debug.LogWarning( e.Message );
        }

        return formulaValue;
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
