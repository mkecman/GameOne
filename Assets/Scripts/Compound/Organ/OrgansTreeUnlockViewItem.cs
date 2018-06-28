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
    private CompoundConfig _compoundConfig;

    public void Awake()
    {
        _compoundConfig = GameConfig.Get<CompoundConfig>();
    }

    public void Setup( TreeBranchData branchData )
    {
        BranchData = branchData;
        BranchData._State.Subscribe( OnStateChange ).AddTo( disposables );

        _compoundConfig[ BranchData.Index ]._CanCraft.Subscribe( OnCanCraft ).AddTo( disposables );
        _compoundSelectMessage.Index = BranchData.Index;

        NameLabel.text = branchData.Name;
        GetComponent<RectTransform>().anchoredPosition = new Vector2( BranchData.X, BranchData.Y );
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        GameMessage.Send( _compoundSelectMessage );
    }

    private void OnStateChange( TreeBranchState state )
    {
        Background.color = StateColors[ (int)state ];
    }

    private void OnCanCraft( bool canCraft )
    {
        if( canCraft )
        {
            if( BranchData.State == TreeBranchState.UNLOCKED )
                BranchData.State = TreeBranchState.AVAILABLE;
        }
        else
        {
            if( BranchData.State == TreeBranchState.AVAILABLE )
                BranchData.State = TreeBranchState.UNLOCKED;
        }
    }
    
}
