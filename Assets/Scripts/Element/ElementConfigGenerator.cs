using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Reflection;
using LitJson;
using System.Text;

public class ElementConfigGenerator
{
    public List<JSONAtomModel> atoms;
    public List<ElementModel> elements;
    
    internal void Load()
    {
        Debug.Log( "Load" );

        TextAsset targetFile = Resources.Load<TextAsset>( "Configs/Atoms" );
        atoms = JsonMapper.ToObject<List<JSONAtomModel>>( targetFile.text );
        
        elements = new List<ElementModel>();
        for( int i = 0; i < atoms.Count; i++ )
        {
            ElementModel tempElement = new ElementModel();
            tempElement.Name = atoms[ i ].Name;
            tempElement.Symbol = atoms[ i ].Symbol;
            tempElement.Index = atoms[ i ].Index;
            tempElement.Weight = atoms[ i ].Weight;
            tempElement.HexColor = atoms[ i ].HexColor;
            tempElement.GroupBlock = atoms[ i ].GroupBlock;

            tempElement.Modifiers = new List<ElementModifierModel>();
            tempElement.Modifiers.Add( CreateModifier( ElementModifiers.FOOD, atoms[ i ].Food ) );
            tempElement.Modifiers.Add( CreateModifier( ElementModifiers.SCIENCE, atoms[ i ].Science ) );
            tempElement.Modifiers.Add( CreateModifier( ElementModifiers.WORDS, atoms[ i ].Words ) );
            tempElement.Modifiers.Add( CreateModifier( ElementModifiers.TEMPERATURE, atoms[ i ].Temperature ) );
            tempElement.Modifiers.Add( CreateModifier( ElementModifiers.PRESSURE, atoms[ i ].Pressure ) );
            tempElement.Modifiers.Add( CreateModifier( ElementModifiers.GRAVITY, atoms[ i ].Gravity ) );
            tempElement.Modifiers.Add( CreateModifier( ElementModifiers.RADIATION, atoms[ i ].Radiation ) );

            elements.Add( tempElement );
        }

        Save();
    }

    internal ElementModifierModel CreateModifier( string Name, double Delta )
    {
        ElementModifierModel tempModifier = new ElementModifierModel();
        tempModifier.Property = Name;
        tempModifier.Delta = Delta;
        return tempModifier;
    }
    
    internal void Save()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter( sb );
        writer.PrettyPrint = true;
        JsonMapper.ToJson( elements, writer );
        File.WriteAllText( Application.persistentDataPath + "-Elements.json", sb.ToString() );
        Debug.Log( Application.persistentDataPath + "-Elements.json" );
    }
    
}
