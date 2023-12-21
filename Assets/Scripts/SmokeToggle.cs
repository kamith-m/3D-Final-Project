using UnityEngine;

public class SmokeToggle : MonoBehaviour
{
    public GameObject smokeObject; // Assign this in the inspector

    // Update is called once per frame
    void Update()
    {
        // Check if the A button on the right controller is pressed
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            // If the smoke object is assigned, toggle its active state
            if (smokeObject != null)
            {
                smokeObject.SetActive(!smokeObject.activeSelf);
            }
        }
    }
}
