using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        ElementConfig elementConfig = new ElementConfig();
        elementConfig.Load();

        StarConfig starConfig = new StarConfig();
        starConfig.Load();
    }
    
}
