using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ElementModifier : MonoBehaviour
{
    public Text Property;
    public Text Value;

    internal void UpdateModel( ElementModifierModel model )
    {
        Property.text = model.Property.ToString();
        Value.text = model.Delta.ToString();
    }
}
