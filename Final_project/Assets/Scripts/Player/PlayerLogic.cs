using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public int health;
    public int specialAbilitiesMeter;
    private float lastUpdate_SpecialAbilitiesMeter;
    private bool abilityDeployed;
    private Collider canPickUp;
    private string selectedWeapon;
    private string carriedPrimaryWeapon;
    private string carriedSecondaryWeapon;
    private int noEnemyShot;
    private bool isGameOver, isPaused;


    // Prehabs: to implement drop weapons logic
    public GameObject[] weapons = new GameObject[5];




    // Start is called before the first frame update
    void Start()
    {
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
                // Increase the special abilities meter every second
                if (specialAbilitiesMeter != 100 && Time.time - lastUpdate_SpecialAbilitiesMeter >= 0.2f)
                {
                    specialAbilitiesMeter += 1;
                    lastUpdate_SpecialAbilitiesMeter = Time.time;
                }

                // Activate special abilites
                if (!abilityDeployed && specialAbilitiesMeter == 100 && Input.GetKeyDown(KeyCode.Q))
                {
                    specialAbilitiesMeter = 0;
                }

                // Execute special ability
                if (abilityDeployed)
                    executeSpecialAbility();

                //pick up logic
                if (canPickUp != null && Input.GetKeyDown(KeyCode.E))
                {
                    if (canPickUp.gameObject.CompareTag("Health Pack") && health < 75)
                    {
                        //besela: pick up animaton needed
                        Destroy(canPickUp.gameObject);
                        health += 25;
                        canPickUp = null;
                    }
                    else if (canPickUp.gameObject.CompareTag("Grenade Launcher") || canPickUp.gameObject.CompareTag("Fire Launcher"))
                    {
                        if (!carriedSecondaryWeapon.Equals(null))
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
                            if (carriedPrimaryWeapon.Equals(null) || !selectedWeapon.Equals(carriedPrimaryWeapon))
                                //select weapon
                                selectedWeapon = carriedSecondaryWeapon;
                            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        }
                        else if (canPickUp.gameObject.CompareTag("Fire Launcher"))
                        {
                            //pick up fire launcher
                            carriedSecondaryWeapon = "Fire Launcher";
                            if (carriedPrimaryWeapon.Equals(null) || !selectedWeapon.Equals(carriedPrimaryWeapon))
                                //select weapon
                                selectedWeapon = carriedSecondaryWeapon;
                            this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                        }
                        canPickUp = null;
                    }
                    else if (canPickUp.gameObject.CompareTag("Sniper") || canPickUp.gameObject.CompareTag("Shotgun") || canPickUp.gameObject.CompareTag("Rifle"))
                    {
                        if (!carriedPrimaryWeapon.Equals(null))
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
                            if (carriedSecondaryWeapon.Equals(null) || !selectedWeapon.Equals(carriedSecondaryWeapon))
                                //select weapon
                                selectedWeapon = carriedPrimaryWeapon;
                            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                        }
                        else if (canPickUp.gameObject.CompareTag("Shotgun"))
                        {
                            //pick up grenade launcher
                            carriedPrimaryWeapon = "Shotgun";
                            if (carriedSecondaryWeapon.Equals(null) || !selectedWeapon.Equals(carriedSecondaryWeapon))
                                //select weapon
                                selectedWeapon = carriedPrimaryWeapon;
                            this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                        }
                        else if (canPickUp.gameObject.CompareTag("Rifle"))
                        {
                            //pick up grenade launcher
                            carriedPrimaryWeapon = "Rifle";
                            if (carriedSecondaryWeapon.Equals(null) || !selectedWeapon.Equals(carriedSecondaryWeapon))
                                //select weapon
                                selectedWeapon = carriedPrimaryWeapon;
                            this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        }
                        canPickUp = null;
                    }
                    else if (canPickUp.gameObject.CompareTag("Primary Ammo") && health < 75)
                    {
                        //besela: pick up animaton needed
                        Destroy(canPickUp.gameObject);
                        health += 25;
                        canPickUp = null;
                    }
                    else if (canPickUp.gameObject.CompareTag("Secondary Ammo") && health < 75)
                    {
                        //besela: pick up animaton needed
                        Destroy(canPickUp.gameObject);
                        health += 25;
                        canPickUp = null;
                    }
                }
                //switch weapons logic
                if (Input.GetKeyDown(KeyCode.Z) && !carriedPrimaryWeapon.Equals(null) && !carriedSecondaryWeapon.Equals(null))
                {
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
                    else if (selectedWeapon.Equals("Shotgun"))
                    {
                        //select shotgun
                        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                    }
                    else if (selectedWeapon.Equals("Rifle"))
                    {
                        //select rifle
                        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
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

        }
        else if(gameObject.CompareTag("Bangalor"))
        {

        }
        else if(gameObject.CompareTag("Bloodhound"))
        {

        }
        //IF ABILITY DURATION DONE (CODE STILL NOT IMPLEMENTED)
        abilityDeployed = false;
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
        health = health - amount < 0 ? 0: health - amount;

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

}
