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
        GameModel.Bind<LifeModel>( onLifeModel );
        Debug.Log( "LifeView OnEnable" );
    }
    
    private void onLifeModel( LifeModel model )
    {
        Debug.Log( "onLifeModel:" + model.Name );

        Population.SetModel( ElementModifiers.Food, model._Population );
        Science.SetModel( ElementModifiers.Science, model._Science );
        Words.SetModel( ElementModifiers.Words, model._Words );

        model._WorkingElements.ObserveAdd().Subscribe( x => AddWorkingElement( x.Value ) ).AddTo( disposables );

        foreach( KeyValuePair<int, WorkedElementModel> item in model._WorkingElements )
        {
            AddWorkingElement( item.Value );
        }
    }
    
    private void AddWorkingElement( WorkedElementModel model )
    {
        GameObject elementPrefabInstance = Instantiate( ElementPrefab, ElementPanel );
        elementPrefabInstance.GetComponent<LifeWorkingElement>().UpdateModel( model );
    }
}
