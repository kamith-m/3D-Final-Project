using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBucket : MonoBehaviour
{
    public GameObject weaponPrefab; // Assign the prefab of the FinalMold in the inspector
    public Transform WeaponSpawner;

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
        if (collision.gameObject.CompareTag("PreCooled"))
        {
            // Destroy the bars object
            Destroy(collision.gameObject);

            SpawnWeapon();
        }
    }

    public void SpawnWeapon()
    {
        GameObject spawnedWeapon = Instantiate(weaponPrefab, WeaponSpawner.position, WeaponSpawner.rotation);
        InitializeIngot(spawnedWeapon);
    }

    void InitializeIngot(GameObject weapon)
    {
        weapon.SetActive(true);
        weapon.tag = "Weapon";

        Rigidbody rb = weapon.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        BoxCollider collider = weapon.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = weapon.AddComponent<BoxCollider>();
        }
        collider.isTrigger = false;
    }
}
