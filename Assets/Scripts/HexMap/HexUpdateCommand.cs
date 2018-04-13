using PsiPhi;
using System.Collections.Generic;
using UnityEngine;

public class HexUpdateCommand : IGameInit
{
    private float temperatureBonus;
    private float pressureBonus;
    private float humidityBonus;
    private float radiationBonus;

    private Resource temperature;
    private Resource humidity;
    private Resource pressure;
    private Resource radiation;
    private Resource energy;
    private Resource science;
    private Resource minerals;
    private Resource hexScore;
    private List<ElementModel> _elements;

    public void Init()
    {
        _elements = GameConfig.Get<ElementConfig>().Elements;
    }

    public void Execute( Dictionary<R, BellCurve> Resistance, HexModel hex )
    {
        temperature = hex.Props[ R.Temperature ];
        humidity = hex.Props[ R.Humidity ];
        pressure = hex.Props[ R.Pressure ];
        radiation = hex.Props[ R.Radiation ];

        //energy = hex.Props[ R.Energy ];
        //science = hex.Props[ R.Science ];
        //minerals = hex.Props[ R.Minerals ];
        hexScore = hex.Props[ R.HexScore ];


        temperatureBonus = Resistance[ R.Temperature ].GetFloatAt( temperature.Value );
        pressureBonus = Resistance[ R.Pressure ].GetFloatAt( pressure.Value );
        humidityBonus = Resistance[ R.Humidity ].GetFloatAt( humidity.Value );
        radiationBonus = Resistance[ R.Radiation ].GetFloatAt( radiation.Value );
        /*
        hex.Props[ R.Temperature ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Temperature ].Value );
        hex.Props[ R.Pressure ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Pressure ].Value );
        hex.Props[ R.Humidity ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Humidity ].Value );
        hex.Props[ R.Radiation ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Radiation ].Value );
        /**/
        temperature.Color = Color.Lerp( Color.red, Color.green, temperatureBonus );
        pressure.Color = Color.Lerp( Color.red, Color.green, pressureBonus );
        humidity.Color = Color.Lerp( Color.red, Color.green, humidityBonus );
        radiation.Color = Color.Lerp( Color.red, Color.green, radiationBonus );
        /**/

        //energy.Value = PPMath.Round( ( temperatureBonus + humidityBonus ), 0 ); // * 1.74 for range 0-3
        //energy.Color = Color.Lerp( Color.red, Color.green, energy.Value / 1 );

        //science.Value = PPMath.Round( ( pressureBonus + radiationBonus ), 0 );
        //science.Color = Color.Lerp( Color.red, Color.green, science.Value / 1 );

        hexScore.Value = PPMath.Round( ( temperatureBonus + pressureBonus + humidityBonus + radiationBonus ) / 4, 2 );
        hexScore.Color = Color.Lerp( Color.red, Color.green, hexScore.Value );

        //minerals.Value = _elements[ (int)( ( (1-hexScore.Value) * _elements.Count ) - 1 ) ].Weight;
        //minerals.Value = PPMath.Round( ( ( 1 - Resistance[ R.Altitude ].GetValueAt( hex.Props[ R.Altitude ].Value / 2 ) ) * 2 ), 0 );
        //minerals.Color = Color.Lerp( Color.red, Color.green, minerals.Value / 100 );
    }
}
