using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/* 
 * control sets
 */

public class Controls : MonoBehaviour
{
    private int chosenSide;
    private static GameObject rotButtons;
    private static Text resumeText;
    private static GameObject menuPanel;
    public static bool forward, back, rotLeft, rotRight;
    void Start()
    {
        chosenSide = -2;
        rotButtons = GameObject.Find("rotationButtons");
        if(rotButtons != null)
        {
            rotButtons.SetActive(false);
        }
        forward = back = rotLeft = rotRight = false; 
        menuPanel = GameObject.Find("PauseMenu");
        if(menuPanel != null)
        {
            menuPanel.SetActive(true);
            RunStats.Reset();
            menuPanel.SetActive(false);
        }
        if(GameObject.Find("ResumeTextBox") != null)
        {
            resumeText = GameObject.Find("ResumeTextBox").GetComponent<Text>();
            if(resumeText != null)
            {
                resumeText.text = "";
            }
        }
    }
    public void SwitchSingleRotButtons(GameObject trigger)
    {
        Debug.Log(trigger.name);
        if(trigger.name == "rotTriggersOn")
        {
            rotButtons.SetActive(true);
        }
        else
        {
            rotButtons.SetActive(false);
            switch(chosenSide)
            {
                case -2:
                trigger.transform.parent.gameObject.GetComponent<PlayerTrackRotation>().trigger.enabled = false;
                break;
                case -1:
                trigger.transform.parent.tag = "rotRight";
                break;
                case 1:
                trigger.transform.parent.tag = "rotLeft";
                break;
            }
            chosenSide = -2;
        }
    }
    public void SwitchRotButtons(GameObject trigger)
    {
        Debug.Log(trigger.name);
        if(trigger.name == "rotTriggersOn")
        {
            rotButtons.SetActive(true);
        }
        else
        {
            rotButtons.SetActive(false);
            GameObject.Find("Track").GetComponent<trackController>().SetRotationMode(chosenSide, trigger.transform.parent.gameObject);
            chosenSide = -2;
        }
    }
    public void Reset()
    {
        Start();
    }
    public void Pause(bool endGame = false)
    {
        if(endGame)
        {
            menuPanel.SetActive(true);
            GameObject.Find("Pause").GetComponent<Text>().text = "Game Over";
            // ???
            GameObject.Find("PauseButton").SetActive(false);
            return;
        }
        menuPanel.SetActive(!menuPanel.activeSelf);
        if(menuPanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            StartCoroutine("Resume");
        }
    }
    private IEnumerator Resume()
    {
        resumeText.text = "3";
        yield return new WaitForSecondsRealtime(1);
        resumeText.text = "2";
        yield return new WaitForSecondsRealtime(1);
        resumeText.text = "1";
        yield return new WaitForSecondsRealtime(1);
        resumeText.text = "Go!";
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(0.5f);
        resumeText.text = "";
        StopCoroutine("Resume");        
    }

    public void Restart()
    {
        resumeText.text = "";
        StopAllCoroutines();
        GameObject.Find("Pause").GetComponent<Text>().text = "Pause";
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        Time.timeScale = 1;
        //DontDestroyOnLoad(GameObject.Find("Canvas"));
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("mainMenu");
        Time.timeScale = 1;
    }
    public void ForceForward()
    {
        forward = true;
    }

    public void ForceBack()
    {
        back = true;
    }

    public void ForceRotLeft()
    {
        rotLeft = true;
    }

    public void ForceRotRight()
    {
        rotRight = true;
    }


    
    public void UnforceForward()
    {
        forward = false;
    }

    public void UnforceBack()
    {
        back = false;
    }

    public void UnforceRotLeft()
    {
        rotLeft = false;
    }

    public void UnforceRotRight()
    {
        rotRight = false;
    }

    public void SetChosenSide(int sideMode)
    {
        chosenSide = sideMode;
    }
}
