using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startScene, continueScene;
    public GameObject continueButton;

    void start()
    {
        
    }
    void Update()
    {
        if(PlayerPrefs.HasKey(startScene + "_unlocked"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
        PlayerPrefs.DeleteAll();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ContinueGame()
    {
        SceneManager.LoadScene(continueScene);
    }
}
