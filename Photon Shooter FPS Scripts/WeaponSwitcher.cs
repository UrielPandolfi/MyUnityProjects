using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponSwitcher : MonoBehaviour
{
    public PhotonView playerSetupView;
    private int selectedWeapon = 0;
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousWeapon = selectedWeapon; // guardamos el arma anterior antes de hacer algun cambio

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedWeapon = 3;
        }

        // AGREGAR CAMBIO POR RUEDITA

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(selectedWeapon >= transform.childCount - 1) // Si no quedan mas armas arriba volvemos a 0
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;    
            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(selectedWeapon <= 0) // Si no quedan mas armas arriba volvemos a 0
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;    
            }
        }

        if(selectedWeapon != previousWeapon)
        {
            SelectWeapon();
        }
    }

    private void SelectWeapon()
    {
        int i = 0;

        foreach(Transform _weapon in transform) // esta funcion es para acceder a los hijos del gameobject, el 0 seria el primero y asi...
        {
            if(i == selectedWeapon)
            {
                _weapon.gameObject.SetActive(true);
            }
            else
            {
                _weapon.gameObject.SetActive(false);
            }

            i++;
        }

        playerSetupView.RPC("ChangeShowedWeapon", RpcTarget.All, selectedWeapon);
    }
}
