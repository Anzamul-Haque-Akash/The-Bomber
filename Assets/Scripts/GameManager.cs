using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool PlayerDeath;


    //Getter and Setter method for Player detah.
    public void setPlayerDeath(bool condition)
    {
        PlayerDeath = true;
    }
    public bool getPlayerDeath()
    {
        return PlayerDeath;
    }

}//Game Manager Class
