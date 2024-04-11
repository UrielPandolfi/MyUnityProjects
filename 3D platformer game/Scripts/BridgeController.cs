using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public string levelToUnlock;
    public GameObject bridge;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString(levelToUnlock) == "unlocked")
        {
            bridge.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
