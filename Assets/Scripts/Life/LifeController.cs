using PsiPhi;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : AbstractController, IGameInit
{
    public LifeModel SelectedLife { get { return _selectedLife; } }

    private PlanetModel _planet;
    private LifeModel _selectedLife;
    private Dictionary<R, BellCurve> _bellCurves;
    private HexUpdateCommand _hexUpdateCommand;
    private UnitFactory _factory;
    private List<ElementData> _elements;

    public void Init()
    {
        _bellCurves = GameConfig.Get<BellCurveConfig>();
        _hexUpdateCommand = GameModel.Get<HexUpdateCommand>();
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

        for( int i = 0; i < _elements.Count; i++ )
        {
            _selectedLife.Elements.Add( _elements[ i ].Index, new LifeElementModel( _elements[ i ].Index, _elements[ i ].Symbol, 100, 100 ) );
        }

        for( int i = 0; i < 3; i++ )
            AddCompound( 24 );

        for( int i = 0; i < 20; i++ )
            AddCompound( 25 );

        for( int i = 0; i < 10; i++ )
            AddCompound( 26 );

        AddCompound( 27 );
        AddCompound( 1 );
        AddCompound( 2 );


        int unitX = ( _planet.Map.Width / 2 ) + 2;
        int unitY = ( _planet.Map.Height / 2 ) + 2;
        _selectedLife.Units.Add( _factory.GetUnit( unitX, unitY ) );

        _planet.Life = _selectedLife;
        //UpdatePlanetMapColors();
    }

    private void AddCompound( int index )
    {
        CompoundControlMessage _compoundMessage = new CompoundControlMessage();
        _compoundMessage.Action = CompoundControlAction.ADD;
        _compoundMessage.Index = index;
        _compoundMessage.SpendCurrency = false;
        GameMessage.Send( _compoundMessage );
    }

    private void UpdatePlanetMapColors()
    {
        HexModel hex;

        int[] foodTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        int[] scienceTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        int[] wordsTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        float totalEnergy = 0;
        float totalScience = 0;
        float totalMinerals = 0;
        
        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                hex = _planet.Map.Table[ x ][ y ];

                //_hexUpdateCommand.Execute( _selectedLife.Resistance, hex );

                //totalEnergy += hex.Props[ R.Energy ].Value;
                //totalScience += hex.Props[ R.Science ].Value;
                //totalMinerals += hex.Props[ R.Minerals ].Value;

                //foodTiles[ (int)hex.Props[ R.Energy ].Value ]++;
                //scienceTiles[ (int)hex.Props[ R.Science ].Value ]++;
                //wordsTiles[ (int)hex.Props[ R.Minerals ].Value ]++;

                
            }
        }
        
        //Debug.Log( "Energy: " + totalEnergy + "::: Science: " + totalScience + "::: Minerals: " + totalMinerals );

        for( int i = 0; i < 6; i++ )
        {
            totalEnergy += i * foodTiles[ i ];
            totalScience += i * scienceTiles[ i ];
            totalMinerals += i * wordsTiles[ i ];
        }

    }
}
