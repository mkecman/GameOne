using System;
using System.Collections.Generic;

[Serializable]
public class BuildingConfig
{
    public List<BuildingJSON> BuildingList;

    public List<BuildingModel> Buildings;

    public void Setup()
    {
        Buildings = new List<BuildingModel>();
        BuildingJSON bJson;
        BuildingModel building;

        for( int i = 0; i < BuildingList.Count; i++ )
        {
            bJson = BuildingList[ i ];
            building = new BuildingModel
            {
                Index = bJson.Index,
                Name = bJson.Name,
                UnlockCost = bJson.UnlockCost,
                BuildCost = bJson.BuildCost
            };

            for( int j = 0; j < bJson.Effects.Count; j++ )
            {
                if( bJson.Effects[ j ] != 0 )
                    building.Effects.Add( ((R)j).ToString(), bJson.Effects[ j ] );
            }
            
            Buildings.Add( building );
        }
    }
}