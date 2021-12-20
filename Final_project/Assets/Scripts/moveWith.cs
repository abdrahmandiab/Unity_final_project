using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class moveWith : MonoBehaviour
{
    public GameObject player;
    private Vector3 myTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform.position;
    }

    // Update is called once per frame
     void Update () {
     // Temporary vector
     Vector3 temp = player.transform.position;
     temp.x = temp.x + myTransform.x;
     temp.y = myTransform.y;
     temp.z = temp.z + myTransform.z;
     // Assign value to Camera position

    transform.position = temp;
    }
}
