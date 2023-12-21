using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forge : MonoBehaviour
{

    public GameObject precooledPrefab; // Assign the prefab of the FinalMold in the inspector
    public Transform PreCooledSpawner;

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
        if (collision.gameObject.CompareTag("FinalMold"))
        {
            // Destroy the bars object
            Destroy(collision.gameObject);

            SpawnPreCooled();
        }
    }

    public void SpawnPreCooled()
    {
        GameObject spawnedPreCooled = Instantiate(precooledPrefab, PreCooledSpawner.position, PreCooledSpawner.rotation);
        InitializeIngot(spawnedPreCooled);
    }

    void InitializeIngot(GameObject preCooled)
    {
        preCooled.SetActive(true);
        preCooled.tag = "PreCooled";

        Rigidbody rb = preCooled.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        BoxCollider collider = preCooled.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = preCooled.AddComponent<BoxCollider>();
        }
        collider.isTrigger = false;
    }
}
