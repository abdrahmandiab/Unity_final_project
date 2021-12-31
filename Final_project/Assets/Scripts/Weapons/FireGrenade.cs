
using UnityEngine;

public class FireGrenade : MonoBehaviour
{

    public int damage = 25;
    public float duration = 5f;
    public int bouncies = 0;
    public int maxBouncies = 3;
    private float despawnTime = 4f;
    private float spawnTime = 0;
    public float radius = 4f;
    public float spread = 0.3f;
    public GameObject explosionEffect;
    public GameObject fireEffect;

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
        // transform.position += rate;
        
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
                near.gameObject.GetComponent<EnemyAI>().burn(damage);
                GameObject newFire = Instantiate(fireEffect,near.transform.position, transform.rotation );
                newFire.transform.parent = near.transform;
                }
                //Add code for burn(damage,duration);
            }
        Instantiate(explosionEffect,transform.position, transform.rotation );
        for (int i = 0; i<3; i++){
            float x = Random.Range(-spread,spread);
            float y = Random.Range(-spread,spread);
            Vector3 temp = transform.position;
            Vector3 spawnPoint = temp + new Vector3(x,y,0);
            Instantiate(fireEffect,spawnPoint, transform.rotation );
        }
        Destroy(gameObject);
        }
    
}
