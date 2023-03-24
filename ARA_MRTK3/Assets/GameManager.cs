using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gamemanagerInstance;
    public static GameManager Instance { get { return gamemanagerInstance; } }
    
    private void Awake()
    {
        if (gamemanagerInstance != null && gamemanagerInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gamemanagerInstance = this;
        }
    }

    public void Shutdown()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
