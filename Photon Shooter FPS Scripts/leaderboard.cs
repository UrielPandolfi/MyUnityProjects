using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;

public class leaderboard : MonoBehaviour
{
    public float refreshRate = 1f;
    public GameObject playerHolder;
    public GameObject[] slots;
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI[] nameTexts;
    public TextMeshProUGUI[] kdaTexts;

    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    private void Update()
    {
        playerHolder.SetActive(Input.GetKey(KeyCode.Tab));   
    }

    private void Refresh()
    {
        int i = 0;
        // Desctivamos los slots
        foreach(GameObject slot in slots)
        {
            slot.SetActive(false);
        }

        // Ordenamos la lista en una Var
        var sortedPlayerList = PhotonNetwork.PlayerList.OrderByDescending(player /* cada uno de los elementos del a lista */ => player.GetScore()).ToList();
        
        
        foreach(var player in sortedPlayerList)
        {
            // Activamos el slot en el ui
            slots[i].SetActive(true);
            
            if(player.NickName == "")
            {
                player.NickName = "unnamed";
            }
            
            nameTexts[i].text = player.NickName;
            scoreTexts[i].text = player.GetScore().ToString();
            if(player.CustomProperties["kills"] != null)
            {
                kdaTexts[i].text = player.CustomProperties["kills"] + "/" + player.CustomProperties["deaths"];
            }                
            else
            {
                kdaTexts[i].text = "0/0";
            }
            i++;
        }
    }
}

