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
         /**
        Dictionary<SkillType, List<SkillData>> skills = new Dictionary<SkillType, List<SkillData>>
        {
            { SkillType.Live, new List<SkillData> { new SkillData { Type = SkillType.Live } } },
            { SkillType.Clone, new List<SkillData> { new SkillData { Type = SkillType.Clone } } },
            { SkillType.Craft, new List<SkillData> { new SkillData { Type = SkillType.Craft } } },
            { SkillType.Move, new List<SkillData> { new SkillData { Type = SkillType.Move } } },
            { SkillType.Mine, new List<SkillData>() }
        };
        
        for( int i = 0; i < Buildings.Count; i++ )
        {
            SkillData skill = new SkillData
            {
                Name = "Mining Skill #" + i,
                Type = SkillType.Mine,
                Effects = Buildings[ i ].Effects
            };
            skill.Effects.Remove( R.Minerals );
            skills[ SkillType.Mine ].Add( skill );
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