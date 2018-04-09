using UnityEngine;
using System.Collections;
using UniRx;

public class GameView : MonoBehaviour
{
    internal CompositeDisposable disposables = new CompositeDisposable();
    private GameObject go;

    public virtual void OnDestroy()
    {
        disposables.Dispose();
        disposables = null;
    }
    
    public void RemoveAllChildren( Transform transform )
    {
        while( transform.childCount != 0 )
        {
            go = transform.GetChild( 0 ).gameObject;
            go.transform.SetParent( null );
            DestroyImmediate( go );
        }
    }

}
