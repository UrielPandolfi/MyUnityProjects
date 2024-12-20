using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using WebSocketSharp;

public class GameTextChat : MonoBehaviour
{
    public static GameTextChat instance;
    public TextMeshProUGUI chatText;
    public TMP_InputField inputField;
    [HideInInspector]
    public bool isChatting;
    private PhotonView photonView;

    void Awake()
    {
        instance = this;
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        isChatting = false;
    }
    void Update()
    {
        // Si toca enter se activa
        if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isChatting == false)
        {
            inputField.Select();
            inputField.ActivateInputField();
            isChatting = true;
            Debug.Log("You are chatting");
        }
        else if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isChatting == true)
        {
            if(!inputField.text.IsNullOrEmpty())
            {
                // Mandamos el mensaje y lo volvemos a poner en null
                string message = $"{PhotonNetwork.LocalPlayer.NickName}: {inputField.text}";
                photonView.RPC("SendChatMessage", RpcTarget.All, message);

                inputField.text = "";

                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("You sent a message");
                isChatting = false;

            }
            else
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("You stopped chatting");
                isChatting = false;
            }
        }
    }

    [PunRPC]
    public void SendChatMessage(string text)
    {
        chatText.text += "\n" + text; 
    }
}
