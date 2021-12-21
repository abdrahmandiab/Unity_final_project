using UnityEngine;

public class Shotgun : MonoBehaviour
{
    // private float range = 10000f;
    public float fireRate = 0.75f;
    private float RateSeconds;
    private float lastFire;
    public int ammoCount = 6;
    // public GameObject fps;
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
        if (Input.GetMouseButtonDown(0)  ){
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
        bulletObject.transform.position = transform.position + transform.forward*0.5f;
        bulletObject.transform.Translate(Vector3.up * 0.095f);
        // bulletObject.transform.Translate(Vector3.left * 0.05f);
        Quaternion tempRot = transform.rotation;
        bulletObject.transform.rotation = tempRot;
        //bulletObject.transform.Rotate(0,90,0);
        //Alternative to lines for rotation is:
        //bulletObject.transform.forward = transform.forward;
    }
}
