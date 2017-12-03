using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class JSONAtomModifierModel
{
    private float _food;
    private float _science;
    private float _words;
    private float _temperature;
    private float _pressure;
    private float _magnetic;
    private float _gravity;

    public float Food
    {
        get
        {
            return _food;
        }
        set
        {
            _food = value;
        }
    }
    public float Science
    {
        get
        {
            return _science;
        }
        set
        {
            _science = value;
        }
    }
    public float Words
    {
        get
        {
            return _words;
        }
        set
        {
            _words = value;
        }
    }
    public float Temperature
    {
        get
        {
            return _temperature;
        }
        set
        {
            _temperature = value;
        }
    }
    public float Pressure
    {
        get
        {
            return _pressure;
        }
        set
        {
            _pressure = value;
        }
    }
    public float Magnetic
    {
        get
        {
            return _magnetic;
        }
        set
        {
            _magnetic = value;
        }
    }
    public float Gravity
    {
        get
        {
            return _gravity;
        }
        set
        {
            _gravity = value;
        }
    }


}
