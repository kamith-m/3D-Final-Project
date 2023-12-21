using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour
{

    public GameObject anvilFire;
    public GameObject weaponMoldPrefab;
    public Transform MoldSpawner;

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
        if (collision.gameObject.CompareTag("Bars"))
        {
            // Destroy the bars object
            Destroy(collision.gameObject);

            anvilFire.SetActive(true);
            // Call the function to spawn rods
        }
    }

    public void SpawnMold()
    {
        GameObject spawnedMold = Instantiate(weaponMoldPrefab, MoldSpawner.position, MoldSpawner.rotation);
        InitializeIngot(spawnedMold);
        anvilFire.SetActive(false);
    }

    void InitializeIngot(GameObject weaponMold)
    {
        weaponMold.SetActive(true);
        weaponMold.tag = "Mold";

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
