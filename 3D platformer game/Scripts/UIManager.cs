using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Image blackScreen;
    public GameObject pauseScreen, optionsMenu;
    public bool fadeToBlack, fadeFromBlack;
    public float fadeSpeed;
    public Text healthPoints, coinsText;
    public Sprite[] healthBarImages;
    public Image healthBar;
    public Slider musicSlider, sfxSlider;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
        fadeFromBlack = true;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        if(fadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(blackScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if(fadeFromBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(blackScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }
    }

    public void UpdateHP()
    {
        healthPoints.text = PlayerHealthController.instance.currentHealth.ToString();

        switch(PlayerHealthController.instance.currentHealth)
        {
            case 5:
                healthBar.sprite = healthBarImages[4];
                break;
            case 4:
                healthBar.sprite = healthBarImages[3];
                break;
            case 3:
                healthBar.sprite = healthBarImages[2];
                break;
            case 2:
                healthBar.sprite = healthBarImages[1];
                break;
            case 1:
                healthBar.sprite = healthBarImages[0];
                break;
        }
    }

    public void PauseUnpause()
    {
        if(pauseScreen.gameObject.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            //Hacemos que al momento de despausar se vuelva a esconder el cursor y este bloqueado.
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            //Hacemos que al momento de pausar se vea el cursor y no este bloqueado.
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Resume()
    {
        PauseUnpause();
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void SetMusicSlider()
    {
        AudioManager.instance.musicMixer.audioMixer.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSfxSlider()
    {
        AudioManager.instance.sfxMixer.audioMixer.SetFloat("SfxVol", sfxSlider.value);
    }
}
