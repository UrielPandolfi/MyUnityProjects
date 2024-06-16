using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSUIController : MonoBehaviour
{
    public static LSUIController instance;
    public Image fadeScreen;
    public float fadeSpeed;
    private bool fromBlack, toBlack;
    public GameObject panelLevelName;
    public Text levelName;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        FromBlack();
    }

    // Update is called once per frame
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
        if(LSPlayer.instance.currentPoint.isLevel)
            {
                panelLevelName.SetActive(true);
                levelName.text = LSPlayer.instance.currentPoint.levelName;
            }
            else
            {
                panelLevelName.SetActive(false);
            }

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
