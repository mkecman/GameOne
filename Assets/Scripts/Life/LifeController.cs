﻿using PsiPhi;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : AbstractController, IGameInit
{
    public LifeModel SelectedLife { get { return _selectedLife; } }

    private PlanetModel _planet;
    private LifeModel _selectedLife;
    private GameDebug _debug;
    private Dictionary<R, BellCurve> _bellCurves;
    private UnitFactory _factory;
    private List<ElementData> _elements;
    private CompoundControlMessage _compoundMessage = new CompoundControlMessage( 0, CompoundControlAction.ADD, false );

    public void Init()
    {
        _debug = GameModel.Get<GameDebug>();
        _bellCurves = GameConfig.Get<BellCurveConfig>();
        _factory = GameModel.Get<UnitFactory>();
        _elements = GameConfig.Get<ElementConfig>().ElementsList;
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _planet = value;
        _selectedLife = _planet.Life;
        //if( _selectedLife.Resistance.Count > 0 )
          //  UpdatePlanetMapColors();
    }

    public void New()
    {
        _selectedLife.Name = "Human";

        _selectedLife.Props[ R.Energy ].Value = 500;
        _selectedLife.Props[ R.Science ].Value = 500;
        _selectedLife.Props[ R.Minerals ].Value = 500;
        _selectedLife.Props[ R.Population ].Value = 1;
        _selectedLife.BuildingsState = GameModel.Copy( GameConfig.Get<BuildingConfig>().Buildings );
        _selectedLife.Resistance = GameModel.Copy( _bellCurves );

        int startingElementAmount = 0;
        if( _debug.isActive )
            startingElementAmount = 300;

        for( int i = 0; i < _elements.Count; i++ )
        {
            _selectedLife.Elements.Add( _elements[ i ].Index, new LifeElementModel( _elements[ i ].Index, _elements[ i ].Symbol, startingElementAmount, 300 ) );
        }

        for( int i = 0; i < 5; i++ )
            AddCompound( 1 );

        for( int i = 0; i < 50; i++ )
            AddCompound( 2 );

        for( int i = 0; i < 5; i++ )
            AddCompound( 3 );


        int unitX = ( _planet.Map.Width / 2 );
        int unitY = ( _planet.Map.Height / 2 );
        _selectedLife.Units.Add( _factory.GetUnitType( 0, unitX, unitY ) );

        if( _debug.isActive )
            _selectedLife.Units[ 0 ].Props[ R.UpgradePoint ].Value = 100;

        _planet.Life = _selectedLife;
        //UpdatePlanetMapColors();
    }

    private void AddCompound( int index )
    {
        _compoundMessage.Index = index;
        GameMessage.Send( _compoundMessage );
    }

    
}
