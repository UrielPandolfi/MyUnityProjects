using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerUI : MonoBehaviour
{
    public static ControllerUI instance;

    public Image heart1, heart2, heart3;
    public Sprite heartFull, heartEmpty, heartHalf;
    public Text gemText;
    public Image fadeScreen;
    public float fadeSpeed;
    private bool fromBlack, toBlack;
    public GameObject levelEndText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGemsDisplay();
        fromBlack = true;
    }


    void Update()
    {
        if(toBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 1f)
            {
                toBlack = false;
            }
        }
        if(fromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                fromBlack = false;
            }
        }
    }

    public void updateHealthDisplay()
    {
        switch(PlayerHealthController.instance.currentHealth)
        {
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;

                break;

            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;

                break;

            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;

                break;

            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;

                break;

            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;

            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;

            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                
                break;
        }
    }
    public void UpdateGemsDisplay()
    {
        gemText.text = LevelManager.instance.gemsCollected.ToString();
    }

    public void ToBlack()
    {
        toBlack = true;
    }

    public void FromBlack()
    {
        fromBlack = true;
    }
}
