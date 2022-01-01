using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerAnimations : MonoBehaviour
{
    //Scripts
    FirstPersonController first_person_script;
    PlayerLogic player_script;

    //Animations
    public Animator anim;

    

    void Start()
    {
        first_person_script = GameObject.Find("Player").GetComponent<FirstPersonController>();
        player_script = GameObject.Find("Player").GetComponent<PlayerLogic>();

        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetBool("Crouch", first_person_script.m_Crouched);
        anim.SetBool("Armed", player_script.selectedWeapon != null);
    }
}
