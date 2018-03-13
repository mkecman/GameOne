using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class AbilityGenerator : MonoBehaviour
{
    private List<R> propertyMap = new List<R>() { R.Default, R.Temperature, R.Pressure, R.Humidity, R.Radiation, R.Temperature, R.Pressure, R.Humidity, R.Radiation };
    // Use this for initialization
    void Start()
    {
        CSV.Add( "Temperature+,Pressure+,Humidity+,Radiation+,Temperature-,Pressure-,Humidity-,Radiation-" );
        GetCombination( new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 } );
    }

    public void GetCombination( List<int> list )
    {
        //MAKE COMBINATIONS
        List<AbilityJSON> _tempAbilities = new List<AbilityJSON>();
        double count = Math.Pow( 2, list.Count );
        AbilityJSON ability;
        for( int i = 1; i <= count - 1; i++ )
        {
            string str = Convert.ToString( i, 2 ).PadLeft( list.Count, '0' );
            ability = new AbilityJSON();
            for( int j = 0; j < str.Length; j++ )
            {
                if( str[ j ] == '1' )
                {
                    if( list[ j ] > 4 )
                    {
                        ability.Decreases[ (int)propertyMap[ list[ j ] ] ] = -0.01;
                    }
                    else
                        ability.Increases[ (int)propertyMap[ list[ j ] ] ] = 0.01;
                }
            }
            _tempAbilities.Add( ability );
        }

        //FILTER CONFLICTING ABILITIES
        List<AbilityJSON> _deleteAbilities = new List<AbilityJSON>();
        for( int i = 0; i < _tempAbilities.Count; i++ )
        {
            ability = _tempAbilities[ i ];
            for( int j = 0; j < ability.Increases.Count; j++ )
            {
                if( ability.Increases[ j ] != 0 && ability.Decreases[ j ] != 0 )
                    _deleteAbilities.Add( ability );

            }
        }
        for( int i = 0; i < _deleteAbilities.Count; i++ )
        {
            _tempAbilities.Remove( _deleteAbilities[ i ] );
        }
        for( int i = 0; i < _tempAbilities.Count; i++ )
        {
            _tempAbilities[ i ].Name = "Ability " + i;
            _tempAbilities[ i ].Index = i;
            int price = 0;
            for( int j = 0; j < _tempAbilities[i].Increases.Count; j++ )
            {
                if( _tempAbilities[ i ].Increases[ j ] != 0 )
                {
                    _tempAbilities[ i ].Effects[ j ] = _tempAbilities[ i ].Increases[ j ];
                    price++;
                }
                if( _tempAbilities[ i ].Decreases[ j ] != 0 )
                {
                    _tempAbilities[ i ].Effects[ j ] = _tempAbilities[ i ].Decreases[ j ];
                    price++;
                }
            }
            _tempAbilities[ i ].Increases = new List<double>();
            _tempAbilities[ i ].Decreases = new List<double>();
            _tempAbilities[ i ].UnlockCost = price*10;
        }

        _tempAbilities.Sort( Comparison );

        for( int i = 0; i < _tempAbilities.Count; i++ )
        {
            _tempAbilities[ i ].Name = "Ability " + i;
            _tempAbilities[ i ].Index = i;
        }


        //SAVE TO FILE
        StringBuilder sb = new StringBuilder();
        JsonWriter jsonWriter = new JsonWriter( sb );
        jsonWriter.PrettyPrint = true;
        JsonMapper.ToJson( _tempAbilities, jsonWriter );
        File.WriteAllText( Application.persistentDataPath + "-Abilities.json", sb.ToString() );

        Debug.Log( "DONE Generating Abilities" );
    }

    private int Comparison( AbilityJSON x, AbilityJSON y )
    {
        if( x.UnlockCost > y.UnlockCost )
            return 1;
        if( x.UnlockCost == y.UnlockCost )
            return 0;
        if( x.UnlockCost < y.UnlockCost )
            return -1;

        return 0;
    }
}
