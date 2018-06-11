using UnityEngine.UI;

public class ResistanceGraphStatic : GameView
{
    public R Lens;
    public GradientTextureView Gradient;
    public Text PropertyText;
    public Text MatchText;
    private BellCurve _bellCurve = new BellCurve();

    void Start()
    {
        PropertyText.text = Lens.ToString();
    }

    public void SetPosition( float position )
    {
        _bellCurve.Position.Value = position;
        Gradient.Draw( _bellCurve );
        MatchText.text = _bellCurve.Position.ToString();
    }
}
