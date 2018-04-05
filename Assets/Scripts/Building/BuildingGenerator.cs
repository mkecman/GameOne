using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    private List<R> propertyMap = new List<R>() { R.Default, R.Temperature, R.Pressure, R.Humidity, R.Radiation, R.Temperature, R.Pressure, R.Humidity, R.Radiation };
    private Dictionary<R,BuildingPrice> _prices = new Dictionary<R,BuildingPrice>();


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
        List<BuildingEffect> buildingsEffects = new List<BuildingEffect>();

        int count = (int)Math.Pow( 2, list.Count );
        BuildingEffect buildingEffect;
        for( int i = 1; i <= count - 1; i++ )
        {
            string str = Convert.ToString( i, 2 ).PadLeft( list.Count, '0' );
            buildingEffect = new BuildingEffect();
            for( int j = 0; j < str.Length; j++ )
            {
                if( str[ j ] == '1' )
                {
                    if( list[ j ] > 4 )
                    {
                        buildingEffect.Decreases[ (int)propertyMap[ list[ j ] ] ] = -0.01f;
                    }
                    else
                        buildingEffect.Increases[ (int)propertyMap[ list[ j ] ] ] = 0.01f;
                }
            }


            buildingsEffects.Add( buildingEffect );
        }

        //FILTER CONFLICTING BUILDINGS
        List<BuildingEffect> deleteList = new List<BuildingEffect>();
        for( int i = 0; i < buildingsEffects.Count; i++ )
        {
            buildingEffect = buildingsEffects[ i ];
            for( int j = 0; j < buildingEffect.Increases.Count; j++ )
            {
                if( buildingEffect.Increases[ j ] != 0 && buildingEffect.Decreases[ j ] != 0 )
                    deleteList.Add( buildingEffect );

            }
        }
        for( int i = 0; i < deleteList.Count; i++ )
        {
            buildingsEffects.Remove( deleteList[ i ] );
        }

        //SET PRICE
        List<BuildingModel> buildings = new List<BuildingModel>();
        for( int i = 0; i < buildingsEffects.Count; i++ )
        {
            buildings.Add( new BuildingModel() );

            int price = 0;
            for( int j = 0; j < buildingsEffects[ i ].Increases.Count; j++ )
            {
                if( buildingsEffects[ i ].Increases[ j ] != 0 )
                {
                    buildings[ i ].Effects.Add( (R)j, buildingsEffects[ i ].Increases[ j ] );
                    price += GetPrice( j, true );
                }
                if( buildingsEffects[ i ].Decreases[ j ] != 0 )
                {
                    buildings[ i ].Effects.Add( (R)j, buildingsEffects[ i ].Decreases[ j ] );
                    price += GetPrice( j, false );
                }
            }

            buildings[ i ].UnlockCost = price;
            buildings[ i ].BuildCost = price / 2;
            buildings[ i ].Effects.Add( R.Minerals, -1 * ( price / 100 ) );
            buildings[ i ].State = BuildingState.LOCKED;
        }

        //SORT BY PRICE
        buildings.Sort( Comparison );
        for( int i = 0; i < buildings.Count; i++ )
        {
            buildings[ i ].Name = "Building " + i;
            buildings[ i ].Index = i;
        }


        //SAVE TO FILE
        File.WriteAllText( 
            Application.persistentDataPath + "-Buildings.json",
            JsonConvert.SerializeObject( buildings )
            );

        Debug.Log( "DONE Generating Buildings" );
    }

    private int GetPrice( int property, bool increase )
    {
        if( increase )
            return _prices[ (R)property ].Increase;
        else
            return _prices[ (R)property ].Decrease;
    }

    private int Comparison( BuildingModel x, BuildingModel y )
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

public class BuildingEffect
{
    public List<float> Increases;
    public List<float> Decreases;

    public BuildingEffect()
    {
        Increases = new List<float>();
        Decreases = new List<float>();
        for( int i = 0; i < (int)R.Count; i++ )
        {
            Increases.Add( 0 );
            Decreases.Add( 0 );
        }
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