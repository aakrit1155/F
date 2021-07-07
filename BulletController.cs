using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifetime;
    public Rigidbody theRB;

    public GameObject impactEffect;
    
    public int damage = 1;

    public bool damageEnemy, damagePlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.forward * moveSpeed;         //move forward in the direction its facing. No need for deltatime because we are using velocity
        lifetime -= Time.deltaTime;
        if (lifetime <=0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if the tagged target is enemy
        if(other.gameObject.tag == "Enemy" && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        if(other.gameObject.tag == "Headshot" && damageEnemy)
        {
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage * 2);
            Debug.Log("Headshot");
        }

        if(other.gameObject.tag == "Player" && damagePlayer)
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }

        Destroy(gameObject);
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
    }

}
