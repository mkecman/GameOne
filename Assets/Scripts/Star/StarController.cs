using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class StarController : AbstractController
{
    public StarModel SelectedStar { get { return _selectedStar; } }

    private ReactiveCollection<StarModel> _stars;
    private StarModel _selectedStar;

    private StarsConfig _starsConfig;
    private UniverseConfig _universeConfig;
    private ElementConfig _elementsConfig;

    public StarController()
    {
        _starsConfig = Config.Get<StarsConfig>();
        _universeConfig = Config.Get<UniverseConfig>();
        _elementsConfig = Config.Get<ElementConfig>();
    }

    internal void Load( ReactiveCollection<StarModel> stars )
    {
        _stars = stars;
    }

    internal void New( int type, int index )
    {
        _selectedStar = GameModel.Copy( _starsConfig.Stars[ type ] );
        ConvertUnitsToSI();
        _selectedStar.Name = "Star" + index;
        _selectedStar._AvailableElements = GenerateStarElements( index );
        _selectedStar.PlanetsCount = RandomUtil.FromRangeInt( _starsConfig.MinPlanets, _starsConfig.MaxPlanets );
        _selectedStar.PlanetsCount = 1;
        _stars.Add( _selectedStar );
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

        double curve = index * ( ElementCount * _starsConfig.MaxElementsBellCurveMagnifier / _starsConfig.Stars.Count );
        double ofset = index * ( ElementCount / _starsConfig.Stars.Count );

        ReactiveCollection<WeightedValue> output = new ReactiveCollection<WeightedValue>();
        double probability;
        for( int i = 0; i < ElementCount; i++ )
        {
            probability = ( 1 / Math.Sqrt( 2 * Math.PI * curve ) ) * Math.Exp( -Math.Pow( ofset - i, 2 ) / ( 2 * curve ) );
            if( probability >= .01 )
            {
                WeightedValue element = new WeightedValue
                {
                    Value = i + 1,
                    Weight = probability
                };
                output.Add( element );
            }
        }

        return output;
    }
}
