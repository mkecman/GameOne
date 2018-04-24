using System;
using System.Collections.Generic;

[Serializable]
public class ElementConfig
{
    public List<ElementData> ElementsList;
    public Dictionary<int, ElementData> ElementsDictionary;

    internal void Setup()
    {
        ElementsDictionary = new Dictionary<int, ElementData>();
        for( int i = 0; i < ElementsList.Count; i++ )
        {
            ElementsDictionary.Add( ElementsList[ i ].Index, ElementsList[ i ] );
        }
    }
}

