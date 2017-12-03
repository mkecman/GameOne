using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Reflection;

public class ElementConfig
{
    public List<JSONAtomModel> atoms;
    public List<JSONAtomModifierModel> modifiers;
    public List<ElementModel> elements;
    
    internal void Load()
    {
        Debug.Log( "Load" );

        TextAsset targetFile = Resources.Load<TextAsset>( "Configs/Atoms" );
        atoms = JsonHelper.FromJsonList<JSONAtomModel>( targetFile.text );

        TextAsset targetFile2 = Resources.Load<TextAsset>( "Configs/ElementModifiers" );
        modifiers = JsonHelper.FromJsonList<JSONAtomModifierModel>( targetFile2.text );

        elements = new List<ElementModel>();
        for( int i = 0; i < atoms.Count; i++ )
        {
            ElementModel tempElement = new ElementModel();
            tempElement.Name = atoms[ i ].Name;
            tempElement.Symbol = atoms[ i ].Symbol;
            tempElement.Index = atoms[ i ].AtomicNumber;
            tempElement.Weight = atoms[ i ].AtomicWeight;
            tempElement.HexColor = atoms[ i ].HexColor;
            tempElement.GroupBlock = atoms[ i ].GroupBlock;

            tempElement.Modifiers = new List<ElementModifierModel>();
            AddModifier( ElementModifier.FOOD, tempElement, i );
            AddModifier( ElementModifier.SCIENCE, tempElement, i );
            AddModifier( ElementModifier.WORDS, tempElement, i );
            AddModifier( ElementModifier.TEMPERATURE, tempElement, i );
            AddModifier( ElementModifier.PRESSURE, tempElement, i );
            AddModifier( ElementModifier.GRAVITY, tempElement, i );
            AddModifier( ElementModifier.MAGNETIC, tempElement, i );

            elements.Add( tempElement );
        }

        Save();

    }

    internal void AddModifier( string Name, ElementModel tempElement, int index )
    {
        ElementModifierModel tempModifier = new ElementModifierModel();
        tempModifier.Property = Name;
        tempModifier.Delta = (float)modifiers[ index ].GetType().GetProperty( Name, BindingFlags.Public | BindingFlags.Instance ).GetValue( modifiers[ index ], null );
        
        tempElement.Modifiers.Add( tempModifier );
    }
    
    internal void Save()
    {
        File.WriteAllText( Application.persistentDataPath + "-Elements.json", JsonUtility.ToJson( elements, true ) );
        Debug.Log( Application.persistentDataPath + "-Elements.json" );
    }
    
}
