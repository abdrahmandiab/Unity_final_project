using UnityEngine;

public class Shotgun : MonoBehaviour
{
    private float RateSeconds;
    private float lastFire;
    
    public float damage = 70f;
    public float range = 30f;
    public float fireRate = 3f;
    public int maxAmmo = 12;
    public int bulletsLeft;
    public Camera fps;
    //shotgun stuff
    public int n_pellets = 5;
    public float spread=0.2f;
    //Reload
    private bool reloading = false;
    private float reloadSpeed = 2f;
    private float reloadTimer; 

    //VFX
    public GameObject muzzleFlash, bulletHoleGraphic;
    public Transform flashPoint;
    //BloodHound
    private float beastModeRad = 10f;
    public static float beastModeTime = 10f;
    public static float beastModeTimer;
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
            if(Time.time < beastModeTimer){
                if(Time.time-lastFire> RateSeconds  ){
                    Instantiate(muzzleFlash,flashPoint.position, Quaternion.identity);
                    lastFire = Time.time;
                    Collider[] colliders = Physics.OverlapSphere(transform.position, beastModeRad);
                    if(colliders.Length != 0){
                        int randy = Random.Range(0,colliders.Length-1);
                        for(int i=0; i<n_pellets;i++){
                            ShootAimBot(colliders[randy]);   
                        }
                    }
                }
            }
            else{
                if (Input.GetMouseButtonDown(0)  ){
                    if(Time.time-lastFire> RateSeconds  ){
                        if(bulletsLeft>0){
                        lastFire = Time.time;
                        for(int i=0; i<n_pellets;i++){
                            Shoot();
                        }
                        Instantiate(muzzleFlash,flashPoint.position, Quaternion.identity);
                        bulletsLeft--;}
                        else{
                            Debug.Log("Out of bullets");
                            //gazara  (click sound aw 7aga)         
                        }
                    }
                    else{
                        Debug.Log("Can't fire that fast");
                        //gazara  (click sound aw 7aga)
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
    }
    void Shoot(){
        RaycastHit hit;
        float x = Random.Range(-spread,spread);
        float y = Random.Range(-spread,spread);
        Vector3 temp = fps.transform.forward;
        Vector3 direction = temp + new Vector3(x,y,0);
        
        if(Physics.Raycast(fps.transform.position,direction,out hit, range)){
            Debug.Log(hit.transform.name);
            if(hit.collider.CompareTag("Enemy")){
                Destroy(hit.collider.gameObject);
                // do damage = to damage/n_pellets.
            }
            if (hit.collider.CompareTag("Wall")){
                Instantiate(bulletHoleGraphic,hit.point, Quaternion.Euler(0,180,0));
            }
            if (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Terrain")){
                Instantiate(bulletHoleGraphic,hit.point, Quaternion.Euler(-90,180,0));
            }
        }
        
    }
    void ShootAimBot(Collider andy){
        Debug.Log(andy.transform.name);
        //.GetComponent<Rigidbody>();
        // do takeDamage(damage/n_pellets).
        
    }
    public static void goBeastMode(){
        beastModeTimer = Time.time + beastModeTime;  
        Debug.Log("Beast Mode activated!");
    }

}
