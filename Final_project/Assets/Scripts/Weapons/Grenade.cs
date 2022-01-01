
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public int damage = 50;
    private float despawnTime = 4f;
    private float spawnTime = 0;
    public float radius = 4f;
    public float explosionForce = 10f;
    public GameObject explosionEffect;

    public float torque = 0.1f;
    float upwardForce = 0.3f;
    float shootForce = 0.5f;
    void Start(){
        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * upwardForce, ForceMode.Impulse);
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Impulse);
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.right * 30f);
        spawnTime=Time.time;
    }
    void Update()
    {
        if(Time.time-spawnTime >= despawnTime){
            explode();
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            explode();
            
        }
    } 
    void explode(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider near in colliders){
            if(near.gameObject.tag == "Enemy" ){
            Rigidbody rigb = near.GetComponent<Rigidbody>();
            if (rigb!=null){
                rigb.AddExplosionForce(explosionForce,transform.position,radius,1f,ForceMode.Impulse);
            }
            near.gameObject.GetComponent<EnemyAI>().takeDamage(damage);
            }

            // if(near.gameObject.tag == "Loba" || near.gameObject.tag == "Bangalor" ||near.gameObject.tag == "Bloodhound"  ){
            // Rigidbody rigb = near.GetComponent<Rigidbody>();
            // if (rigb!=null){
            //     rigb.AddExplosionForce(explosionForce,transform.position,radius,1f,ForceMode.Impulse);
            // }
            // near.gameObject.GetComponent<PlayerLogic>().takeDamage(damage);
            // }
            
        }
        Instantiate(explosionEffect,transform.position, transform.rotation );
        Destroy(gameObject);
    }
    
}
