using UnityEngine;
using System.Collections;

public class HexMapModel
{
    public GridModel<HexModel> HexMap;

    public GridModel<float> AltitudeMap;
    public GridModel<float> TemperatureMap;
    public GridModel<float> PressureMap;
    public GridModel<float> HumidityMap;
    public GridModel<float> RadiationMap;
    public GridModel<Color> ColorMap;
    public GridModel<ElementData> ElementMap;
    public int Width;
    public int Height;

    public HexMapModel( int width, int height )
    {
        Width = width;
        Height = height;
        HexMap = new GridModel<HexModel>( width, height );
        AltitudeMap = new GridModel<float>( width, height );
        TemperatureMap = new GridModel<float>( width, height );
        PressureMap = new GridModel<float>( width, height );
        HumidityMap = new GridModel<float>( width, height );
        RadiationMap = new GridModel<float>( width, height );
        ColorMap = new GridModel<Color>( width, height );
        ElementMap = new GridModel<ElementData>( width, height );

    }
}
