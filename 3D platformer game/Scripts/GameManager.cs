using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 respawnPosition;
    public int currentCoins, levelWinMusic;
    public string levelToLoad;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        respawnPosition = PlayerController.instance.transform.position;
        currentCoins = 0;
        UIManager.instance.coinsText.text = "" + currentCoins;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        StartCoroutine("RespawnCo");
    }

    public IEnumerator RespawnCo()
    {
        PlayerController.instance.gameObject.SetActive(false);
        CameraController.instance.cmBrain.enabled = false;
        UIManager.instance.fadeToBlack = true;
        yield return new WaitForSeconds(1f + (1f / UIManager.instance.fadeSpeed));
        CameraController.instance.cmBrain.enabled = true;
        UIManager.instance.fadeFromBlack = true;
        PlayerController.instance.transform.position = respawnPosition;
        PlayerController.instance.gameObject.SetActive(true);
        PlayerHealthController.instance.ResetHealth();
    }

    public void SetSpawnPoint(Vector3 checkpointPosition)
    {
        respawnPosition = checkpointPosition;
    }

    public void addCoins()
    {
        currentCoins++;
        UIManager.instance.coinsText.text = "" + currentCoins;
    }

    public IEnumerator LevelEndWaiter()
    {
        AudioManager.instance.stopMusic(2);
        AudioManager.instance.playMusic(levelWinMusic);
        PlayerController.instance.stopMove = true;
        yield return new WaitForSeconds(4f);
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, "unlocked");
        SceneManager.LoadScene(levelToLoad);
    }
}
