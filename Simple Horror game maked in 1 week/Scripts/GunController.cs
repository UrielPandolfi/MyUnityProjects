using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Shoot")]
    public float damage;
    public float shootSpeed;
    private float shootCooldown;
    public LayerMask shootMask;
    public Transform raycastOrigin;
    public GameObject bloodEffect, groundEffect;

    void Update()
    {
        shootCooldown -= Time.deltaTime;

        if(Input.GetButton("Shoot") && shootCooldown <= 0f) //Disparo
        {
            Shot();
        }
    }

    private void Shot()
    {
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, 100f, shootMask);
        if(hitSomething)
        {
            if(hit.collider.tag == "Enemy")
            {
                StatsController enemyStatsScript = hit.collider.gameObject.GetComponent<StatsController>();
                enemyStatsScript.TakeDamage(damage);
                Instantiate(bloodEffect, hit.point, transform.rotation);
            }
            if(hit.collider.tag == "Ground")
            {
                Instantiate(groundEffect, hit.point, transform.rotation);
            }
            Debug.Log("hit: " + hit.collider.name);
        }
        AudioManager.instance.PlaySFX(4);
        shootCooldown = 1f / shootSpeed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastOrigin.position, raycastOrigin.forward * 10f);
    }
}
