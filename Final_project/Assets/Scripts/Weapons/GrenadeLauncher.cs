using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    public GameObject bulletPrefab;
     
    //Helpers
    private float RateSeconds;
    private float lastFire;

    //Gun profile

    public float fireRate = 1f;
    public int maxAmmo = 3;
    public int bulletsLeft;
    //reload
    private bool reloading = false;
    private float reloadSpeed = 2f;
    private float reloadTimer; 
    //for raycast
    //public Camera fps;
    //VFX
    public Transform flashPoint;
    void Start(){
        bulletsLeft = maxAmmo;
    }
    void Update()
    {
         RateSeconds = 1/fireRate;
        if(reloading){
            if(Time.time >= reloadTimer){
                reloading = false;
                bulletsLeft = maxAmmo;
                Debug.Log("Finished reloading!");
                
            }
            
        }
        else{
            if (Input.GetMouseButtonDown(0)  ){
                if(Time.time-lastFire> RateSeconds  ){
                    if(bulletsLeft>0){
                    lastFire = Time.time;
                    Shoot();
                    //Instantiate(muzzleFlash,flashPoint.position, Quaternion.identity);
                    bulletsLeft--;}
                    else{
                         Debug.Log("Out of bullets");
                         //gazara (click sound aw 7aga)
                    }
                }
                else{
                    Debug.Log("Can't fire that fast");
                    //gazara (click sound aw 7aga)
                }
            }
            if (Input.GetKeyDown(KeyCode.R)){
                reloading = true;
                reloadTimer = Time.time + reloadSpeed;
                Debug.Log("Reloading! (needs animation)");    
                //gazara
                //besela
            }
        }
    }
    void Shoot(){
        GameObject bulletObject = Instantiate(bulletPrefab);
        bulletObject.transform.position = flashPoint.position;
        bulletObject.transform.rotation = transform.rotation;
        // Quaternion tempRot = transform.rotation;
        // bulletObject.transform.rotation = tempRot;
    }

    // void Shoot(){
    //     RaycastHit hit;
    //     float x = Random.Range(-spread,spread);
    //     float y = Random.Range(-spread,spread);
    //     Vector3 temp = fps.transform.forward;
    //     Vector3 direction = temp + new Vector3(x,y,0);
        
    //     if(Physics.Raycast(fps.transform.position,direction,out hit, range)){
    //         Debug.Log(hit.transform.name);
    //         if(hit.collider.CompareTag("Enemy")){
    //             Destroy(hit.collider.gameObject);
    //         }
    //         if (hit.collider.CompareTag("Wall")){
    //             Instantiate(bulletHoleGraphic,hit.point, Quaternion.Euler(0,180,0));
    //         }
    //         if (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Terrain")){
    //             Instantiate(bulletHoleGraphic,hit.point, Quaternion.Euler(-90,180,0));
    //         }
    //     }
    // }

}
