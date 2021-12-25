using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    public CharacterController characController;
    // Start is called before the first frame update
    void Start()
    {
        characController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            characController.height = 0.5f;
        }
        else
        {
            characController.height = 1.8f;
        }
    }
}
