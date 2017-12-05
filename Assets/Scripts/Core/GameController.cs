using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    // Use this for initialization
    void Start()
    {
        PlayerManager player = new PlayerManager();
        player.CreatePlayer();
        Debug.Log( "GameController Start" );
    }

}
