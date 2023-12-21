using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grinder : MonoBehaviour
{
    private int hitCounter = 0; // Counter for hits
    private const int requiredHits = 5; // Number of hits required to transform the object
    public GameObject finalMoldPrefab; // Assign the prefab of the FinalMold in the inspector
    public Transform FinalMoldSpawner;

    public TextMeshProUGUI grinderCounter; // This is for UI elements

    //private PlayerController playerController; // Reference to the PlayerController

    private void Start()
    {
        // Get the PlayerController instance and store it for future use
        //playerController = FindObjectOfType<PlayerController>();
    }

    //private void OnCollisionEnter(Collision collision)
    //{

    //    // Check if the object colliding with the grinder is currently being held
    //    if (playerController != null && collision.gameObject == playerController.heldObject && collision.gameObject.CompareTag("Mold"))
    //    {
    //        hitCounter++; // Increment the hit counter

    //        if (hitCounter >= requiredHits)
    //        {
    //            playerController.TransformHeldObject(finalMoldPrefab);
    //            hitCounter = 0; // Reset the counter for the next object
    //        }

    //        if (grinderCounter != null)
    //        {
    //            grinderCounter.text = "Hits: " + hitCounter;
    //        }
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Bars" tag
        if (collision.gameObject.CompareTag("Mold"))
        {
            // Destroy the bars object
            Destroy(collision.gameObject);

            SpawnFinalMold();
            // Call the function to spawn rods
        }
    }

    public void SpawnFinalMold()
    {
        GameObject spawnedFinalMold = Instantiate(finalMoldPrefab, FinalMoldSpawner.position, FinalMoldSpawner.rotation);
        InitializeIngot(spawnedFinalMold);
    }

    void InitializeIngot(GameObject weaponMold)
    {
        weaponMold.SetActive(true);
        weaponMold.tag = "FinalMold";

        Rigidbody rb = weaponMold.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        BoxCollider collider = weaponMold.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = weaponMold.AddComponent<BoxCollider>();
        }
        collider.isTrigger = false;
    }

}
