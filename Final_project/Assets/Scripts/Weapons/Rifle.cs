using UnityEngine;

public class Rifle : MonoBehaviour
{
    public GameObject bulletPrefab;
     
    //Helpers
    private float RateSeconds;
    private float lastFire;

    //Gun profile
    public int damage = 10;
    public float range = 65f;
    public float fireRate = 10f;
    public int maxAmmo = 35;
    public int bulletsLeft;
    public float spread=0.05f;
    //reload
    private bool reloading = false;
    private float reloadSpeed = 2f;
    private float reloadTimer; 
    //for raycast
    public Camera fps;
    //VFX
    public GameObject muzzleFlash, bulletHoleGraphic;
    public Transform flashPoint;
    //Bloodhound
    private float beastModeRad = 10f;
    public float beastModeTime = 10f;
    public float beastModeTimer;
    public LayerMask enemiesLayer;
    void Start(){
        RateSeconds = 1/fireRate;
        bulletsLeft = maxAmmo;
    }
    void Update()
    {
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
                    lastFire = Time.time;
                    ShootAimBot();
                   
                 }
            }
            else{
                if (Input.GetMouseButton(0)  ){
                    if(Time.time-lastFire> RateSeconds  ){
                        if(bulletsLeft>0){
                        lastFire = Time.time;
                        Shoot();
                        Instantiate(muzzleFlash,flashPoint.position, Quaternion.identity);
                        bulletsLeft--;}
                        else{
                            Debug.Log("Out of bullets");
                            //gazara  (click sound aw 7aga)
                        }
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
            //Debug.Log(hit.transform.name);
            if(hit.collider.CompareTag("Enemy")){
                hit.collider.gameObject.GetComponent<EnemyAI>().takeDamage(damage);
            }
            if (hit.collider.CompareTag("Wall")){
                Instantiate(bulletHoleGraphic,hit.point, Quaternion.Euler(0,180,0));
            }
            if (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Terrain")){
                Instantiate(bulletHoleGraphic,hit.point, Quaternion.Euler(-90,180,0));
            }
        }
    }
    void ShootAimBot(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, beastModeRad, enemiesLayer);
        if(colliders.Length != 0){
            
            int randy = Random.Range(0,colliders.Length-1);
            if(colliders[randy].gameObject.tag == "Enemy" ){
                Instantiate(muzzleFlash,flashPoint.position, Quaternion.identity);
                colliders[randy].gameObject.GetComponent<EnemyAI>().takeDamage(damage);
            }
        }
    }
    public void goBeastMode(){
        beastModeTimer = Time.time + beastModeTime;  
        Debug.Log("Beast Mode activated!");
    }

}
