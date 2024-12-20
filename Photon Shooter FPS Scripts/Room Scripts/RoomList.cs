using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject createRoomScreen;
    public RoomManager roomManagerScript;
    public GameObject lobby;
    public Transform roomListParent;
    public GameObject roomPrefab;
    private List<RoomInfo> currentList = new List<RoomInfo>();
    [Header("CreateRoom Vars")]
    public int selectedMapIndex = 1;
    public string roomNameToCreate;
    IEnumerator Start()
    {
        // nos aseguramos de que no este conectado a ninguna room
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public void OnNewRoomButtonPressed()
    {
        createRoomScreen.SetActive(true);
    }

    // Boton cuando presionamos para crear una room, la cual ya elegimos mapa anteriormente y le pusimos un nombre con "ChangeRoomName"
    public void OnCreateRoomButtonPressed()
    {
        selectedMapIndex = 1;
        OnJoinRoomByName(roomNameToCreate, selectedMapIndex);
    }

    public void OnJoinRoomByName(string _roomName, int _sceneIndex)
    {
        PlayerPrefs.SetString("RoomNameToJoin", _roomName);

        SceneManager.LoadScene(_sceneIndex);
        // this.gameObject.SetActive(false);
    }

    public void ChangeRoomName(string _roomName) 
    {
        roomNameToCreate = _roomName;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) // Este parametro contiene la roomList actual
    {
        base.OnRoomListUpdate(roomList);

        if(currentList.Count == 0)
        {
            currentList = roomList;
        }
        else
        {
            // hacemos las actualizaciones debidas

            foreach(var room in roomList)
            {
                // buscamos la room en nuestra lista
                // Creamos un for nuevo para evitar errores, nos aseguramos que se recorra una vez por cada una de las salas para que si o si se encuentre
                for(int i = 0; i < currentList.Count; i++)
                {
                    if(room.Name == currentList[i].Name)
                    {
                        List<RoomInfo> newList = currentList;

                        if(room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            newList[i] = room;
                        }

                        currentList = newList;
                    }
                }
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Destruimos todos
        foreach(Transform roomSlot in roomListParent)
        {
            Destroy(roomSlot.gameObject);
        }

        // activamos los que haya que activar
        foreach(var room in currentList)
        {
            // Guardamos la nueva room
            GameObject newRoom = Instantiate(roomPrefab, roomListParent);

            // Accedemos a traves de GetChild a sus componentes de texto y los modificamos
            newRoom.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            newRoom.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount.ToString() + "/16";

            // Asignamos el nombre y scene index de la room al componente roomSlot para que luego el jugador se pueda unir al tocar click en la room
            newRoom.GetComponent<RoomSlot>().roomName = room.Name;

            // intentamos conseguir el valor del indice de la escnena, el cual mandamos hacia el lobby en el room manager a traves de "customPropertiesForLobby
            int roomSceneIndex = 1; // tenemos que darle un valor por si el TryGetValue no funciona
            object roomObject;

            if(room.CustomProperties.TryGetValue("mapSceneIndex", out roomObject))
            {
                roomSceneIndex = (int)roomObject;
            }

            newRoom.GetComponent<RoomSlot>().mapSceneIndex = roomSceneIndex;
        }
    }
}
