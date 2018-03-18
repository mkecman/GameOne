using UnityEngine;
using System.Collections;
using System;

public class HexUpdateCommand : ICommand
{
    public void Execute( object[] data )
    {
        RDictionary<BellCurve> Resistance = data[ 0 ] as RDictionary<BellCurve>;
        HexModel hex = data[ 1 ] as HexModel;

        float temperatureBonus = Resistance[ R.Temperature ].GetValueAt( hex.Props[ R.Temperature ].Value );
        float pressureBonus = Resistance[ R.Pressure ].GetValueAt( hex.Props[ R.Pressure ].Value );
        float humidityBonus = Resistance[ R.Humidity ].GetValueAt( hex.Props[ R.Humidity ].Value );
        float radiationBonus = Resistance[ R.Radiation ].GetValueAt( hex.Props[ R.Radiation ].Value );

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

        //hex.TotalScore = Math.Round( ( temperatureBonus + pressureBonus + humidityBonus + radiationBonus ) / 4, 2 );
        hex.Props[ R.HexScore ].Value = Math.Round( ( temperatureBonus + pressureBonus + humidityBonus + radiationBonus ) / 4, 2 );
        hex.Props[ R.HexScore ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.HexScore ].Value );
    }
}
