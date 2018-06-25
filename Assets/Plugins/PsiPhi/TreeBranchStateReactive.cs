using System;
using UniRx;

namespace PsiPhi
{
    [Serializable]
    public enum TreeBranchState
    {
        LOCKED,
        UNLOCKED,
        AVAILABLE,
        ACTIVE
    }

    [Serializable]
    public class TreeBranchStateReactiveProperty : ReactiveProperty<TreeBranchState>
    {
        public TreeBranchStateReactiveProperty()
            : base()
        {

        }
    }
}