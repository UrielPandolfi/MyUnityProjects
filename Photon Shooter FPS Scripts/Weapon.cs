using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class Weapon : MonoBehaviour
{
    public float fireRate;
    public float damage;

    public Camera cam;
    private float fireCount;
    [Header("VFX")]
    public GameObject glockParticle;

    [Header("SFX")]
    public PlayerPhotonSoundManager soundManager;
    public int shootIndex = 0;

    [Header("Ammo")]
    public int mag = 5;         // Cantidad de cartuchos
    public int ammo = 30;       // Cantidad de balas inicial
    public int magAmmo = 30;    // Cantidad de balas maximas del cartucho

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Recoil Settings")]
    public float upRecoil;
    public float backRecoil;
    [Range(0,1)]
    public float recoilPercent;
    [Range(0,2)]
    public float recoverPercent;
    private float recoilLenght;     // Duración del recoil, mientras mas bajo mas brusco el movimiento y menos amortiguado
    private float recoverLenght;    // Duración del recover del arma, mientras mas bajo mas brusco el movimiento y menos amortiguado
    private Vector3 origin;
    private Vector3 recoilVelocity = Vector3.zero; // Referencia simplemente para guardar la velocidad del recoil como referencia
    private bool recoiling = false;
    private bool recovering = false;

    [Header("Animations")]
    public Animation animation;
    public AnimationClip reloadClip;

    void Start()
    {
        origin = transform.localPosition;

        recoilLenght = 1 / fireRate * recoilPercent;
        recoverLenght = 1 / fireRate * recoverPercent;

        UpdateUI();
    }

    void Update()
    {

        // Delay entre bala y bala
        if(fireCount > 0f)
        {
            fireCount -= Time.deltaTime;
        }

        if(Input.GetButton("Fire1") && fireCount <= 0f && ammo > 0 && !animation.isPlaying)
        {
            Fire();
            fireCount = 1 / fireRate; // Mientras mas rapido el Fire Rate mas rapido se disparara

            ammo--;

            UpdateUI();
        }

        
        if(Input.GetKeyDown(KeyCode.R) && ammo < magAmmo)
        {
            Reload();
        }

        if(recoiling)
        {
            Recoil();
        }

        if(recovering)
        {
            Recover();
        }

    }

    public void Fire( )
    {
        recoiling = true;
        recovering = false;
        // Hacer un hit para guardar la info del raycast
        RaycastHit hit;

        //El sonido
        soundManager.WeaponsSound(shootIndex);
        // Ejecutamos el disparo    
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            PhotonNetwork.Instantiate(glockParticle.name, hit.point, Quaternion.identity);

            if(hit.collider.tag == "Player")
            {
                // si el raycast golpea un jugador, tomamos su componente y le sacamos daño.
                hit.collider.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);

                // Si matamos al otro jugador
                if(hit.collider.GetComponent<healthController>().health <= damage)
                {
                    PhotonNetwork.LocalPlayer.AddScore(100);
                    RoomManager.instance.kills++;
                    RoomManager.instance.RefreshStats();
                }
            }   
        }
        
    }

    public void Reload()
    {
        if(mag > 0)
        {
            animation.Play(reloadClip.name);
            ammo = magAmmo;
            mag--;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo.ToString() + " / " + magAmmo.ToString();
    }

    public void Recoil()
    {
        Vector3 finalPosition = new Vector3(origin.x, origin.y + upRecoil, origin.z - backRecoil);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLenght);

        if(Vector3.Distance(transform.localPosition, finalPosition) < 0.1f)
        {
            recoiling = false;
            recovering = true;
        }
    }
    public void Recover()
    {
        Vector3 finalPosition = origin;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLenght);

        if(Vector3.Distance(transform.localPosition, finalPosition) < 0.1f)
        {
            recovering = false;
        }
    }
}
