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

        model._WorkingElements.ObserveAdd().Subscribe( x => AddWorkingElement( x.Value ) ).AddTo( disposables );

        foreach( KeyValuePair<int, WorkedElementModel> item in model._WorkingElements )
        {
            AddWorkingElement( item.Value );
        }

        //model._WorkingElements.Add( 2, new WorkedElementModel( 2, 3 ) );
        //model._WorkingElements.Add( 3, new WorkedElementModel( 3, 5 ) );
    }
    
    private void AddWorkingElement( WorkedElementModel model )
    {
        GameObject elementPrefabInstance = Instantiate( ElementPrefab, ElementPanel );
        elementPrefabInstance.GetComponent<LifeWorkingElement>().UpdateModel( model );
    }
}
