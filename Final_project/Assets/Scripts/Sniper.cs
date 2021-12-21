using UnityEngine;

public class Sniper : MonoBehaviour
{
    
    public float damage = 60f;
    public float range = 100f;
    public float fireRate = 0.75f;
    private float RateSeconds;
    private float lastFire;
    //public Camera fpscam;
    public GameObject bulletPrefab;
     

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
