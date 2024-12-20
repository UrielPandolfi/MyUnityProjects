using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class healthController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI hpText;
    public float health;
    public bool isLocalPlayer;

    [PunRPC] // La creamos como RPC (Remote Procedura Call) ya que cuando un jugador recibe daño, se debe enviar la informacion a todos.
    public void TakeDamage(float _damage)
    {
        health -= _damage;
        Debug.Log("Player has taken damage");

        hpText.text = "HP - " + health.ToString();
        if(health <= 0f)
        {
            if(isLocalPlayer)
            {
                RoomManager.instance.SpawnPlayer();
                RoomManager.instance.deaths++;
                RoomManager.instance.RefreshStats();
            }

            Destroy(gameObject);    
            hpText.text = "HP - 0";
        }
    }
}
