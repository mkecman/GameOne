using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    private List<R> propertyMap = new List<R>() { R.Default, R.Temperature, R.Pressure, R.Humidity, R.Radiation, R.Temperature, R.Pressure, R.Humidity, R.Radiation };
    private RDictionary<BuildingPrice> _prices = new RDictionary<BuildingPrice>();


    void Start()
    {
        _prices.Add( R.Temperature, new BuildingPrice( 100, 200 ) );
        _prices.Add( R.Pressure, new BuildingPrice( 150, 70 ) );
        _prices.Add( R.Humidity, new BuildingPrice( 80, 160 ) );
        _prices.Add( R.Radiation, new BuildingPrice( 120, 60 ) );

        CSV.Add( "Temperature+,Pressure+,Humidity+,Radiation+,Temperature-,Pressure-,Humidity-,Radiation-" );
        GetCombination( new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 } );
    }

    public void GetCombination( List<int> list )
    {
        //MAKE COMBINATIONS
        List<BuildingJSON> buildings = new List<BuildingJSON>();
        double count = Math.Pow( 2, list.Count );
        BuildingJSON building;
        for( int i = 1; i <= count - 1; i++ )
        {
            string str = Convert.ToString( i, 2 ).PadLeft( list.Count, '0' );
            building = new BuildingJSON();
            for( int j = 0; j < str.Length; j++ )
            {
                if( str[ j ] == '1' )
                {
                    if( list[ j ] > 4 )
                    {
                        building.Decreases[ (int)propertyMap[ list[ j ] ] ] = -0.01;
                    }
                    else
                        building.Increases[ (int)propertyMap[ list[ j ] ] ] = 0.01;
                }
            }


            buildings.Add( building );
        }

        //FILTER CONFLICTING BUILDINGS
        List<BuildingJSON> deleteList = new List<BuildingJSON>();
        for( int i = 0; i < buildings.Count; i++ )
        {
            building = buildings[ i ];
            for( int j = 0; j < building.Increases.Count; j++ )
            {
                if( building.Increases[ j ] != 0 && building.Decreases[ j ] != 0 )
                    deleteList.Add( building );

            }
        }
        for( int i = 0; i < deleteList.Count; i++ )
        {
            buildings.Remove( deleteList[ i ] );
        }

        //SET PRICE
        for( int i = 0; i < buildings.Count; i++ )
        {
            int price = 0;
            for( int j = 0; j < buildings[ i ].Increases.Count; j++ )
            {
                if( buildings[ i ].Increases[ j ] != 0 )
                {
                    buildings[ i ].Effects[ j ] = buildings[ i ].Increases[ j ];
                    price += GetPrice( j, true );
                }
                if( buildings[ i ].Decreases[ j ] != 0 )
                {
                    buildings[ i ].Effects[ j ] = buildings[ i ].Decreases[ j ];
                    price += GetPrice( j, false );
                }
            }
            buildings[ i ].Increases = new List<double>();
            buildings[ i ].Decreases = new List<double>();
            buildings[ i ].UnlockCost = price;
            buildings[ i ].BuildCost = price / 2;
            buildings[ i ].Effects[ (int)R.Minerals ] = -1 * ( (double)price / 100 );
        }

        //SORT BY PRICE
        buildings.Sort( Comparison );
        for( int i = 0; i < buildings.Count; i++ )
        {
            buildings[ i ].Name = "Building " + i;
            buildings[ i ].Index = i;
        }


        //SAVE TO FILE
        StringBuilder sb = new StringBuilder();
        JsonWriter jsonWriter = new JsonWriter( sb );
        jsonWriter.PrettyPrint = true;
        JsonMapper.ToJson( buildings, jsonWriter );
        File.WriteAllText( Application.persistentDataPath + "-Buildings.json", sb.ToString() );

        Debug.Log( "DONE Generating Buildings" );
    }

    private int GetPrice( int property, bool increase )
    {
        if( increase )
            return _prices[ (R)property ].Increase;
        else
            return _prices[ (R)property ].Decrease;
    }

    private int Comparison( BuildingJSON x, BuildingJSON y )
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

public class BuildingPrice
{
    public int Increase = 0;
    public int Decrease = 0;

    public BuildingPrice()
    {
    }

    public BuildingPrice( int increase, int decrease )
    {
        Increase = increase;
        Decrease = decrease;
    }
}