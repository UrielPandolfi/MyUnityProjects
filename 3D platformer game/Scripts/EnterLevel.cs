using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevel : MonoBehaviour
{
    public string levelToCharge, levelToCheck;
    public bool canEnter;
    public GameObject particles;

    void Start()
    {
        if(PlayerPrefs.GetString(levelToCheck) == "unlocked")
        {
            particles.SetActive(true);
            canEnter = true;
        }
        else
        {
            canEnter = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && canEnter)
        {
            StartCoroutine("LevelLoadScreen");
        }
    }

    public IEnumerator LevelLoadScreen()
    {
        PlayerController.instance.stopMove = true;
        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(levelToCharge);
    }
}
