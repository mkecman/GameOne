using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;

public class CompoundGenerator : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        ConvertOldConfigToNewFormat();
    }

    public List<CompoundDefinition> Items;
    private List<ElementModel> _elements;

    //call to convert and generate new config
    public void ConvertOldConfigToNewFormat()
    {
        _elements = GameConfig.Get<ElementConfig>().Elements;

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
