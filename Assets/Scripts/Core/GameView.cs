using UnityEngine;
using System.Collections;
using UniRx;

public class GameView : MonoBehaviour
{
    internal CompositeDisposable disposables = new CompositeDisposable();

    private void OnDestroy()
    {
        disposables.Dispose();
        disposables = null;
    }

}
