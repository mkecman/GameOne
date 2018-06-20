using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OrgansTreeUnlockViewItem : GameView
{
    public TextMeshProUGUI NameLabel;
    public Image Background;
    public Color[] StateColors;
    public TreeBranchData BranchData;

    public void Setup( TreeBranchData branchData )
    {
        BranchData = branchData;
        BranchData._State.Subscribe( OnStateChange ).AddTo( disposables );
        NameLabel.text = branchData.Name;

        transform.position = new Vector3( BranchData.X, BranchData.Y );
    }

    private void OnStateChange( TreeBranchState state )
    {
        Background.color = StateColors[ (int)state ];
    }
}
