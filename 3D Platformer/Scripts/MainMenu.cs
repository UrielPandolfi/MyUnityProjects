using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGame, levelSelect;
    public string[] levelsList;
    public GameObject continueButton;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Continue"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGame);

        PlayerPrefs.SetInt("Continue", 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetProgress()
    {
        for(int i=0; i<levelsList.Length; i++)
        {
            PlayerPrefs.SetString(levelsList[i], "locked");
        }
    }
}
