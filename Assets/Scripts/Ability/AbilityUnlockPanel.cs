using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AbilityUnlockPanel : GameView
{
    public Transform Container;
    public GameObject AbilityUnlockPrefab;
    public GameObject EffectPrefab;

    private List<AbilityData> _abilitiesConfig;
    private Dictionary<int, AbilityUnlockView> _abilitiesViews;
    private UnitModel _selectedUnit;

    // Use this for initialization
    void Start()
    {
        _abilitiesConfig = Config.Get<AbilityConfig>().Abilities;
        _abilitiesViews = new Dictionary<int, AbilityUnlockView>();

        for( int i = 0; i < _abilitiesConfig.Count; i++ )
        {
            GameObject ability_go = Instantiate( AbilityUnlockPrefab, Container );
            AbilityUnlockView abilityUnlockView = ability_go.GetComponent<AbilityUnlockView>();
            abilityUnlockView.Setup( _abilitiesConfig[ i ], EffectPrefab );
            _abilitiesViews.Add( _abilitiesConfig[ i ].Index, abilityUnlockView );
        }
    }

    private void OnEnable()
    {
        GameMessage.Send( new CameraControlMessage( false ) );
    }

    private void OnDisable()
    {
        GameMessage.Send( new CameraControlMessage( true ) );
    }

}
