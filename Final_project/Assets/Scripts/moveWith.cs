using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWith : MonoBehaviour
{
    public GameObject player;
    public int x;
    public int y;
    public int z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
     void Update () {
     // Temporary vector
     Vector3 temp = player.transform.position;
     temp.x = temp.x - x;
     temp.y = temp.y - y;
     temp.z = temp.z - z;
     // Assign value to Camera position
     transform.position = temp;
    }
}
