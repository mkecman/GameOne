using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using System;

public class LifeWorkingElement : GameView
{
    public Text Name;
    public Transform ModifiersFirstGroup;
    public Transform ModifiersSecondGroup;
    public GameObject ModifierPrefab;
    public Transform WorkersPanel;
    public RaycastTarget WorkersPanelTarget;
    public GameObject WorkerPrefab;

    private List<ElementModel> _elements;
    private List<Worker> _workers;
    private WorkedElementModel _model;

    void Awake()
    {
        _elements = Config.Get<ElementConfig>().Elements;
        _workers = new List<Worker>();
    }
    
    public void UpdateModel( WorkedElementModel model )
    {
        _model = model;
        ElementModel element = _elements[ model.Index ];

        Name.text = element.Name;

        WorkersPanelTarget.Id = model.Index;

        //UpdateWorkers( model.Workers );
        model._Workers.Subscribe( x => UpdateWorkers( x ) ).AddTo( disposables );
        
        AddModifier( element.Modifier( ElementModifiers.Food ), ModifiersFirstGroup );
        AddModifier( element.Modifier( ElementModifiers.Science ), ModifiersFirstGroup );
        AddModifier( element.Modifier( ElementModifiers.Words ), ModifiersFirstGroup );

        AddModifier( element.Modifier( ElementModifiers.Temperature ), ModifiersSecondGroup );
        AddModifier( element.Modifier( ElementModifiers.Pressure ), ModifiersSecondGroup );
        AddModifier( element.Modifier( ElementModifiers.Gravity ), ModifiersSecondGroup );
        AddModifier( element.Modifier( ElementModifiers.Radiation ), ModifiersSecondGroup );
    }

    private void UpdateWorkers( int count )
    {
        int diference = count - _workers.Count;
        //Debug.Log( _elements[ _model.Index ].Name + " => Have: " + _workers.Count + ", New: " + count + ", Diff: " + diference );

        if (diference > 0 )
            for( int i = 0; i < diference; i++ )
            {
                GameObject instance = Instantiate( WorkerPrefab, WorkersPanel );
                Worker worker = instance.GetComponent<Worker>();
                worker.UpdateModel( _model.Index );
                _workers.Add( worker );
            }

        if( diference < 0 )
        {
            for( int i = 0; i < Math.Abs( diference ); i++ )
            {
                Destroy( _workers[ 0 ].gameObject );
                _workers.RemoveAt( 0 );
            }
        }
    }
    
    private void AddModifier( ElementModifierModel model, Transform container )
    {
        if( model.Delta != 0 )
        {
            GameObject instance = Instantiate( ModifierPrefab, container );
            instance.GetComponent<ElementModifier>().UpdateModel( model );
        }
    }


}
