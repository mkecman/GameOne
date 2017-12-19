using UnityEngine;
using System.Collections;
using System;

public class LifeView : MonoBehaviour
{
    public Transform ElementPanel;
    public GameObject ElementPrefab;
    public PropertyView Population;
    public PropertyView Science;

    // Use this for initialization
    void OnEnable()
    {
        LifeModel _life = getModel<LifeModel>();
    }

    private T getModel<T>()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
