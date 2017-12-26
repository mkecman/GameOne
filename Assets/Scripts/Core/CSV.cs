using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class CSV
{
    private StringBuilder _sb;

    public CSV()
    {
        _sb = new StringBuilder();
    }

    public static void Add( string text, bool newLine = true )
    {
        Instance._sb.Append( text );
        if( newLine )
            NewLine();
    }

    public static void NewLine()
    {
        Instance._sb.AppendLine();
    }

    public static void Out( bool clear = false )
    {
        Debug.Log( Instance._sb );
        if( clear )
            Instance._sb.Clear();
    }

    public static void Save( string filename )
    {
        File.WriteAllText( Application.persistentDataPath + filename, Instance._sb.ToString() );
        Instance._sb.Clear();
    }

    private static CSV instance;
    public static CSV Instance
    {
        get
        {
            if( instance == null )
                instance = new CSV();
            return instance;
        }
    }
    
}
