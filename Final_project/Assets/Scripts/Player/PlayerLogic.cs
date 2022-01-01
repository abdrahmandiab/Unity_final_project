using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerLogic : MonoBehaviour
{
    public int health;
    public int specialAbilitiesMeter;
    private float lastUpdate_SpecialAbilitiesMeter;
    private Collider canPickUp;
    public bool pickingUp; // for animations
    public string selectedWeapon;
    private string carriedPrimaryWeapon;
    private string carriedSecondaryWeapon;
    private int primaryAmmoCount;
    private int secondaryAmmoCount;
    private int noEnemyShot;
    private bool isGameOver, isPaused;
    GameObject player;

    //selected legend
    string selectedCharacter;
    GameObject playerModel;

    //Animations
    private Animator playerAnim;

    //Scripts
    PlayerAnimations playeraAnimScript;
    FirstPersonController first_person_script;

    // Prehabs: to implement drop weapons logic
    public GameObject[] weapons = new GameObject[5];
    //0 is Grenade Launcher
    //1 is Sniper
    //2 is Shotgun
    //3 is Rifle
    //4 is Fire Grenade Launcher

    //cameras
    Camera firstPerson;
    Camera thirdPerson;


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

        // carriedPrimaryWeapon = null;
        carriedPrimaryWeapon = null;
        carriedSecondaryWeapon = null;
        selectedWeapon = null;
        selectedCharacter = "Bloodhound"; //TODO: Change based on selected character

        primaryAmmoCount = 0;
        secondaryAmmoCount = 0;
        pickingUp = false;

        //get player
        player = GameObject.Find("Player");
        playerModel = GameObject.FindGameObjectWithTag(selectedCharacter);

        //script init
        first_person_script = player.GetComponent<FirstPersonController>();
        playeraAnimScript = playerModel.GetComponent<PlayerAnimations>();

        //animators init
        playerAnim = playerModel.GetComponent<Animator>();

        //cameras
        firstPerson = first_person_script.m_Camera;
        thirdPerson = GameObject.FindGameObjectWithTag("3rd Person").GetComponent<Camera>();
        firstPerson.enabled = true;
        thirdPerson.enabled = false;
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
                if (specialAbilitiesMeter == 100 && Input.GetKeyDown(KeyCode.Q))
                {
                    executeSpecialAbility();
                    specialAbilitiesMeter = 0;
                }

                // Execute special ability



                //pick up logic
                if (canPickUp != null && Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(pickUp());
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

            //model crouch fix
            if (first_person_script.m_Crouched)
            {
                playerModel.transform.position = new Vector3(playerModel.transform.position.x,
                                                             player.transform.position.y - 0.4f,
                                                             playerModel.transform.position.z);
            }
            else
            {
                playerModel.transform.position = new Vector3(playerModel.transform.position.x,
                                                             player.transform.position.y - 0.98f,
                                                             playerModel.transform.position.z);
            }
        }
    }

    private IEnumerator pickUp()
    {

        if (first_person_script.m_Crouched)
        {
            playerAnim.Play("Pick Up Crouching");
            yield return new WaitForSeconds(1);
            playerAnim.Play("Rifle Crouching Aiming Idle");
        }
        else
        {
            playerAnim.Play("Pick Up Standing");
            yield return new WaitForSeconds(1.8f);
            playerAnim.Play("Rifle Aiming Idle");
        }


        if (canPickUp.gameObject.CompareTag("Health Pack"))
        {
            //besela: pick up animaton needed
            Destroy(canPickUp.gameObject);
            health = health + 25 > 100 ? 100 : health + 25;
            canPickUp = null;
        }
        else if (canPickUp.gameObject.CompareTag("Grenade Launcher") || canPickUp.gameObject.CompareTag("Fire Launcher"))
        {
            if (carriedSecondaryWeapon != null)
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
                    gameObject = (GameObject)Instantiate(weapons[4]);
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
                if (carriedPrimaryWeapon == null || !selectedWeapon.Equals(carriedPrimaryWeapon))
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
            secondaryAmmoCount = (secondaryAmmoCount + 2) > 5 ? 5 : (secondaryAmmoCount + 2);
            canPickUp = null;
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
        selectedWeapon = carriedPrimaryWeapon;
        if (selectedWeapon.Equals("Sniper")){
            //select sniper
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

            Sniper.goBeastMode();
        }
        else if (selectedWeapon.Equals("Shotgun"))
        {
            //select shotgun
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

            Shotgun.goBeastMode();
        }
        else if (selectedWeapon.Equals("Rifle"))
        {
            //select rifle
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

            Rifle.goBeastMode();
        }

        }
        //IF ABILITY DURATION DONE (CODE STILL NOT IMPLEMENTED)
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

        //animation
        thirdPersonAnim(false);

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
        if (health <= 0)
        {         
            gameOver();
        }

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

    public void thirdPersonAnim(bool win)
    {
        firstPerson.enabled = false;
        thirdPerson.enabled = true;
        playerAnim.SetLayerWeight(1, 0);
        playerAnim.applyRootMotion = true;

        if(win)
            playerAnim.Play("Celebrate");
        else
            playerAnim.Play("Die");
    }

}
