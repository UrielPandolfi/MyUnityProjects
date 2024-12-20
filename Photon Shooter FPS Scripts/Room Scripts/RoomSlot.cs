using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSlot : MonoBehaviour
{
    [HideInInspector]
    public string roomName = "unnamed";
    public int mapSceneIndex = 1;

    public void OnRoomPressed()
    {
        RoomList.instance.OnJoinRoomByName(roomName, mapSceneIndex);
    }
    
}
