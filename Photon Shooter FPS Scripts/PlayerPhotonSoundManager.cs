using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPhotonSoundManager : MonoBehaviour
{
    // Creamos un audiosource para cada elemento digamos, para los pasos, armas, etc.
    private PhotonView photonView;
    [Header("movement")]
    public AudioSource walkSource;
    public AudioClip stepsClip;

    [Header("Weapons")]
    public AudioSource weaponSource;
    public AudioClip[] shootClip;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
 
    public void PlayerStepsSound()
    {
        photonView.RPC("PlayerStepsSound_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void PlayerStepsSound_RPC()
    {
        walkSource.clip = stepsClip;

        walkSource.pitch = Random.Range(0.7f, 1.2f);
        
        walkSource.Play();
    }

     public void WeaponsSound(int index)
    {
        photonView.RPC("WeaponsSound_RPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void WeaponsSound_RPC(int index)
    {
        weaponSource.clip = shootClip[index];

        weaponSource.pitch = Random.Range(0.7f, 1.2f);
        
        weaponSource.Play();
    }
}
