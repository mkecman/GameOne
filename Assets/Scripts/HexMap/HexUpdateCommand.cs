using PsiPhi;
using System.Collections.Generic;
using UnityEngine;

public class HexUpdateCommand : IGameInit
{
    private float temperatureBonus;
    private float pressureBonus;
    private float humidityBonus;
    private float radiationBonus;

    public void Init() { }

    public void Execute( Dictionary<R, BellCurve> Resistance, HexModel hex )
    {
        temperatureBonus = Resistance[ R.Temperature ].GetValueAt( hex.Props[ R.Temperature ].Value );
        pressureBonus = Resistance[ R.Pressure ].GetValueAt( hex.Props[ R.Pressure ].Value );
        humidityBonus = Resistance[ R.Humidity ].GetValueAt( hex.Props[ R.Humidity ].Value );
        radiationBonus = Resistance[ R.Radiation ].GetValueAt( hex.Props[ R.Radiation ].Value );
        /*
        hex.Props[ R.Temperature ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Temperature ].Value );
        hex.Props[ R.Pressure ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Pressure ].Value );
        hex.Props[ R.Humidity ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Humidity ].Value );
        hex.Props[ R.Radiation ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Radiation ].Value );
        /**/
        hex.Props[ R.Temperature ].Color = Color.Lerp( Color.red, Color.green, temperatureBonus );
        hex.Props[ R.Pressure ].Color = Color.Lerp( Color.red, Color.green, pressureBonus );
        hex.Props[ R.Humidity ].Color = Color.Lerp( Color.red, Color.green, humidityBonus );
        hex.Props[ R.Radiation ].Color = Color.Lerp( Color.red, Color.green, radiationBonus );
        /**/

        hex.Props[ R.Energy ].Value = PPMath.Round( ( temperatureBonus + humidityBonus ), 0 ); // * 1.74 for range 0-3
        hex.Props[ R.Energy ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Energy ].Value / 1 );

        hex.Props[ R.Science ].Value = PPMath.Round( ( pressureBonus + radiationBonus ), 0 );
        hex.Props[ R.Science ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Science ].Value / 1 );

        hex.Props[ R.Minerals ].Value = PPMath.Round( ( ( 1 - Resistance[ R.Altitude ].GetValueAt( hex.Props[ R.Altitude ].Value / 2 ) ) * 2 ), 0 );
        hex.Props[ R.Minerals ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.Minerals ].Value / 1 );

        hex.Props[ R.HexScore ].Value = PPMath.Round( ( temperatureBonus + pressureBonus + humidityBonus + radiationBonus ) / 4, 2 );
        hex.Props[ R.HexScore ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.HexScore ].Value );
    }
}
