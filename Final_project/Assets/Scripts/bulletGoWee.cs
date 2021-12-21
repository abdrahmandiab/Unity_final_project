using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletGoWee : MonoBehaviour
{
    [HideInInspector] new public Rigidbody rigidbody;
    public bool useGravity = true;
    private float despawnTime = 3f;
    private float spawnTime= 0f;
    public float speed = 100f;
    private Vector3 rate;
    void Start(){
        rigidbody = GetComponent<Rigidbody>();
        rate = transform.forward*speed*Time.deltaTime;
        transform.Rotate(0,90,0);
        spawnTime=Time.time;
    }
    void Update()
    {
        rigidbody.useGravity = false;
        if (useGravity) {rigidbody.AddForce(Physics.gravity * (rigidbody.mass * rigidbody.mass));}
        
        if(Time.time-spawnTime >= despawnTime){
        Destroy(gameObject);
        }
        //Make bullet move
        transform.position += rate;
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Terrain") || other.gameObject.CompareTag("Terrain") )
        {
            Destroy(gameObject);
        }
    }
    
}
