using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class Log
{
    private StringBuilder _sb;

    public Log()
    {
        _sb = new StringBuilder();
    }

    public static void Add( string text, bool newLine = false )
    {
        Instance._sb.Append( text );
        if( newLine )
            NewLine();
    }

    public static void NewLine()
    {
        Instance._sb.AppendLine();
    }

    public static void Out()
    {
        Debug.Log( Instance._sb );
        Instance._sb.Clear();
    }

    public static void Save( string filename )
    {
        File.WriteAllText( Application.persistentDataPath + filename, Instance._sb.ToString() );
        Instance._sb.Clear();
    }

    private static Log instance;
    public static Log Instance
    {
        get
        {
            if( instance == null )
                instance = new Log();
            return instance;
        }
    }
    
}
