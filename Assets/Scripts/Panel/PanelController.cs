using System.Collections.Generic;
using UnityEngine;

public class PanelController : GameView
{
    public bool ShouldOutputPanelNames;
    public Transform Container;
    public GameObject[] Panels;

    private Dictionary<string, GameObject> PanelsDictionary;

    public void Awake()
    {
        if( ShouldOutputPanelNames )
            OutputPanelNames();

        PanelsDictionary = new Dictionary<string, GameObject>();
        for( int i = 0; i < Panels.Length; i++ )
        {
            PanelsDictionary.Add( Panels[ i ].name, Panels[ i ] );
        }

        GameMessage.Listen<PanelMessage>( OnPanelMessage );
    }

    private void OutputPanelNames()
    {
        string output = "";
        for( int i = 0; i < Panels.Length; i++ )
        {
            output += "public static string " + Panels[ i ].name + " = \"" + Panels[ i ].name + "\";\n";
        }

        Debug.Log( output );
    }

    private void OnPanelMessage( PanelMessage value )
    {
        switch( value.Action )
        {
            case PanelAction.SHOW:
                ShowPanel( value.PanelName );
                break;
            case PanelAction.SHOWONLY:
                ShowPanel( value.PanelName, true );
                break;
            case PanelAction.HIDE:
                HidePanel( value.PanelName );
                break;
            case PanelAction.HIDEALL:
                HideAllPanels();
                break;
            default:
                break;
        }
    }

    private void HideAllPanels()
    {
        foreach( KeyValuePair<string, GameObject> panel in PanelsDictionary )
        {
            panel.Value.SetActive( false );
        }
    }

    private void HidePanel( string panelName )
    {
        PanelsDictionary[ panelName ].SetActive( false );
    }

    private void ShowPanel( string panelName, bool showOnly = false )
    {
        if( showOnly )
            HideAllPanels();

        PanelsDictionary[ panelName ].SetActive( true );
    }
}

public class PanelNames
{
    public static string HexTilePanel = "HexTilePanel";
    public static string ResistancePanel = "ResistancePanel";
    public static string MainMenuPanel = "MainMenuPanel";
    public static string BuildingInfoPanel = "BuildingInfoPanel";
    public static string BuildUnlockPanel = "BuildUnlockPanel";
    public static string ElementsPanel = "ElementsPanel";
    public static string UnitPassiveSkillsPanel = "UnitPassiveSkillsPanel";
    public static string SkillsPanel = "SkillsPanel";
    public static string CompoundsPanel = "CompoundsPanel";
    public static string UnitEditPanel = "UnitEditPanel";
    public static string CompoundInventoryPanel = "CompoundInventoryPanel";
    public static string CompoundTooltipPanel = "CompoundTooltipPanel";
    public static string PlanetChangePanel = "PlanetChangePanel";
    public static string NewUnitPanel = "NewUnitPanel";
}
