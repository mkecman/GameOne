using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor( typeof( HexMap ) )]
public class HexMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HexMap map = (HexMap)target;
        if( GUILayout.Button( "Redraw" ) )
            map.ReDraw();
            
        DrawDefaultInspector();
    }
}
