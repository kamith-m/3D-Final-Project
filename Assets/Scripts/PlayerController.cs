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

    private int bellowsCounter = 0; // Counter for bellows interactions
    private bool isLaserActive = true; // State to manage laser activity

    public GameObject materialPrefab; // Prefab of the material to spawn
    public Transform spawnPoint; // Transform of the empty GameObject used as spawn point

    private float spawnCooldown = 0.5f; // Cooldown time in seconds between spawns
    private float lastSpawnTime = -1f; // Initialize to -1 so the first spawn can happen immediately


    void Start()
    {
        // Always enable the laser pointer
        laserPointer.enabled = true;
        laserPointer.material.color = defaultLaserColor;

        // Initial setup
        ActivateLaser(true);
        pickaxe.SetActive(false);
        pliers.SetActive(false);
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
        }

        float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (triggerValue > 0.9f && Time.time >= lastSpawnTime + spawnCooldown)
        {
            SpawnMaterial();
            lastSpawnTime = Time.time; // Update the time of the last spawn
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
                Firepit firepitsmoke = hit.collider.GetComponent<Firepit>();
                if (firepitsmoke != null && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
                {
                    bellowsCounter++; // Increment the counter
                    BellowsText.text = "Count: " + bellowsCounter;
                    if (bellowsCounter < 5) 
                    {
                        firepitsmoke.ToggleFirepit();
                        firepitsmoke.ToggleFirepit(); 
                    }

                    // Check if it's the 5th or 6th interaction
                    if (bellowsCounter == 5 || bellowsCounter == 6)
                    {
                        firepitsmoke.ToggleFirepit(); // Trigger the method

                        // Reset the counter after the 6th interaction
                        if (bellowsCounter == 6)
                        {
                            bellowsCounter = 0;
                            BellowsText.text = "Count: " + bellowsCounter;
                        }
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
            }
            else if (hit.collider.CompareTag("Pliers") && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                laserPointer.material.color = interactionLaserColor;
                ActivateLaser(false); // Deactivate laser
                pliers.SetActive(true); // Show pliers
                pickaxe.SetActive(false); // Ensure pickaxe is hidden
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
        spawnedMaterial.SetActive(true); // Activate the spawned object

        // Add a Rigidbody to enable physics interactions
        Rigidbody rb = spawnedMaterial.AddComponent<Rigidbody>();

        // Add a Collider to the spawned object
        // Use the appropriate type of collider, e.g., BoxCollider, SphereCollider, etc.
        spawnedMaterial.AddComponent<BoxCollider>(); // Example: Adding a BoxCollider

        lastSpawnTime = Time.time; // Update the time of the last spawn
    }

    public void HandlePickaxeTrigger(Collider other)
    {
        if (other.CompareTag("Ores"))
        {
            SpawnMaterial();
        }
    }

}