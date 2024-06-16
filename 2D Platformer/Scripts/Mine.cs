using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject explosion;
    private float explosionCounter;
    public float timeToExplode;

    // Start is called before the first frame update
    void Start()
    {
        explosionCounter = timeToExplode;
    }

    // Update is called once per frame
    void Update()
    {
        explosionCounter -= Time.deltaTime;
        if(explosionCounter < 0)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealthController.instance.dealDamage();
            Explode();
        }
    }

    public void Explode()
    {
        gameObject.SetActive(false);
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
