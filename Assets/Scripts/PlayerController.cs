using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public LineRenderer laserPointer;
    public float maxLaserDistance = 100f;
    public Color defaultLaserColor = Color.red;
    public Color interactionLaserColor = Color.green; // Color when hitting interactable object
    public Transform rightHandAnchor;

    private GameObject currentTarget; // Current target object the laser is hitting
    public TextMeshProUGUI debugText; // This is for UI elements
    public TextMeshProUGUI BellowsText; // This is for UI elements

    public GameObject pickaxe;
    public GameObject pliers;
    public GameObject hammer;

    private int bellowsCounter = 0; // Counter for bellows interactions
    private bool isLaserActive = true; // State to manage laser activity

    public GameObject materialPrefab; // Prefab of the material to spawn
    public Transform spawnPoint; // Transform of the empty GameObject used as spawn point

    private float spawnCooldown = 0.5f; // Cooldown time in seconds between spawns
    private float lastSpawnTime = -1f; // Initialize to -1 so the first spawn can happen immediately

    public GameObject heldObject = null;
    private float pickUpRange = 4f;


    void Start()
    {
        // Always enable the laser pointer
        laserPointer.enabled = true;
        laserPointer.material.color = defaultLaserColor;

        // Initial setup
        ActivateLaser(true);
        pickaxe.SetActive(false);
        pliers.SetActive(false);
        hammer.SetActive(false);
        BellowsText.text = "Count: 0";
    }

    void Update()
    {
        UpdateLaserPointer();

        // Check for B button press to toggle laser and hide tools
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            ActivateLaser(true);
            pickaxe.SetActive(false);
            pliers.SetActive(false);
            hammer.SetActive(false);
        }

        if (laserPointer.enabled)
        {
            RaycastHit hit;
            if (Physics.Raycast(rightHandAnchor.position, rightHandAnchor.forward, out hit, pickUpRange))
            {
                if (hit.collider.CompareTag("Ores") && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.9f)
                {
                    if (heldObject == null)
                    {
                        PickUpObject(hit.collider.gameObject);
                    }
                }
            }

            if (heldObject != null && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) < 0.1f)
            {
                DropObject();
            }
        }

        if (pliers.activeSelf && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.9f)
        {
            if (heldObject == null)
            {
                // Attempt to pick up an object if the pliers are close to it
                AttemptPickUp();
            }
        }
        else if (heldObject != null)
        {
            // Drop the object when the trigger is released
            DropObject();
        }
    }

    void UpdateLaserPointer()
    {
        if (!isLaserActive) return; 

        // Cast a ray from the right hand anchor in the direction it's pointing
        RaycastHit hit;
        if (Physics.Raycast(rightHandAnchor.position, rightHandAnchor.forward, out hit, maxLaserDistance))
        {
            laserPointer.SetPosition(0, rightHandAnchor.position);
            laserPointer.SetPosition(1, hit.point); // Set the end of the laser to the point it hits

            // Check if the ray hits an object with the 'Lever' tag
            if (hit.collider.CompareTag("Lever"))
            {
                laserPointer.material.color = interactionLaserColor;
                Lever lever = hit.collider.GetComponent<Lever>();
                if (lever != null && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
                {
                    lever.ToggleWaterSmoke();
                    //debugText.text = "Hit Lever: " + hit.collider.name; // Update the UI text
                }
                else
                {
                    //debugText.text = "Lever script not found on: " + hit.collider.name;
                }
            }
            else if (hit.collider.CompareTag("Bellows"))
            {
                laserPointer.material.color = interactionLaserColor;
                Firepit firepit = hit.collider.GetComponent<Firepit>();
                if (firepit != null && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
                {
                    bellowsCounter++; // Increment the counter
                    BellowsText.text = "Count: " + bellowsCounter;

                    // Check if it's the 5th or 6th interaction
                    if (bellowsCounter == 5)
                    {
                        firepit.ToggleFirepit(); // Trigger the method
                        firepit.SpawnBars();
                    }
                    else if (bellowsCounter == 6)
                    {
                        firepit.ToggleFirepit(); // Trigger the method

                        bellowsCounter = 0;
                        BellowsText.text = "Count: " + bellowsCounter;
                    }

                    //debugText.text = "Bellows activated " + bellowsCounter + " times";
                }
            }
            else if (hit.collider.CompareTag("Pickaxe") && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                laserPointer.material.color = interactionLaserColor;
                ActivateLaser(false); // Deactivate laser
                pickaxe.SetActive(true); // Show pickaxe
                pliers.SetActive(false); // Ensure pliers are hidden
                hammer.SetActive(false);
            }
            else if (hit.collider.CompareTag("Pliers") && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                laserPointer.material.color = interactionLaserColor;
                ActivateLaser(false); // Deactivate laser
                pliers.SetActive(true); // Show pliers
                pickaxe.SetActive(false); // Ensure pickaxe is hidden
                hammer.SetActive(false);
            }
            else if (hit.collider.CompareTag("Hammer") && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                laserPointer.material.color = interactionLaserColor;
                ActivateLaser(false); // Deactivate laser
                pliers.SetActive(false); // Ensure pliers is hidden
                pickaxe.SetActive(false); // Ensure pickaxe is hidden
                hammer.SetActive(true);
            }
            else 
            {
                laserPointer.material.color = defaultLaserColor;
                currentTarget = null;
            }
        }
        else
        {
            laserPointer.SetPosition(0, rightHandAnchor.position);
            laserPointer.SetPosition(1, rightHandAnchor.position + rightHandAnchor.forward * maxLaserDistance);
            laserPointer.material.color = defaultLaserColor;
            currentTarget = null;
            ResetLaser();
        }
    }

    void ActivateLaser(bool activate)
    {
        isLaserActive = activate;
        laserPointer.enabled = activate;
        laserPointer.material.color = activate ? defaultLaserColor : Color.clear;
    }

    void ResetLaser()
    {
        if (!isLaserActive) return;

        laserPointer.SetPosition(0, rightHandAnchor.position);
        laserPointer.SetPosition(1, rightHandAnchor.position + rightHandAnchor.forward * maxLaserDistance);
        laserPointer.material.color = defaultLaserColor;
        currentTarget = null;
    }


    public void SpawnMaterial()
    {
        GameObject spawnedMaterial = Instantiate(materialPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedMaterial.SetActive(true);

        // Tag the spawned object so it can be interacted with by the laser
        spawnedMaterial.tag = "Ores";

        // Initialize the Rigidbody for the spawned object
        Rigidbody rb = spawnedMaterial.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false; // Make sure the object is not kinematic so it can be picked up and dropped

        // Initialize the Collider for the spawned object
        BoxCollider collider = spawnedMaterial.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = spawnedMaterial.AddComponent<BoxCollider>();
        }
        collider.isTrigger = false; // Make sure the collider is not a trigger so it can be picked up

        lastSpawnTime = Time.time;
    }
    void AttemptPickUp()
    {
        Collider[] hitColliders = Physics.OverlapSphere(pliers.transform.position, pickUpRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ores") || hitCollider.CompareTag("Bars") || hitCollider.CompareTag("Mold") || hitCollider.CompareTag("FinalMold") || hitCollider.CompareTag("PreCooled") || hitCollider.CompareTag("Weapon"))
            {
                PickUpObject(hitCollider.gameObject);
                break; // Only pick up the first object found
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        // Disable the object's Rigidbody so it doesn't fall while we're holding it
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        // Parent the object to the right hand anchor so it moves with the laser
        obj.transform.SetParent(rightHandAnchor);
        heldObject = obj;
    }
    void DropObject()
    {
        // Re-enable the object's Rigidbody so it falls naturally
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        // Unparent the object so it no longer moves with the laser
        heldObject.transform.SetParent(null);
        heldObject = null;
    }

    public void HandlePickaxeTrigger(Collider other)
    {
        if (other.CompareTag("Ores"))
        {
            SpawnMaterial();
        }
    }

    // Call this method to transform the currently held object
    public void TransformHeldObject(GameObject newPrefab)
    {
        if (heldObject != null && heldObject.CompareTag("Mold"))
        {
            // Instantiate the new object at the same position and rotation as the old one
            GameObject newObject = Instantiate(newPrefab, heldObject.transform.position, heldObject.transform.rotation);

            newObject.tag = "FinalMold";

            // Optionally transfer any other components or properties from the old object to the new one

            // Ensure the new object is now being held
            newObject.transform.SetParent(rightHandAnchor);
            newObject.GetComponent<Rigidbody>().isKinematic = true;

            // Destroy the old object
            Destroy(heldObject);

            // Update the reference to the held object
            heldObject = newObject;
        }
    }

}