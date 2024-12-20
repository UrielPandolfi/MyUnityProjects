using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public Transform showedWeapons;
    public GameObject cam;
    public Movement movement;
    public string nickname;
    public TextMeshPro nicknameText;

    // Esta funcion setea al jugador local como local ya que no es un rpc, y solo se llama en la pc de cada uno.
    public void SetupAsLocal()
    {
        cam.SetActive(true);
        movement.enabled = true;
    }

    [PunRPC]    // Esta funcion tiene que ser RPC porque todos se deben enterar de que se puso tal nombre
    public void SetupNickname(string _name)
    {
        nickname = _name;
        nicknameText.text = _name;
    }

    [PunRPC]
    public void ChangeShowedWeapon(int _index)
    {
        foreach(Transform _weapon in showedWeapons)
        {
            _weapon.gameObject.SetActive(false);
        }

        showedWeapons.GetChild(_index).gameObject.SetActive(true);
    }
}
