using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    public Text healthPoints;
    public PlayerStats playerStats;
    public Image hurtScreen;
    public GameObject winPanel, gameOverPanel;

    void Update()
    {
        healthPoints.text = playerStats.currentHealth.ToString();
    }

    public void WinPanel()
    {
        winPanel.SetActive(true);
    }

    public void GameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void HurtScreen()
    {
        StartCoroutine(HurtScreenCoroutine());
    }

    IEnumerator HurtScreenCoroutine()
    {
        hurtScreen.color = new Color(hurtScreen.color.r, hurtScreen.color.g, hurtScreen.color.b, 0.5f);
        yield return new WaitForSeconds(0.4f);
        hurtScreen.color = new Color(hurtScreen.color.r, hurtScreen.color.g, hurtScreen.color.b, 0f);
    }
}
