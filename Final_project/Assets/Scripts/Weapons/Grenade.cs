
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float damage = 50f;
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
            Rigidbody rigb = near.GetComponent<Rigidbody>();
            if (rigb!=null){
                rigb.AddExplosionForce(explosionForce,transform.position,radius,1f,ForceMode.Impulse);
                // near.takeDamage(damage);
            }
        }
        Instantiate(explosionEffect,transform.position, transform.rotation );
        Destroy(gameObject);
    }
    
}
