using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using PsiPhi;
using UniRx;

public class OrgansTreeUnlockViewConnection : GameView
{
    public GameObject Parent;
    public GameObject Source;
    public RawImage Background;
    private RectTransform _rectTransform;
    private Vector3 _differenceVector;
    private Vector3 _parentPosition;

    public void Update()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        if( Parent != null )
        {
            gameObject.SetActive( true );
            _differenceVector = Parent.transform.position - Source.transform.position;
            _rectTransform.sizeDelta = new Vector2( 20f, _differenceVector.magnitude );
            _rectTransform.rotation = Quaternion.Euler( 0, 0, -Mathf.Atan2( _differenceVector.x, _differenceVector.y ) * Mathf.Rad2Deg );
            _rectTransform.position = Source.transform.position;
        }
        else
            gameObject.SetActive( false );
    }

    internal void Setup( TreeBranchData branch, GameObject source, GameObject parent )
    {
        Parent = parent;
        Source = source;

        branch._State.Subscribe( OnStateChange ).AddTo( disposables );
        Update();
    }

    private void OnStateChange( TreeBranchState state )
    {
        switch( state )
        {
            case TreeBranchState.LOCKED:
                Background.color = new Color32( 100, 100, 100, 255 );
                break;
            case TreeBranchState.UNLOCKED:
                Background.color = new Color32( 255, 141, 141, 255 );
                break;
            case TreeBranchState.AVAILABLE:
                Background.color = new Color32( 141, 255, 156, 255 );
                break;
            case TreeBranchState.ACTIVE:
                Background.color = new Color32( 141, 255, 156, 255 );
                break;
            default:
                Background.color = new Color32( 100, 100, 100, 255 );
                break;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Parent = null;
        Source = null;
        _rectTransform = null;
        Background = null;
    }
}
