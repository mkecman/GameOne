using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor( typeof( CoreSimulator ) )]
public class CoreSimulatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CoreSimulator simulator = (CoreSimulator)target;
        
        if( GUILayout.Button( "Run Preset" ) )
            simulator.Run( (int)simulator.TimePreset );

        if( GUILayout.Button( "Run TimeDelta" ) )
            simulator.Run( (int)simulator.TimeDelta );

    }
}