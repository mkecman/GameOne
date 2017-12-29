using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
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

    void Awake()
    {
        _elements = Config.Get<ElementConfig>().Elements;
    }
    
    public void UpdateModel( WorkedElementModel model )
    {
        ElementModel element = _elements[ model.Index ];

        Name.text = element.Name;

        WorkersPanelTarget.Id = model.Index;
        for( int i = 0; i < model.Workers; i++ )
        {
            AddWorker( model.Index );
        }
        
        AddModifier( element.Modifier( ElementModifiers.Food ), ModifiersFirstGroup );
        AddModifier( element.Modifier( ElementModifiers.Science ), ModifiersFirstGroup );
        AddModifier( element.Modifier( ElementModifiers.Words ), ModifiersFirstGroup );

        AddModifier( element.Modifier( ElementModifiers.Temperature ), ModifiersSecondGroup );
        AddModifier( element.Modifier( ElementModifiers.Pressure ), ModifiersSecondGroup );
        AddModifier( element.Modifier( ElementModifiers.Gravity ), ModifiersSecondGroup );
        AddModifier( element.Modifier( ElementModifiers.Radiation ), ModifiersSecondGroup );
    }

    private void AddWorker( int index )
    {
        GameObject instance = Instantiate( WorkerPrefab, WorkersPanel );
        instance.GetComponent<Worker>().UpdateModel( index );
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
