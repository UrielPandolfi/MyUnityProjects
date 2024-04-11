using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
