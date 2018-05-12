using TMPro;

public class PlanetChangeTableHeaderView : GameView
{
    public TextMeshProUGUI TemperatureText;
    public TextMeshProUGUI PressureText;
    public TextMeshProUGUI HumidityText;
    public TextMeshProUGUI RadiationText;

    public void SetupText( string temperature, string pressure, string humidity, string radiation )
    {
        TemperatureText.text = temperature;
        PressureText.text = pressure;
        HumidityText.text = humidity;
        RadiationText.text = radiation;
    }


}
