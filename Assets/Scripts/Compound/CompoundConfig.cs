using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.IO;

public class CompoundConfig
{
    public List<CompoundDefinition> Items;
    private List<ElementModel> _elements;
    
    public void Load()
    {
        _elements = GameConfig.Get<ElementConfig>().Elements;

        TextAsset configFile = Resources.Load<TextAsset>( "Configs/Recipes-Common176" );
        Items = JsonConvert.DeserializeObject<CompoundConfig>( configFile.text ).Items;

        CompoundJSON compoundJSON;
        CompoundDefinition compoundDefinition;

        List<CompoundJSON> jsons = new List<CompoundJSON>();

        for( int i = 0; i < Items.Count; i++ )
        {
            compoundDefinition = Items[ i ];
            compoundJSON = new CompoundJSON
            {
                Name = compoundDefinition.Name,
                Formula = compoundDefinition.Formula,
                MolecularMass = compoundDefinition.MolecularMass
            };
            compoundJSON.Elements = SplitFormula( compoundJSON.Formula );

            jsons.Add( compoundJSON );
        }

        File.WriteAllText(
            Application.persistentDataPath + "CompoundConfig.json",
            JsonConvert.SerializeObject( jsons )
            );

        Debug.Log( "DOOOOOOOOOOOOONNNNNNNNNNNEEEEEEEEEEE" );
    }

    private List<LifeElementModel> SplitFormula( string formula )
    {
        List<LifeElementModel> output = new List<LifeElementModel>();
        foreach( KeyValuePair<string, LifeElementModel> item in splitFormula( formula ) )
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

    private Dictionary<string, LifeElementModel> splitFormula( string formula, int multiplier = 1 )
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
                foreach( KeyValuePair<string, LifeElementModel> item in splitFormula( symbol, result ) )
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

    private ElementModel GetAtom( string symbol )
    {
        for( int i = 0; i < _elements.Count; i++ )
        {
            if( _elements[ i ].Symbol == symbol )
                return _elements[ i ];
        }

        return null;
    }
}
