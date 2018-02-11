using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

public class LifeView : GameView
{
    public Transform ElementPanel;
    public GameObject ElementPrefab;
    public PropertyView Population;
    public PropertyView Science;
    public PropertyView Words;

    void OnEnable()
    {
        GameModel.HandleGet<LifeModel>( onLifeModel );
        Debug.Log( "LifeView OnEnable" );
    }
    
    private void onLifeModel( LifeModel model )
    {
        Debug.Log( "onLifeModel:" + model.Name );

        PropertyViewModel propertyModel = new PropertyViewModel();
        propertyModel.showDelta = true;

        propertyModel.property = ElementModifiers.Food;
        propertyModel.value = model._Population;
        Population.UpdateModel( propertyModel );

        propertyModel.property = ElementModifiers.Science;
        propertyModel.value = model._Science;
        Science.UpdateModel( propertyModel );

        propertyModel.property = ElementModifiers.Words;
        propertyModel.value = model._Words;
        Words.UpdateModel( propertyModel );

        /*
        model._WorkingElements.ObserveAdd().Subscribe( x => AddWorkingElement( x.Value ) ).AddTo( disposables );

        foreach( KeyValuePair<int, WorkedElementModel> item in model._WorkingElements )
        {
            AddWorkingElement( item.Value );
        }
        */
    }
    
    private void AddWorkingElement( WorkedElementModel model )
    {
        GameObject elementPrefabInstance = Instantiate( ElementPrefab, ElementPanel );
        elementPrefabInstance.GetComponent<LifeWorkingElement>().UpdateModel( model );
    }
}
