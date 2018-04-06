using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class StarController : AbstractController, IGameInit
{
    public StarModel SelectedStar { get { return _selectedStar; } }

    private ReactiveCollection<StarModel> _stars;
    private StarModel _selectedStar;
    private GalaxyModel _galaxy;

    private StarsConfig _starsConfig;
    private UniverseConfig _universeConfig;
    private ElementConfig _elementsConfig;

    public void Init()
    {
        _starsConfig = Config.Get<StarsConfig>();
        _universeConfig = Config.Get<UniverseConfig>();
        _elementsConfig = Config.Get<ElementConfig>();
        GameModel.HandleGet<GalaxyModel>( OnGalaxyChange );
    }

    private void OnGalaxyChange( GalaxyModel value )
    {
        _galaxy = value;
        _stars = value._Stars;
    }
    
    internal void Load( int index )
    {
        _selectedStar = _stars[ index ];
        GameModel.Set<StarModel>( _selectedStar );
    }

    internal void New( int type )
    {
        _selectedStar = GameModel.Copy( _starsConfig.Stars[ type ] );
        ConvertUnitsToSI();
        _selectedStar.Name = "Star" + _galaxy.CreatedStars;
        _selectedStar._AvailableElements = GenerateStarElements( _galaxy.CreatedStars );
        _selectedStar.PlanetsCount = RandomUtil.FromRangeInt( _starsConfig.MinPlanets, _starsConfig.MaxPlanets );
        _selectedStar.PlanetsCount = 1;
        _stars.Add( _selectedStar );

        _galaxy.CreatedStars++;
        GameModel.Set<StarModel>( _selectedStar );
    }

    private void ConvertUnitsToSI()
    {
        _selectedStar.Mass *= _universeConfig.SunMassInKilograms;
        _selectedStar.Radius *= _universeConfig.SunRadiusInMeters;
        _selectedStar.HabitableZone *= _universeConfig.AUInMeters;
        _selectedStar.InnerHabitableZone *= _universeConfig.AUInMeters;
        _selectedStar.OuterHabitableZone *= _universeConfig.AUInMeters;
        _selectedStar.Luminosity *= _universeConfig.SunLuminosityInWatts;
    }

    private ReactiveCollection<WeightedValue> GenerateStarElements( int index )
    {
        int ElementCount = _elementsConfig.Elements.Count;

        float curve = index * ( ElementCount * _starsConfig.MaxElementsBellCurveMagnifier / _starsConfig.Stars.Count );
        float ofset = index * ( ElementCount / _starsConfig.Stars.Count );

        ReactiveCollection<WeightedValue> output = new ReactiveCollection<WeightedValue>();
        float probability;
        for( int i = 0; i < ElementCount; i++ )
        {
            probability = ( 1 / Mathf.Sqrt( 2 * Mathf.PI * curve ) ) * Mathf.Exp( -Mathf.Pow( ofset - i, 2 ) / ( 2 * curve ) );
            if( probability >= .01 )
            {
                WeightedValue element = new WeightedValue( i + 1, probability );
                output.Add( element );
            }
        }

        return output;
    }
}
