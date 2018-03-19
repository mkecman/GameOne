using UnityEngine;
using System.Collections;
using System;

public class HexUpdateCommand
{
    private float temperatureBonus;
    private float pressureBonus;
    private float humidityBonus;
    private float radiationBonus;

    public void Execute( RDictionary<BellCurve> Resistance, HexModel hex )
    {
        temperatureBonus = Resistance[ R.Temperature ].GetValueAt( hex.Props[ R.Temperature ].Value );
        pressureBonus = Resistance[ R.Pressure ].GetValueAt( hex.Props[ R.Pressure ].Value );
        humidityBonus = Resistance[ R.Humidity ].GetValueAt( hex.Props[ R.Humidity ].Value );
        radiationBonus = Resistance[ R.Radiation ].GetValueAt( hex.Props[ R.Radiation ].Value );

        hex.Props[ R.Temperature ].Color = Color.Lerp( Color.red, Color.green, temperatureBonus );
        hex.Props[ R.Pressure ].Color = Color.Lerp( Color.red, Color.green, pressureBonus );
        hex.Props[ R.Humidity ].Color = Color.Lerp( Color.red, Color.green, humidityBonus );
        hex.Props[ R.Radiation ].Color = Color.Lerp( Color.red, Color.green, radiationBonus );

        hex.Props[ R.Energy ].Value = Math.Round( ( temperatureBonus + humidityBonus ) * 1.74, 0 ); // * 1.74 for range 0-3
        hex.Props[ R.Energy ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.Energy ].Value / 3 );

        hex.Props[ R.Science ].Value = Math.Round( ( pressureBonus + radiationBonus ) * 1.74, 0 );
        hex.Props[ R.Science ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.Science ].Value / 3 );

        hex.Props[ R.Minerals ].Value = 0;//Math.Round( ( temperatureBonus + radiationBonus ) * 1.74, 0 );
        hex.Props[ R.Minerals ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.Minerals ].Value / 10 );

        hex.Props[ R.HexScore ].Value = Math.Round( ( temperatureBonus + pressureBonus + humidityBonus + radiationBonus ) / 4, 2 );
        hex.Props[ R.HexScore ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.HexScore ].Value );
    }
}
