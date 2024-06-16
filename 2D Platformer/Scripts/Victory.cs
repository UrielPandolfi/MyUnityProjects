using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public string menu;
    public void Menu()
    {
        SceneManager.LoadScene(menu);
    }
}
