using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    public int killsCounter;
    public int notesCounter;
    
    void Start()
    {
        killsCounter = 0;
        Cursor.lockState = CursorLockMode.Locked;   // Centrar el cursor en el centro de la pantalla
        Cursor.visible = false;                     // Hacer que no se vea
    }

    void Update()
    {
        if(notesCounter==2)
        {
            WinLevel();
        }
    }

    public void AddKill()
    {
        killsCounter++;
    }

    public void WinLevel()
    {
        // Mensaje de victoria y pasar al siguiente nivel
        StartCoroutine(LevelPassCoroutine());
        Debug.Log("Ganaste el nivel!");
    }
    public void LevelLost()
    {
        StartCoroutine(LevelLostCoroutine());
        Debug.Log("Perdiste el nivel!");
    }

    IEnumerator LevelPassCoroutine()
    {
        UIManager.instance.WinPanel();
        yield return new WaitForSeconds(3f);
        if(SceneManager.GetActiveScene().name == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
        if(SceneManager.GetActiveScene().name == "Level2")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator LevelLostCoroutine()
    {
        PlayerContrller.instance.canMove = false;
        UIManager.instance.GameOverPanel();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
