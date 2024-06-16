using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LSPlayer : MonoBehaviour
{
    public static LSPlayer instance;
    private float moveSpeed = 10;
    public MapPoint currentPoint;
    public bool isLevel;
    
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime);
        
        if(transform.position == currentPoint.transform.position)
        {
            if(Input.GetAxisRaw("Horizontal") > .5f)
            {
                if(currentPoint.right != null && !currentPoint.right.isLocked)
                {
                    SetNextPoint(currentPoint.right);
                }
            }
            if(Input.GetAxisRaw("Horizontal") < -.5f)
            {
                if(currentPoint.left != null && !currentPoint.left.isLocked)
                {
                    SetNextPoint(currentPoint.left);
                }
            }
            if(Input.GetAxisRaw("Vertical") > .5f)
            {
                if(currentPoint.up != null && !currentPoint.up.isLocked)
                {
                    SetNextPoint(currentPoint.up);
                }
            }
            if(Input.GetAxisRaw("Vertical") < -.5f)
            {
                if(currentPoint.down != null && !currentPoint.down.isLocked)
                {
                    SetNextPoint(currentPoint.down);
                }
            }
            if(currentPoint.isLevel && !currentPoint.isLocked)
            {
                if(Input.GetButtonDown("Jump"))
                {
                    LoadLevel();
                }
            }
        }
        
        
    }

    public void SetNextPoint(MapPoint nextPoint)
    {
        currentPoint = nextPoint;
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCo());
    }

    public IEnumerator LoadLevelCo()
    {
        LSUIController.instance.ToBlack();
        yield return new WaitForSeconds((1f / LSUIController.instance.fadeSpeed) + .25f);
        SceneManager.LoadScene(currentPoint.levelToLoad);
    }
}
