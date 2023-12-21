using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOre : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Bars" tag
        if (collision.gameObject.CompareTag("Ores"))
        {
            // Destroy the bars object
            Destroy(collision.gameObject);
        }
    }
}
