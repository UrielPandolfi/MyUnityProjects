using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject cpOn, cpOff;
    private Vector3 checkpointPosition;

    void Start()
    {
        checkpointPosition = transform.position;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach(Checkpoint point in checkpoints)
        {
            point.DeactivateCp();
        }
        cpOn.SetActive(true);
        cpOff.SetActive(false);
        GameManager.instance.SetSpawnPoint(checkpointPosition);
    }

    public void DeactivateCp()
    {
        cpOff.SetActive(true);
        cpOn.SetActive(false);
    }
}
