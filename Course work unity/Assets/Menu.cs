using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void loadLocationCity()
    {
        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
