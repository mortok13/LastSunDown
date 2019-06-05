using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private static GameObject menuPanel = GameObject.Find("PauseMenu");

    void Awake()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public void Pause()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "City")
        {
            RunStats.Reset();
        }
    }
}
