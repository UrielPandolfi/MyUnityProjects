using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    public MapPoint up, right, left, down;
    public MapPoint[] allPoints;
    public bool isLevel, isLocked;
    public string levelToLoad, leveltocheck, levelName;
    void Start()
    {
        allPoints = FindObjectsOfType<MapPoint>();
        if(isLevel)
        {
            isLocked=true;
            if(PlayerPrefs.HasKey(leveltocheck + "_unlocked"))
            {
                if(PlayerPrefs.GetInt(leveltocheck + "_unlocked") == 1)
                {
                    isLocked =  false;
                }
            }
            if(PlayerPrefs.HasKey("CurrentLevel"))
            {
                foreach(MapPoint point in allPoints)
                {
                    if(point.levelToLoad == PlayerPrefs.GetString("CurrentLevel"))
                    {
                        LSPlayer.instance.transform.position = point.transform.position;
                        LSPlayer.instance.currentPoint = point;
                    }
                }
            }
            if(levelToLoad == leveltocheck)
            {
                isLocked = false;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
