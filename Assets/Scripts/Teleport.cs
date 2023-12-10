using UnityEngine;

public class Teleport : MonoBehaviour
{
    public LineRenderer laserPointer;
    public float maxLaserDistance = 100f;
    public Color defaultLaserColor = Color.red;
    public Color teleportationLaserColor = Color.green;
    public Transform cameraRig;
    public Transform rightHandAnchor;
    public float playerHeight = 2.0f;
    public LayerMask teleportationLayer;
    public GameObject directionIndicator; 
    private Quaternion targetRotation;
    public float rotationSpeed = 5f; 

    private bool isLaserVisible = false;

    void Start()
    {
        laserPointer.enabled = false;
        laserPointer.material.color = defaultLaserColor;
    }
    void Update()
    {
        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        // Show laser when thumbstick is pressed forward
        if (thumbstickInput.y > 0.9f)
        {
            isLaserVisible = true;
            laserPointer.enabled = true;

            // Make the direction indicator visible
            if (!directionIndicator.activeSelf)
            {
                directionIndicator.SetActive(true);
            }
        }
        else if (isLaserVisible)
        {
            // Check for teleportation when releasing the thumbstick
            HandleTeleportation();
            isLaserVisible = false;
            laserPointer.enabled = false;
            laserPointer.material.color = defaultLaserColor; // Reset laser color when not visible

            // Hide the direction indicator
            directionIndicator.SetActive(false);
        }

        if (isLaserVisible)
        {
            UpdateLaserPointer();
        }

        if (isLaserVisible && directionIndicator.activeSelf)
        {
            float thumbstickRotationInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
            targetRotation = Quaternion.Euler(0f, thumbstickRotationInput * 360f, 0f);
            directionIndicator.transform.rotation = Quaternion.Lerp(directionIndicator.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void UpdateLaserPointer()
    {
        RaycastHit hit;
        bool hitTeleportationTarget = Physics.Raycast(rightHandAnchor.position, rightHandAnchor.forward, out hit, maxLaserDistance, teleportationLayer) && hit.collider.CompareTag("Portal");

        // Update laser pointer position
        laserPointer.SetPosition(0, rightHandAnchor.position);
        laserPointer.SetPosition(1, hitTeleportationTarget ? hit.point : rightHandAnchor.position + rightHandAnchor.forward * maxLaserDistance);

        // Change laser color based on whether a teleportation target is hit    
        laserPointer.material.color = hitTeleportationTarget ? teleportationLaserColor : defaultLaserColor;

        // Activate the direction indicator if a valid teleportation target is hit
        directionIndicator.SetActive(hitTeleportationTarget);
        if (hitTeleportationTarget)
        {
            directionIndicator.transform.position = hit.point + new Vector3(0, 0.05f, 0); // Slightly above the hit point to avoid z-fighting
            directionIndicator.transform.forward = hit.normal; // Align with the hit surface
        }
    }

    void HandleTeleportation()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightHandAnchor.position, rightHandAnchor.forward, out hit, maxLaserDistance, teleportationLayer) && hit.collider.CompareTag("Portal"))
        {
            Vector3 teleportPosition = hit.point;
            teleportPosition.y = hit.point.y + playerHeight; // Adjust the height to account for the player's height

            // Move the camera rig to the teleport position
            cameraRig.position = teleportPosition;

            // Rotate the camera rig to face the direction indicated by the directionIndicator
            Quaternion finalRotation = Quaternion.Euler(0f, directionIndicator.transform.eulerAngles.y, 0f);
            cameraRig.rotation = finalRotation;

            // Hide the direction indicator after teleportation
            directionIndicator.SetActive(false);
        }
    }

}



/*OLD CODE
 * 
 * //using UnityEngine;

//public class Teleport : MonoBehaviour
//{
//    public LineRenderer laserPointer;
//    public float maxLaserDistance = 100f;
//    public Color defaultLaserColor = Color.red;
//    public Color teleportationLaserColor = Color.green;
//    public Transform cameraRig;
//    public Transform rightHandAnchor;
//    public float playerHeight = 2.0f;
//    public LayerMask teleportationLayer;

//    private bool isLaserVisible = false;

//    void Start()
//    {
//        laserPointer.enabled = false;
//        laserPointer.material.color = defaultLaserColor;
//    }
//    void Update()
//    {
//        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

//        // Show laser when thumbstick is pressed forward
//        if (thumbstickInput.y > 0.9f)
//        {
//            isLaserVisible = true;
//            laserPointer.enabled = true;
//        }
//        else if (isLaserVisible)
//        {
//            // Check for teleportation when releasing the thumbstick
//            HandleTeleportation();
//            isLaserVisible = false;
//            laserPointer.enabled = false;
//            laserPointer.material.color = defaultLaserColor; // Reset laser color when not visible
//        }

//        if (isLaserVisible)
//        {
//            UpdateLaserPointer();
//        }
//    }

//    void UpdateLaserPointer()
//    {
//        RaycastHit hit;
//        bool hitTeleportationTarget = Physics.Raycast(rightHandAnchor.position, rightHandAnchor.forward, out hit, maxLaserDistance, teleportationLayer) && hit.collider.CompareTag("Portal");

//        // Update laser pointer position
//        laserPointer.SetPosition(0, rightHandAnchor.position);
//        laserPointer.SetPosition(1, hitTeleportationTarget ? hit.point : rightHandAnchor.position + rightHandAnchor.forward * maxLaserDistance);

//        // Change laser color based on whether a teleportation target is hit    
//        laserPointer.material.color = hitTeleportationTarget ? teleportationLaserColor : defaultLaserColor;
//    }

//    void HandleTeleportation()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(rightHandAnchor.position, rightHandAnchor.forward, out hit, maxLaserDistance, teleportationLayer) && hit.collider.CompareTag("Portal"))
//        {
//            Vector3 teleportPosition = hit.point;
//            teleportPosition.y = hit.point.y + playerHeight; // Adjust the height to account for the player's height
//            cameraRig.position = teleportPosition; // Move the camera rig to the teleport position
//        }
//    }

//}

 * 
 * 
 */