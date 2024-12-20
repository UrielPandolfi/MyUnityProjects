using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public Transform[] spawnPoints;

    [Header("Player Things")]   // Aquí iran las estadisticas de los jugadores, cada uno usara este script con estas variables para el seguimiento de estas
    public GameObject player;
    [HideInInspector]
    public int kills;
    [HideInInspector]
    public int deaths;

    [Header("Room Cam")]
    public GameObject roomCam;
    public GameObject nameScreen;
    public GameObject connectingScreen;
    private string nickname = "unnamed";
    public string roomNameToJoin = "unnamed";

    private void Awake()
    {
        instance = this;
    }

    public void OnJoinButtonPressed()   // Esta funcion se ejecuta luego de elegir un nombre.
    {
        roomNameToJoin = PlayerPrefs.GetString("RoomNameToJoin");
        Debug.Log("Connecting to" + roomNameToJoin + "...");

        // Creamos las opciones de la room y le asignamos una nueva variable llamada mapSceneIndex para que luego el usuario pueda unirse a esta scene desde el lobby.
        RoomOptions ro = new RoomOptions();
        ro.CustomRoomProperties = new Hashtable()
        {
            {"mapSceneIndex", SceneManager.GetActiveScene().buildIndex}
        };

        // Pasar el mapSceneIndex para el lobby
        ro.CustomRoomPropertiesForLobby = new[]
        {
            "mapSceneIndex"
        };

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, ro, null);

        nameScreen.SetActive(false);
        connectingScreen.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We're connected to a room now!");

        // Desactivamos la pantalla de connecting...
        roomCam.SetActive(false);

        SpawnPlayer();
    }

    public void SpawnPlayer() // Las funciones que no son RPC solo se ejecutan para el local player
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Length)]; // tomamos un spawnpoint random
        // Guardamos esta instancia ya que es nuestro jugador local
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        _player.GetComponent<PlayerSetup>().SetupAsLocal();
        _player.GetComponent<healthController>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetupNickname", RpcTarget.AllBuffered, nickname); // Buffered para que los nuevos players tambien lo vean asi
        PhotonNetwork.LocalPlayer.NickName = nickname;  // Seteamos en el photon network el nickname
   }

    public void ChangeName(string name) // Debemos crear esta funcion aca, ya que si no guardamos el nombre en el roomManager, cuando muera el jugador se perdera el nombre
    {
        nickname = name;
    }


    public void RefreshStats()
    {
        try
        {
            // Guardamos las anteriores stats
            Hashtable stats = PhotonNetwork.LocalPlayer.CustomProperties; 

            // Cargamos las kills y muertes
            stats["kills"] = kills;
            stats["deaths"] = deaths;

            // Lo seteamos
            PhotonNetwork.SetPlayerCustomProperties(stats);
        }
        catch
        {
            // Do nothing
        }
    }
}