using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerLogic : MonoBehaviour
{
    public int health;
    public int specialAbilitiesMeter;
    private float lastUpdate_SpecialAbilitiesMeter;
    private float lastUpdate_BangalorSpecialAbilityActivated;
    private Collider canPickUp;
    public string selectedWeapon;
    public string carriedPrimaryWeapon;
    private string carriedSecondaryWeapon;
    private int primaryAmmoCount;
    private int secondaryAmmoCount;
    private int noEnemyShot;
    private bool isGameOver, isPaused, shieldOn;
    public GameObject ball;
    public Transform startPoint;

    private PostProcessVolume ppvol;
    private ColorGrading cGrade = null;
    public GameObject mainCam;
    // Prehabs: to implement drop weapons logic
    public GameObject[] weapons = new GameObject[5];
    //0 is Grenade Launcher
    //1 is Sniper
    //2 is Shotgun
    //3 is Rifle
    //4 is Fire Grenade Launcher



    // Start is called before the first frame update
    void Start()
    {
        //- Diab stuff
        ppvol = mainCam.GetComponent<PostProcessVolume>();
        ppvol.profile.TryGetSettings(out cGrade);



        // Health is initially set to 100
        health = 100;
        // Special abilities meter is initially empty and not updated
        specialAbilitiesMeter = 0;
        lastUpdate_SpecialAbilitiesMeter = Time.time;
        // No objectis picked up initially
        canPickUp = null;
        //initally no weapon is carried
        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
        carriedPrimaryWeapon = null;
        carriedSecondaryWeapon = null;
        selectedWeapon = null;
        primaryAmmoCount = 0;
        secondaryAmmoCount = 0;
        //for Banaglore's special abilty
        lastUpdate_BangalorSpecialAbilityActivated = Time.time;
        shieldOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            if (Input.GetKeyDown("escape") && isPaused)
            {
                resume();
            }
            else if (Input.GetKeyDown("escape"))
            {
                pause();

            }
            if (!isPaused)
            {
                // Disable Bangalore's special ability after 10 seconds
                if(gameObject.CompareTag("Bangalor") && (Time.time - lastUpdate_BangalorSpecialAbilityActivated) >= 10.0f)
                {
                    this.gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
                    shieldOn = false;
                }

                // Increase the special abilities meter every second
                if (specialAbilitiesMeter != 100 && Time.time - lastUpdate_SpecialAbilitiesMeter >= 0.2f)
                {
                    specialAbilitiesMeter += 1;
                    lastUpdate_SpecialAbilitiesMeter = Time.time;
                }

                // Activate special abilites
                if (specialAbilitiesMeter == 100 && Input.GetKeyDown(KeyCode.Q) && !gameObject.CompareTag("Bloodhound"))
                {
                    executeSpecialAbility();
                    specialAbilitiesMeter = 0;
                }
                else if (specialAbilitiesMeter == 100 && Input.GetKeyDown(KeyCode.Q) && gameObject.CompareTag("Bloodhound")){
                    if(carriedPrimaryWeapon!=null){
                        executeSpecialAbility();
                        specialAbilitiesMeter = 0;
                    }else{
                        Debug.Log("Equip a primary first!");
                    }
                }

                //pick up logic
                if (canPickUp != null && Input.GetKeyDown(KeyCode.E))
                {
                    if (canPickUp.gameObject.CompareTag("Health Pack"))
                    {
                        //besela: pick up animaton needed
                        Destroy(canPickUp.gameObject);
                        health = health + 25 > 100 ? 100 : health + 25;
                        canPickUp = null;
                    }
                    else if (canPickUp.gameObject.CompareTag("Grenade Launcher") || canPickUp.gameObject.CompareTag("Fire Launcher"))
                    {
                        if (carriedSecondaryWeapon!=null)
                        {
                            //we must drop current secondary weapon first!
                            //besela: drop animation needed
                            GameObject gameObject;
                            float x = canPickUp.gameObject.transform.position.x; 
                            float y = canPickUp.gameObject.transform.position.y;
                            float z = canPickUp.gameObject.transform.position.z;

                            if (carriedSecondaryWeapon.Equals("Grenade Launcher"))
                            {
                                //drop grenade launcher
                                gameObject = (GameObject) Instantiate(weapons[4]);
                                gameObject.transform.SetParent(transform);
                                gameObject.transform.position = new Vector3(x, y, z);
                                this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                            }
                            else if (carriedSecondaryWeapon.Equals("Fire Launcher"))
                            {
                                //drop grenade launcher
                                gameObject = (GameObject)Instantiate(weapons[3]);
                                gameObject.transform.SetParent(transform);
                                gameObject.transform.position = new Vector3(x, y, z);
                                this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                            }
                        }
                        //besela: pick up animaton needed
                        Destroy(canPickUp.gameObject);
                        if (canPickUp.gameObject.CompareTag("Grenade Launcher"))
                        {
                            //pick up grenade launcher
                            carriedSecondaryWeapon = "Grenade Launcher";
                            if (carriedPrimaryWeapon == null || !selectedWeapon.Equals(carriedPrimaryWeapon))
                                //select weapon
                                //besela: switching animaton needed
                                selectedWeapon = carriedSecondaryWeapon;
                                this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        }
                        else if (canPickUp.gameObject.CompareTag("Fire Launcher"))
                        {
                            //pick up fire launcher
                            carriedSecondaryWeapon = "Fire Launcher";
                            if (carriedPrimaryWeapon ==null || !selectedWeapon.Equals(carriedPrimaryWeapon))
                                //select weapon
                                //besela: switching animaton needed
                                selectedWeapon = carriedSecondaryWeapon;
                                this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                        }
                        canPickUp = null;
                    }
                    else if (canPickUp.gameObject.CompareTag("Sniper") || canPickUp.gameObject.CompareTag("Shotgun") || canPickUp.gameObject.CompareTag("Rifle"))
                    {
                        if (carriedPrimaryWeapon != null)
                        {
                            //we must drop current primary weapon first!
                            //besela: drop animation needed
                            GameObject gameObject;
                            float x = canPickUp.gameObject.transform.position.x;
                            float y = canPickUp.gameObject.transform.position.y;
                            float z = canPickUp.gameObject.transform.position.z;

                            if (carriedPrimaryWeapon.Equals("Sniper"))
                            {
                                //drop Sniper
                                gameObject = (GameObject)Instantiate(weapons[1]);
                                gameObject.transform.SetParent(transform);
                                gameObject.transform.position = new Vector3(x, y, z);
                                this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                            }
                            else if (carriedPrimaryWeapon.Equals("Shotgun"))
                            {
                                //drop Shotgun
                                gameObject = (GameObject)Instantiate(weapons[2]);
                                gameObject.transform.SetParent(transform);
                                gameObject.transform.position = new Vector3(x, y, z);
                                this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                            }
                            else if (carriedPrimaryWeapon.Equals("Rifle"))
                            {
                                //drop Rifle
                                gameObject = (GameObject)Instantiate(weapons[0]);
                                gameObject.transform.SetParent(transform);
                                gameObject.transform.position = new Vector3(x, y, z);
                                this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                            }
                        }
                        //besela: pick up animaton needed
                        Destroy(canPickUp.gameObject);
                        if (canPickUp.gameObject.CompareTag("Sniper"))
                        {
                            //pick up Sniper
                            carriedPrimaryWeapon = "Sniper";
                            if (carriedSecondaryWeapon == null || !selectedWeapon.Equals(carriedSecondaryWeapon))
                                //select weapon
                                //besela: switching animaton needed
                                selectedWeapon = carriedPrimaryWeapon;
                                this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                        }
                        else if (canPickUp.gameObject.CompareTag("Shotgun"))
                        {
                            //pick up grenade launcher
                            carriedPrimaryWeapon = "Shotgun";
                            if (carriedSecondaryWeapon == null || !selectedWeapon.Equals(carriedSecondaryWeapon))
                                //select weapon
                                //besela: switching animaton needed
                                selectedWeapon = carriedPrimaryWeapon;
                                this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                        }
                        else if (canPickUp.gameObject.CompareTag("Rifle"))
                        {
                            //pick up grenade launcher
                            carriedPrimaryWeapon = "Rifle";
                            if (carriedSecondaryWeapon == null || !selectedWeapon.Equals(carriedSecondaryWeapon))
                                //select weapon
                                //besela: switching animaton needed
                                selectedWeapon = carriedPrimaryWeapon;
                                this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        }
                        canPickUp = null;
                    }
                    else if (canPickUp.gameObject.CompareTag("Primary Ammo"))
                    {
                        //besela: pick up animaton needed
                        Destroy(canPickUp.gameObject);
                        primaryAmmoCount = (primaryAmmoCount + 50) > 150 ? 150 : (primaryAmmoCount + 50);
                        canPickUp = null;
                    }
                    else if (canPickUp.gameObject.CompareTag("Secondary Ammo"))
                    {
                        //besela: pick up animaton needed
                        Destroy(canPickUp.gameObject);
                        if (gameObject.CompareTag("Loba"))
                        {
                            secondaryAmmoCount = (secondaryAmmoCount + 2) > 10 ? 10 : (secondaryAmmoCount + 2);
                        }
                        else
                        {
                            secondaryAmmoCount = (secondaryAmmoCount + 2) > 5 ? 5 : (secondaryAmmoCount + 2);
                        }
                        canPickUp = null;
                    }
                }
                //switch weapons logic
                if (Input.GetKeyDown(KeyCode.Z) && carriedPrimaryWeapon!=null && carriedSecondaryWeapon!=null)
                {
                    //besela: switching animaton needed
                    selectedWeapon = selectedWeapon.Equals(carriedPrimaryWeapon) ? carriedSecondaryWeapon : carriedPrimaryWeapon;
                    if (selectedWeapon.Equals("Grenade Launcher"))
                    {
                        //select grenade launcher
                        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                    }
                    else if (selectedWeapon.Equals("Sniper"))
                    {
                        //select sniper
                        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                        this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                    }
                    else if (selectedWeapon.Equals("Shotgun"))   // Weird shit happening, this turned to index 3 instead of 2 - DIAB
                    {
                        //select shotgun
                        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                    }
                    else if (selectedWeapon.Equals("Rifle"))    // Weird shit happening, this turned to index 3 instead of 2 - DIAB
                    {
                        //select rifle
                        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                    }
                    else if (selectedWeapon.Equals("Fire Launcher"))
                    {
                        //select fire launcher
                        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                    }
                }
            }
            
        }
    }

    // Special ability logic
    private void executeSpecialAbility()
    {

        if(gameObject.CompareTag("Loba"))
        {
            GameObject thrownBall = Instantiate(ball, startPoint.position, startPoint.rotation);
            thrownBall.GetComponent<Rigidbody>().velocity = startPoint.transform.up * 7.0f;
        }
        else if(gameObject.CompareTag("Bangalor"))
        {
            this.gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
            lastUpdate_BangalorSpecialAbilityActivated = Time.time;
            shieldOn = true;
        }
        else if(gameObject.CompareTag("Bloodhound"))
        {
        goBW();
        Invoke(nameof(unBW), 10f);
        selectedWeapon = carriedPrimaryWeapon;
        // Debug.Log("Selected Weapon:" + selectedWeapon);
        if (selectedWeapon.Equals("Sniper")){
            //select sniper
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Sniper>().goBeastMode();
        }
        else if (selectedWeapon.Equals("Shotgun"))
        {
            //select shotgun
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

            // Debug.Log("Shotgun Beast mode!");
            this.gameObject.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Shotgun>().goBeastMode();
        }
        else if (selectedWeapon.Equals("Rifle"))
        {
            //select rifle
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
            
            // Debug.Log("Rifle Beast mode!");
            this.gameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Rifle>().goBeastMode();
        }

        }
        
    }

    // POST PROCESSING STUFF
    public void goBW(){
        cGrade.saturation.value = -100f;

    }
    public void unBW(){
        cGrade.saturation.value = 0f;
    }


    // Picking up logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Health Pack") || other.gameObject.CompareTag("Grenade Launcher") || other.gameObject.CompareTag("Sniper") || other.gameObject.CompareTag("Shotgun") || other.gameObject.CompareTag("Rifle") || other.gameObject.CompareTag("Fire Launcher") || other.gameObject.CompareTag("Primary Ammo") || other.gameObject.CompareTag("Secondary Ammo"))
        {
            canPickUp = other;
        }
    }

    // Picking up logic
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Health Pack") || other.gameObject.CompareTag("Grenade Launcher") || other.gameObject.CompareTag("Sniper") || other.gameObject.CompareTag("Shotgun") || other.gameObject.CompareTag("Rifle") || other.gameObject.CompareTag("Fire Launcher") || other.gameObject.CompareTag("Primary Ammo") || other.gameObject.CompareTag("Secondary Ammo"))
        {
            canPickUp = null;
        }
    }
    
    // Game over logic
    public void gameOver()
    {
        // Gazara: music needed
        isGameOver = true;
        // Batates: Game over menu
    }

    public void resume()
    {
        // Gazara: music needed
        isPaused = false;
        // Batates: Pause menu
    }

    public void pause()
    {
        // Gazara: music needed
        isPaused = true;
        // Batates: Pause menu
    }

    // Method for Diab
    public void takeDamage(int amount)
    {
        // Special ability: Defensive Shield: protects Bangalor from any damage for 10 seconds.
        if (!shieldOn)
        {
            health = health - amount < 0 ? 0 : health - amount;
        }
        // Whenever the player's health points reaches zero, the player dies
        if (health == 0)
            gameOver();

    }

    // Method for Diab
    public void burn(int amount, float duration)
    {
        health = health - amount < 0 ? 0 : health - amount;
    }

    public void noEnemiesShot()
    {
        noEnemyShot++;
    }

    //getters & setters
    public int getPrimaryAmmoCount()
    {
        return primaryAmmoCount;
    }

    public int getSecondaryAmmoCount()
    {
        return secondaryAmmoCount;
    }

    public void setPrimaryAmmoCount(int val)
    {
        primaryAmmoCount = val < 0 ? 0 : val;
    }

    public void setSecondaryAmmoCount(int val)
    {
        secondaryAmmoCount = val < 0 ? 0 : val;
    }

}
