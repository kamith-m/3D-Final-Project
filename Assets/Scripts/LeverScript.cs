using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public GameObject waterSmoke; // Assign the Water_Smoke object in the inspector
    public void ToggleWaterSmoke()
    {
        if (waterSmoke != null)
        {
            waterSmoke.SetActive(!waterSmoke.activeSelf); // Toggle the visibility state
        }
    }
}
