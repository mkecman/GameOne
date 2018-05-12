using System;

[Serializable]
public class UniverseConfig
{
    public float G;
    public float SunLuminosityInWatts;
    public float SunMassInKilograms;
    public float SunRadiusInMeters;
    public float AUInMeters;
    public float Boltzmann;
    public float SolarLuminosity;
    public float IntToPlanetValueMultiplier;
    public int PlanetValueToIntMultiplier;
    public string PlanetValueStringFormat;
}
