using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Thrown Object"))
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            float x = other.gameObject.transform.position.x;
            float y = player.transform.position.y;
            float z = other.gameObject.transform.position.z;
            player.transform.position = new Vector3(x, y, z);
            cc.enabled = true;
            Destroy(other.gameObject);
        }
    }
}
