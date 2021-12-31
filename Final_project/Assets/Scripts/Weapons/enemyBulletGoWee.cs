using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletGoWee : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;

    private float despawnTime = 4f;
    private float spawnTime= 0f;
    void Start(){
        spawnTime=Time.time;
    }
    void Update()
    {
        if(Time.time-spawnTime >= despawnTime){
            Destroy(gameObject);
        }
        
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if(other.gameObject.CompareTag("Loba") ||other.gameObject.CompareTag("Bangalor") || other.gameObject.CompareTag("Bloodhound") ){
            other.gameObject.GetComponent<PlayerLogic>().takeDamage(damage);
            Destroy(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
}
