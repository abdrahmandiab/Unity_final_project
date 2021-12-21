
using UnityEngine;

public class bulletGoWeeSniper : MonoBehaviour
{
    public float speed = 100f;
    public float damage = 85f;
    public float range = 100f;
    
    [HideInInspector] new public Rigidbody rigidbody;
    private bool useGravity = true;
    private float despawnTime = 3f;
    private float spawnTime= 0f;
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
