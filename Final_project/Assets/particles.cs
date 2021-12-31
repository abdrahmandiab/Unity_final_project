using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particles : MonoBehaviour
{
    ParticleSystem part;
    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("p"))
        {
            part.Play();
            Destroy(gameObject, part.main.duration);
        }
    }
}
