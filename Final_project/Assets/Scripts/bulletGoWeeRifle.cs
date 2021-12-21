
using UnityEngine;

public class bulletGoWeeRifle : MonoBehaviour
{
    [HideInInspector] new public Rigidbody rigidbody;
    public bool useGravity = true;
    private float despawnTime = 3f;
    private float spawnTime= 0f;
    public float speed = 100f;
    private Vector3 rate;

    public float range= 65; // Interpretted as 6.5*2=13 world units, instead of 65
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
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
    
}
