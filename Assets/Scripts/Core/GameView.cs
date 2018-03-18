using UnityEngine;
using System.Collections;
using UniRx;

public class GameView : MonoBehaviour
{
    internal CompositeDisposable disposables = new CompositeDisposable();

    public virtual void OnDestroy()
    {
        disposables.Dispose();
        disposables = null;
    }

}
