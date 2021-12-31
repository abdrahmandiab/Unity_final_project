using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }  
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("7aseb yasta");
        if (other.gameObject == Player) {
            other.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("teslam");
        if (other.gameObject == Player)
        {
            other.transform.parent = null;
        }
    }
}
