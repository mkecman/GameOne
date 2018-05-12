using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class BuildingConfig
{
    public List<BuildingModel> Buildings;

    public void Setup()
    {
        /*
         * Take Effects from BuildingConfig and make a new SkillConfig
         * 
         /**/
        List<SkillData> skills = new List<SkillData>()
        {
            new SkillData { Index = 0,Type = SkillType.LIVE, IsPassive = true },
            new SkillData { Index = 1,Type = SkillType.CLONE, IsPassive = false },
            new SkillData { Index = 2,Type = SkillType.CRAFT, IsPassive = false },
            new SkillData { Index = 3,Type = SkillType.MOVE, IsPassive = false }
        };

        int index = 4;
        
        for( int i = 0; i < Buildings.Count; i++ )
        {
            SkillData skill = new SkillData
            {
                Name = "Mining Skill #" + i,
                Index = index,
                Type = SkillType.MINE,
                State = SkillState.LOCKED,
                IsPassive = true,
                Effects = Buildings[ i ].Effects
            };
            skill.Effects.Remove( R.Minerals );

            if( !skill.Effects.ContainsKey( R.Temperature ) )
                skill.Effects.Add( R.Temperature, 0 );
            if( !skill.Effects.ContainsKey( R.Pressure ) )
                skill.Effects.Add( R.Pressure, 0 );
            if( !skill.Effects.ContainsKey( R.Humidity ) )
                skill.Effects.Add( R.Humidity, 0 );
            if( !skill.Effects.ContainsKey( R.Radiation ) )
                skill.Effects.Add( R.Radiation, 0 );

            skills.Add( skill );
            index++;
        }

        File.WriteAllText(
            Application.persistentDataPath + "SkillConfig.json",
            JsonConvert.SerializeObject( skills )
            );

        Debug.Log( "DONE" );
        /**/

        /*
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
                    building.Effects.Add( (R)j, bJson.Effects[ j ] );
            }
            
            Buildings.Add( building );
        }
        */
    }
}