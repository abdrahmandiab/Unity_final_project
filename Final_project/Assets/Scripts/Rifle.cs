using UnityEngine;

public class Rifle : MonoBehaviour
{
    
    public float damage = 18f;
    //public float range = 65f;
    public float fireRate = 4f;
    private float RateSeconds;
    private float lastFire;
    //public GameObject fps;
    public int ammoCount = 35;
    public GameObject bulletPrefab;
     
    void Start(){
        // // Vector3 screenPos = new Vector3(Screen.width /2.0f, Screen.height/2.0f,0);
        // // Vector3 worldPos = ScreenToWorldPoint(screenPos);
        // transform.LookAt(fps.transform.forward);
        // transform.Rotate(-90,0,0);
        // initPos();
    }
    void Update()
    {

        RateSeconds = 1/fireRate;
        if (Input.GetMouseButton(0)){
            if(Time.time-lastFire> RateSeconds){
                lastFire = Time.time;
                Shoot();
            }
            else{
                Debug.Log("Can't fire that fast!");
            }
        }
        
    }
    // void initPos(){
    //     RaycastHit hit;
    //     if(Physics.Raycast(fps.transform.position,fps.transform.forward,out hit, range)){
    //     Debug.Log(hit.transform.position.x);
    //     }
    // }
    
    void Shoot(){
        GameObject bulletObject = Instantiate(bulletPrefab);
        bulletObject.transform.position = transform.position + transform.forward*0.3f;
        bulletObject.transform.Translate(Vector3.up * 0.02f);
        // bulletObject.transform.Translate(Vector3.left * 0.05f);
        Quaternion tempRot = transform.rotation;
        bulletObject.transform.rotation = tempRot;
        //bulletObject.transform.Rotate(0,90,0);
        //Alternative to lines for rotation is:
        //bulletObject.transform.forward = transform.forward;
    }
}
