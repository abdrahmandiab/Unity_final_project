
using UnityEngine;

public class explosionScript : MonoBehaviour
{
    public float timer = 1f;


    // Update is called once per frame
    void Update()
    {
        if(Time.time>=timer){
            Destroy(gameObject);
        }
    }
}
