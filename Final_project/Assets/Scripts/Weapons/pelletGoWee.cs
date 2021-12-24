
using UnityEngine;

public class pelletGoWee : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 70f;
    public float range = 30f;
    
    [HideInInspector] new public Rigidbody rigidbody;
    private bool useGravity = true;
    private float despawnTime = 3f;
    private float spawnTime;
    void Start(){
        //cuz gravity was bugging vvvv
        rigidbody = GetComponent<Rigidbody>();
        
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
        transform.position += transform.forward*speed*Time.deltaTime;
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Call something like:
            // other.gameObject.triggerDamage(damage);
            //here...
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
    
}
