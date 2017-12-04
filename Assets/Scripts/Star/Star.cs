using UnityEngine;
using System.Collections;

public class Star
{
    private StarModel _star;
    
    public void CreateStar( double Words, int Index )
    {
        _star = new StarModel();
        _star.Name = "Star " + Index;
    }
}
