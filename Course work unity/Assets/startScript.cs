using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class startScript : MonoBehaviour
{
    public static byte gameMode;
    void Start()
    {
        GetComponent<moving>().enabled = true;
        GetComponent<speedControl>().enabled = true;
        GetComponent<controls>().enabled = true;
        switch(SceneManager.GetActiveScene().name)
        {
            case "mainMenu":
            gameMode = 0;
            GetComponent<MenuControl>().enabled = true;
            Debug.Log("mainmenu");
            break;

            default:
            gameMode = 1;
          //  GetComponent<controls>().enabled = true;
            GetComponent<stats>().enabled = true;
           // GetComponent<speedControl>().enabled = true;
            break;
        }
    }
}
