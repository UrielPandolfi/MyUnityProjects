using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public float spawnTime;
    public int gemsCollected;
    public string levelToLoad;
    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }
    
    IEnumerator RespawnCo()
    {
        PlayerController.instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(spawnTime - (1f / ControllerUI.instance.fadeSpeed));
        ControllerUI.instance.ToBlack();
        yield return new WaitForSeconds((1f / ControllerUI.instance.fadeSpeed)+.2f);
        ControllerUI.instance.FromBlack();
        PlayerController.instance.gameObject.SetActive(true);
        PlayerController.instance.transform.position = CheckpointController.instance.spawnPoint;
        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        ControllerUI.instance.updateHealthDisplay();
    }

    public void LevelEnd()
    {
        StartCoroutine(EndLevelCo());
    }

    public IEnumerator EndLevelCo()
    {
        PlayerController.instance.stopInput = true;
        ControllerUI.instance.levelEndText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ControllerUI.instance.ToBlack();
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);
        yield return new WaitForSeconds((1f / ControllerUI.instance.fadeSpeed)+.25f);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        SceneManager.LoadScene(levelToLoad);
    }
}