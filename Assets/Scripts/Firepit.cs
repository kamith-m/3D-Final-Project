using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firepit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firepitsmoke;
    public GameObject Red_IngotPrefab; // Assign this in the inspector
    public Transform Barspawner; // Assign the Barspawner object in the inspector
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
        if (collision.gameObject.CompareTag("Bars"))
        {
            // Destroy the bars object
            Destroy(collision.gameObject);
        }
    }

    public void ToggleFirepit()
    {
        if (firepitsmoke != null)
        {
            bool isActive = firepitsmoke.activeSelf;

            if (!isActive)
            {
                firepitsmoke.SetActive(true);
            }
            else
            {
                firepitsmoke.SetActive(false);
            }
        }
    }
        
    public void SpawnBars()
    {
        GameObject spawnedIngot = Instantiate(Red_IngotPrefab, Barspawner.position, Barspawner.rotation);
        InitializeIngot(spawnedIngot);
    }

    void InitializeIngot(GameObject ingot)
    {
        ingot.SetActive(true);
        ingot.tag = "Bars"; // Assuming you want to tag it as "Bars"

        Rigidbody rb = ingot.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        BoxCollider collider = ingot.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = ingot.AddComponent<BoxCollider>();
        }
        collider.isTrigger = false;
    }

}
