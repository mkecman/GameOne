using System;
using System.Collections.Generic;

[Serializable]
public class AbilityConfig
{
    public List<AbilityJSON> AbilitiesJSON;

    public List<Ability> Abilities;

    public void Setup()
    {
        AbilityJSON abilityJSON;
        Ability ability;
        for( int i = 0; i < AbilitiesJSON.Count; i++ )
        {
            abilityJSON = AbilitiesJSON[ i ];
            ability = new Ability
            {
                Index = abilityJSON.Index,
                Name = abilityJSON.Name,
                UnlockCost = abilityJSON.UnlockCost
            };

            for( int j = 0; j < abilityJSON.Increases.Count; j++ )
            {
                ability.Increases.Set( (R)j, abilityJSON.Increases[ j ] );
            }
            for( int j = 0; j < abilityJSON.Decreases.Count; j++ )
            {
                ability.Decreases.Set( (R)j, abilityJSON.Decreases[ j ] );
            }
        }
    }
}