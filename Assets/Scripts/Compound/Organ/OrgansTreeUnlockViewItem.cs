using PsiPhi;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrgansTreeUnlockViewItem : GameView, IPointerClickHandler
{
    public TextMeshProUGUI NameLabel;
    public Image Background;
    public Color[] StateColors;
    public TreeBranchData BranchData;

    private CompoundSelectMessage _compoundSelectMessage = new CompoundSelectMessage();

    public void Setup( TreeBranchData branchData )
    {
        BranchData = branchData;
        BranchData._State.Subscribe( OnStateChange ).AddTo( disposables );
        NameLabel.text = branchData.Name;

        GetComponent<RectTransform>().anchoredPosition = new Vector2( BranchData.X, BranchData.Y );
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        _compoundSelectMessage.Index = BranchData.Index;
        _compoundSelectMessage.State = BranchData.State;
        GameMessage.Send( _compoundSelectMessage );
    }

    private void OnStateChange( TreeBranchState state )
    {
        Background.color = StateColors[ (int)state ];
    }

    
}
