using System;
using System.Collections.Generic;

[Serializable]
public class AbilityConfig
{
    public List<AbilityJSON> AbilitiesJSON;

    public List<AbilityData> Abilities;

    public void Setup()
    {
        Abilities = new List<AbilityData>();
        AbilityJSON abilityJSON;
        AbilityData ability;

        for( int i = 0; i < AbilitiesJSON.Count; i++ )
        {
            abilityJSON = AbilitiesJSON[ i ];
            ability = new AbilityData
            {
                Index = abilityJSON.Index,
                Name = abilityJSON.Name,
                UnlockCost = abilityJSON.UnlockCost
            };

            for( int j = 0; j < abilityJSON.Effects.Count; j++ )
            {
                if( abilityJSON.Effects[ j ] != 0 )
                    ability.Effects.Add( (R)j, abilityJSON.Effects[ j ] );
            }
            
            Abilities.Add( ability );
        }
    }
}