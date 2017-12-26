using UnityEngine;
using System.Collections;
using System;

public class LifeView : MonoBehaviour
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

        Population.SetModel( ElementModifiers.FOOD, model._Population );
        Science.SetModel( ElementModifiers.SCIENCE, model._Science );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
