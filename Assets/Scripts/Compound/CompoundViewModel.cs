using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CompoundViewModel
{
    private CompoundJSON _compound;
    private CompositeDisposable disposables = new CompositeDisposable();

    public CompoundViewModel( CompoundJSON compound )
    {
        _compound = compound;
    }

    public void Setup( ReactiveDictionary<int, LifeElementModel> elements )
    {
        disposables.Clear();

        for( int i = 0; i < _compound.Elements.Count; i++ )
        {
            AddListener( _compound.Elements[ i ], elements );
        }
    }

    private void AddListener( LifeElementModel compoundElement, ReactiveDictionary<int, LifeElementModel> elements )
    {
        //set compound element amount when life's element amount change
        elements[ compoundElement.Index ]._Amount
            .Subscribe( _ => compoundElement.Amount = _ )
            .AddTo( disposables );

        //when element is full, check if all elements in the compound are full too
        compoundElement._IsFull
            .Subscribe( _ => CheckCanCraft() )
            .AddTo( disposables );
    }

    private void CheckCanCraft()
    {
        for( int i = 0; i < _compound.Elements.Count; i++ )
        {
            if( !_compound.Elements[ i ].IsFull )
            {
                _compound.CanCraft = false;
                return;
            }
        }
        _compound.CanCraft = true;
    }

    internal void Disable()
    {
        disposables.Clear();
    }
}
