using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

public class ElementConfigGenerator
{
    public List<JSONAtomModel> atoms;
    public List<ElementModel> elements;
    
    internal void Load()
    {
        Debug.Log( "Load" );

        TextAsset targetFile = Resources.Load<TextAsset>( "Configs/AtomsJSON" );
        atoms = JsonConvert.DeserializeObject<List<JSONAtomModel>>( targetFile.text );
        
        elements = new List<ElementModel>();
        for( int i = 0; i < atoms.Count; i++ )
        {
            ElementModel tempElement = new ElementModel();
            tempElement.Name = atoms[ i ].Name;
            tempElement.Symbol = atoms[ i ].Symbol;
            tempElement.Index = atoms[ i ].Index;
            tempElement.Weight = atoms[ i ].Weight;
            tempElement.Density = atoms[ i ].Density;
            tempElement.HexColor = atoms[ i ].HexColor;
            tempElement.GroupBlock = atoms[ i ].GroupBlock;

            //tempElement.Modifiers = new List<ElementModifierModel>();
            tempElement._Modifiers.Add( CreateModifier( ElementModifiers.Food, atoms[ i ].Food ) );
            tempElement._Modifiers.Add( CreateModifier( ElementModifiers.Science, atoms[ i ].Science ) );
            tempElement._Modifiers.Add( CreateModifier( ElementModifiers.Words, atoms[ i ].Words ) );
            /*tempElement._Modifiers.Add( CreateModifier( ElementModifiers.Temperature, atoms[ i ].Temperature ) );
            tempElement._Modifiers.Add( CreateModifier( ElementModifiers.Pressure, atoms[ i ].Pressure ) );
            tempElement._Modifiers.Add( CreateModifier( ElementModifiers.Gravity, atoms[ i ].Gravity ) );
            tempElement._Modifiers.Add( CreateModifier( ElementModifiers.Radiation, atoms[ i ].Radiation ) );*/

            elements.Add( tempElement );
        }

        Save();
    }

    internal ElementModifierModel CreateModifier( ElementModifiers Name, double Delta )
    {
        ElementModifierModel tempModifier = new ElementModifierModel();
        tempModifier.Property = Name;
        tempModifier.Delta = Delta;
        return tempModifier;
    }
    
    internal void Save()
    {
        File.WriteAllText( Application.persistentDataPath + "-ElementConfig.json", JsonConvert.SerializeObject( elements ) );
        Debug.Log( Application.persistentDataPath + "-ElementConfig.json" );
    }
    
}
