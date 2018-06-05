﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using System;

public class CompoundGenerator : MonoBehaviour
{

    public List<CompoundDefinition> Items;
    private Dictionary<int, ElementData> _elements;
    private List<ElementData> _elementsList;
    private List<BuildingModel> _buildings;
    private Dictionary<int, Dictionary<ElementRarityClass, CompoundLevelData>> _levelConfig;
    private List<CompoundJSON> _compounds;
    private int _indexer;
    private Dictionary<ElementRarityClass, List<WeightedValue>> _elementsProbabilities;
    private Color[] _pixels;
    private List<CompoundExcelJSON> _rawCompounds;

    // Use this for initialization
    void Start()
    {
        _elements = GameConfig.Get<ElementConfig>().ElementsDictionary;
        _elementsList = GameConfig.Get<ElementConfig>().ElementsList;
        _buildings = GameConfig.Get<BuildingConfig>().Buildings;
        _compounds = GameConfig.Get<CompoundConfig>();

        _elementsProbabilities = new Dictionary<ElementRarityClass, List<WeightedValue>>();
        _elementsProbabilities.Add( ElementRarityClass.Abundant, new List<WeightedValue>() );
        _elementsProbabilities.Add( ElementRarityClass.Common, new List<WeightedValue>() );
        _elementsProbabilities.Add( ElementRarityClass.Uncommon, new List<WeightedValue>() );
        _elementsProbabilities.Add( ElementRarityClass.Rare, new List<WeightedValue>() );
        for( int i = 0; i < _elementsList.Count; i++ )
        {
            _elementsProbabilities[ _elementsList[ i ].RarityClass ].Add( new WeightedValue( _elementsList[ i ].Index, 0 ) );
        }
        foreach( KeyValuePair<ElementRarityClass, List<WeightedValue>> probs in _elementsProbabilities )
        {
            foreach( WeightedValue item in probs.Value )
            {
                item.Weight = 1f / probs.Value.Count;
            }
        }

        _levelConfig = new Dictionary<int, Dictionary<ElementRarityClass, CompoundLevelData>>();

        ///level 1
        Dictionary<ElementRarityClass, CompoundLevelData> temp = new Dictionary<ElementRarityClass, CompoundLevelData>
        {
            { ElementRarityClass.Abundant, new CompoundLevelData( ElementRarityClass.Abundant, 3, 0.8f, 300, 2, 4 ) },
            { ElementRarityClass.Common, new CompoundLevelData( ElementRarityClass.Common, 3, 0.2f, 300, 1, 1 ) },
            { ElementRarityClass.Uncommon, new CompoundLevelData( ElementRarityClass.Uncommon, 3, 0f, 300, 0, 0 ) },
            { ElementRarityClass.Rare, new CompoundLevelData( ElementRarityClass.Rare, 3, 0f, 300, 0, 0 ) }
        };
        _levelConfig.Add( 1, temp );
        ///level 2
        temp = new Dictionary<ElementRarityClass, CompoundLevelData>
        {
            { ElementRarityClass.Abundant, new CompoundLevelData( ElementRarityClass.Abundant, 8, 0.4f, 900, 1, 2 ) },
            { ElementRarityClass.Common, new CompoundLevelData( ElementRarityClass.Common, 8, 0.4f, 900, 1, 2 ) },
            { ElementRarityClass.Uncommon, new CompoundLevelData( ElementRarityClass.Uncommon, 8, 0.2f, 900, 1, 1 ) },
            { ElementRarityClass.Rare, new CompoundLevelData( ElementRarityClass.Rare, 8, 0f, 900, 0, 0 ) }
        };
        _levelConfig.Add( 2, temp );
        ///level 3
        temp = new Dictionary<ElementRarityClass, CompoundLevelData>
        {
            { ElementRarityClass.Abundant, new CompoundLevelData( ElementRarityClass.Abundant, 13, 0.2f, 8000, 1, 1 ) },
            { ElementRarityClass.Common, new CompoundLevelData( ElementRarityClass.Common, 13, 0.2f, 8000, 1, 1 ) },
            { ElementRarityClass.Uncommon, new CompoundLevelData( ElementRarityClass.Uncommon, 13, 0.4f, 8000, 1, 2 ) },
            { ElementRarityClass.Rare, new CompoundLevelData( ElementRarityClass.Rare, 13, 0.2f, 8000, 1, 1 ) }
        };
        _levelConfig.Add( 3, temp );
        ///level 4
        temp = new Dictionary<ElementRarityClass, CompoundLevelData>
        {
            { ElementRarityClass.Abundant, new CompoundLevelData( ElementRarityClass.Abundant, 18, 0.2f, 24000, 1, 1 ) },
            { ElementRarityClass.Common, new CompoundLevelData( ElementRarityClass.Common, 18, 0.2f, 24000, 1, 1 ) },
            { ElementRarityClass.Uncommon, new CompoundLevelData( ElementRarityClass.Uncommon, 18, 0.2f, 24000, 1, 1 ) },
            { ElementRarityClass.Rare, new CompoundLevelData( ElementRarityClass.Rare, 18, 0.4f, 24000, 1, 2 ) }
        };
        _levelConfig.Add( 4, temp );

        _compounds = new List<CompoundJSON>();
        _compounds.Add( new CompoundJSON() );
        _indexer = 1;
        ///////END SETUP


        GenerateConsumableCompounds();

        GenerateArmorCompounds();
        GenerateWeaponCompounds();
        //ConvertOldConfigToNewFormat();

        SaveToFile();
    }

    private void GenerateConsumableCompounds()
    {
        //load json exported from a spreadsheet
        TextAsset configFile = Resources.Load<TextAsset>( "Configs/CompoundExcelConfig" );
        _rawCompounds = JsonConvert.DeserializeObject<List<CompoundExcelJSON>>( configFile.text );

        for( int i = 0; i < _rawCompounds.Count; i++ )
        {
            CompoundExcelJSON rawCompound = _rawCompounds[ i ];
            CompoundJSON compound = new CompoundJSON
            {
                Index = _indexer,
                Type = rawCompound.Type,
                Name = rawCompound.Name,
                MolecularMass = rawCompound.MolecularMass
            };
            if( rawCompound.Effect != R.Default )
                compound.Effects.Add( rawCompound.Effect, rawCompound.EffectDelta );

            for( int j = 0; j < 5; j++ )
            {
                string symbol = (string)GetPropValue( rawCompound, "E" + j );
                if( symbol != "-" )
                compound.Elements.Add( 
                    new LifeElementModel( 
                        GetAtom( symbol ).Index, 
                        symbol, 
                        0, (int)GetPropValue( rawCompound, "A" + j ) ) );
            }

            foreach( LifeElementModel item in compound.Elements )
            {
                compound.Formula += item.Symbol + item.MaxAmount + " ";
            }

            CreateCompoundTexture( compound );

            _compounds.Add( compound );
            _indexer++;
        }
    }

    public object GetPropValue( object src, string propName )
    {
        return src.GetType().GetField( propName ).GetValue( src );
    }

    private void GenerateWeaponCompounds()
    {
        bool isPositive = true;
        for( int i = 1; i <= 4; i++ )
        {
            CreateCompound( R.Temperature, i, isPositive, CompoundType.Weapon );
            CreateCompound( R.Pressure, i, isPositive, CompoundType.Weapon );
            CreateCompound( R.Humidity, i, isPositive, CompoundType.Weapon );
            CreateCompound( R.Radiation, i, isPositive, CompoundType.Weapon );
            isPositive = false;
            CreateCompound( R.Temperature, i, isPositive, CompoundType.Weapon );
            CreateCompound( R.Pressure, i, isPositive, CompoundType.Weapon );
            CreateCompound( R.Humidity, i, isPositive, CompoundType.Weapon );
            CreateCompound( R.Radiation, i, isPositive, CompoundType.Weapon );
            isPositive = true;
        }

    }

    private void GenerateArmorCompounds()
    {
        bool isPositive = true;
        for( int i = 1; i <= 4; i++ )
        {
            CreateCompound( R.Temperature, i, isPositive );
            CreateCompound( R.Pressure, i, isPositive );
            CreateCompound( R.Humidity, i, isPositive );
            CreateCompound( R.Radiation, i, isPositive );
            isPositive = false;
            CreateCompound( R.Temperature, i, isPositive );
            CreateCompound( R.Pressure, i, isPositive );
            CreateCompound( R.Humidity, i, isPositive );
            CreateCompound( R.Radiation, i, isPositive );
            isPositive = true;
        }
    }

    private void CreateCompound( R effect, int level, bool isPositive, CompoundType compoundType = CompoundType.Armor )
    {
        float sign = isPositive ? 1 : -1;
        float delta = _levelConfig[ level ][ ElementRarityClass.Abundant ].Impact * sign;
        CompoundJSON compound = new CompoundJSON
        {
            Index = _indexer,
            Type = compoundType,
            Name = compoundType.ToString() + " #" + _indexer
        };
        compound.Effects.Add( effect, delta );
        if( compoundType == CompoundType.Weapon )
            compound.Effects.Add( R.Attack, 4+level );

        compound.Elements.AddRange( CreateCompoundElements( compound, level, ElementRarityClass.Rare ) );
        compound.Elements.AddRange( CreateCompoundElements( compound, level, ElementRarityClass.Uncommon ) );
        compound.Elements.AddRange( CreateCompoundElements( compound, level, ElementRarityClass.Common ) );
        compound.Elements.AddRange( CreateCompoundElements( compound, level, ElementRarityClass.Abundant ) );

        foreach( LifeElementModel item in compound.Elements )
        {
            compound.Formula += item.Symbol + item.MaxAmount + " ";
        }

        CreateCompoundTexture( compound );

        _compounds.Add( compound );
        _indexer++;
    }

    public void CreateCompoundTexture( CompoundJSON compound )
    {
        Gradient gradient = new Gradient();
        GradientColorKey[] gradientColorKeys = new GradientColorKey[ compound.Elements.Count ];
        GradientAlphaKey[] gradientAlphaKeys = new GradientAlphaKey[ compound.Elements.Count ];

        float increment = 1f / compound.Elements.Count;
        float time = increment / 2f;
        Color mColor;
        for( int i = 0; i < compound.Elements.Count; i++ )
        {
            gradientAlphaKeys[ i ].alpha = 1f;
            gradientAlphaKeys[ i ].time = time;

            ColorUtility.TryParseHtmlString( _elements[ compound.Elements[ i ].Index ].Color, out mColor );
            gradientColorKeys[ i ].color = mColor;
            gradientColorKeys[ i ].time = time;

            time += increment;
        }
        gradient.SetKeys( gradientColorKeys, gradientAlphaKeys );

        _pixels = new Color[ 100 ];
        for( int y = 0; y < 100; y++ )
        {
            _pixels[ y ] = gradient.Evaluate( y / 100f );
        }
        Texture2D _texture = new Texture2D( 1, 100, TextureFormat.RGB24, false );
        _texture.SetPixels( _pixels );
        _texture.wrapMode = TextureWrapMode.Clamp;
        _texture.Apply();

        byte[] bytes = _texture.EncodeToPNG();
        File.WriteAllBytes( Application.persistentDataPath + "-" + _indexer + ".png", bytes );
    }

    private List<LifeElementModel> CreateCompoundElements( CompoundJSON compound, int level, ElementRarityClass rarityClass )
    {
        List<LifeElementModel> output = new List<LifeElementModel>();

        CompoundLevelData armorLevelData = _levelConfig[ level ][ rarityClass ];
        int numberOfElements = RandomUtil.FromRangeInt( armorLevelData.Min, armorLevelData.Max );
        float amountNeeded = Mathf.Ceil( armorLevelData.PercentOfCompound * armorLevelData.Price );
        List<WeightedValue> probabilities = GameModel.Copy( _elementsProbabilities[ rarityClass ] );
        WeightedValue weightedValue;
        int index = 0;
        for( int i = 0; i < numberOfElements; i++ )
        {
            weightedValue = RandomUtil.GetWeightedValueObject( probabilities );
            index = (int)weightedValue.Value;
            output.Add( new LifeElementModel( index, _elements[ index ].Symbol, 0, 0 ) );
            probabilities.Remove( weightedValue );
            foreach( WeightedValue item in probabilities )
            {
                item.Weight = 1f / probabilities.Count;
            }
        }
        //only exception is if Hydrogen is selected, we'll add another abundand element to avoid big numbers of Hydrogen
        if( rarityClass == ElementRarityClass.Abundant && index == 1 )
        {
            weightedValue = RandomUtil.GetWeightedValueObject( probabilities );
            index = (int)weightedValue.Value;
            output.Add( new LifeElementModel( index, _elements[ index ].Symbol, 0, 0 ) );
            numberOfElements++;
        }


            float amountCollected = 0;
        int fullElements = 0;
        bool isFull = false;
        while( !isFull )
        {
            fullElements = 0;
            for( int i = 0; i < numberOfElements; i++ )
            {
                if( _elements[ output[ i ].Index ].Weight + amountCollected > amountNeeded )
                    fullElements++;
                else
                {
                    output[ i ].MaxAmount++;
                    amountCollected += _elements[ output[ i ].Index ].Weight;
                }
            }
            if( fullElements == numberOfElements )
                isFull = true;
        }

        compound.MolecularMass += amountCollected;

        return output;
    }

    private void SaveToFile()
    {
        //SAVE TO FILE
        File.WriteAllText(
            Application.persistentDataPath + "-CompoundConfig.json",
            JsonConvert.SerializeObject( _compounds )
            );

        Debug.Log( "DONE Generating Compounds" );
    }





    //OLD STUFF BELOW
    //call to convert and generate new config
    public void ConvertOldConfigToNewFormat()
    {
        TextAsset configFile = Resources.Load<TextAsset>( "Configs/Recipes-Common176" );
        Items = JsonConvert.DeserializeObject<CompoundGenerator>( configFile.text ).Items;

        CompoundJSON compoundJSON;
        CompoundDefinition compoundDefinition;

        List<CompoundJSON> jsons = new List<CompoundJSON>();

        for( int i = 0; i < Items.Count; i++ )
        {
            compoundDefinition = Items[ i ];
            compoundJSON = new CompoundJSON
            {
                Name = compoundDefinition.Name,
                Type = (CompoundType)RandomUtil.FromRangeInt( 0, 2 ),
                Formula = compoundDefinition.Formula,
                MolecularMass = compoundDefinition.MolecularMass
            };
            compoundJSON.Elements = SplitFormula( compoundJSON.Formula );
            if( i < _buildings.Count )
                compoundJSON.Effects = _buildings[ i ].Effects;

            jsons.Add( compoundJSON );
        }

        Dictionary<string, int> eleDict = new Dictionary<string, int>();
        for( int i = 0; i < jsons.Count; i++ )
        {
            for( int j = 0; j < jsons[ i ].Elements.Count; j++ )
            {
                if( eleDict.ContainsKey( jsons[ i ].Elements[ j ].Symbol ) )
                {
                    eleDict[ jsons[ i ].Elements[ j ].Symbol ] += 1;
                }
                else
                {
                    eleDict.Add( jsons[ i ].Elements[ j ].Symbol, 1 );
                }
            }

        }

        //Debug.Log( JsonConvert.SerializeObject( eleDict ) );

        File.WriteAllText(
            Application.persistentDataPath + "CompoundConfig.json",
            JsonConvert.SerializeObject( jsons )
            );

        Debug.Log( "DOOOOOOOOOOOOONNNNNNNNNNNEEEEEEEEEEE" );
    }

    private List<LifeElementModel> SplitFormula( string formula )
    {
        List<LifeElementModel> output = new List<LifeElementModel>();
        foreach( KeyValuePair<string, LifeElementModel> item in GetSplitDictionary( formula ) )
        {
            output.Add( item.Value );
        }
        output.Sort( CompareLifeElement );

        return output;
    }

    private int CompareLifeElement( LifeElementModel x, LifeElementModel y )
    {
        if( x.Index > y.Index )
            return 1;
        if( x.Index < y.Index )
            return -1;

        return 0;
    }

    private Dictionary<string, LifeElementModel> GetSplitDictionary( string formula, int multiplier = 1 )
    {
        var pattern = @"\((.*?)\)|([A-Z][a-z])|([A-Z])|\d+"; //find between brackets, Uppercase+lowercase, Uppercase, digit
        var matches = Regex.Matches( formula, pattern );

        Dictionary<string, LifeElementModel> atomDict = new Dictionary<string, LifeElementModel>();

        for( int i = 0; i < matches.Count; i++ )
        {
            string symbol = matches[ i ].ToString();
            if( symbol.StartsWith( "(" ) )
            {
                symbol = symbol.Substring( 1, symbol.Length - 1 );
                int result = 1;
                int.TryParse( matches[ i + 1 ].ToString(), out result );
                foreach( KeyValuePair<string, LifeElementModel> item in GetSplitDictionary( symbol, result ) )
                {
                    if( atomDict.ContainsKey( item.Key ) )
                        atomDict[ item.Key ].Amount += item.Value.Amount;
                    else
                        atomDict.Add( item.Key, new LifeElementModel( GetAtom( item.Value.Symbol ).Index, item.Value.Symbol, item.Value.Amount ) );
                }
                i++;
            }
            else
            {
                int amount = 1;
                if( i + 1 < matches.Count )
                {
                    int number;
                    var isNumeric = int.TryParse( matches[ i + 1 ].ToString(), out number );
                    if( isNumeric )
                    {
                        amount = number;
                        i++;
                    }
                }
                amount *= multiplier;

                LifeElementModel atomModel;
                if( atomDict.TryGetValue( symbol, out atomModel ) )
                    atomModel.Amount += amount;
                else
                    atomDict.Add( symbol, new LifeElementModel( GetAtom( symbol ).Index, symbol, amount ) );
            }
        }
        return atomDict;
    }

    private ElementData GetAtom( string symbol )
    {
        for( int i = 0; i < _elementsList.Count; i++ )
        {
            if( _elementsList[ i ].Symbol == symbol )
                return _elementsList[ i ];
        }

        return null;
    }
}

internal class CompoundLevelData
{
    public ElementRarityClass RarityClass;
    public float Impact;
    public float PercentOfCompound;
    public int Price;
    public int Min;
    public int Max;

    public CompoundLevelData( ElementRarityClass rarityClass, float delta, float weight, int price, int min, int max )
    {
        RarityClass = rarityClass;
        Impact = delta;
        PercentOfCompound = weight;
        Price = price;
        Min = min;
        Max = max;
    }
}


