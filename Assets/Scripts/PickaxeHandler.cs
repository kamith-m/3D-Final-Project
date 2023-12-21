using TMPro;
using UnityEngine;
public class PickaxeHandler : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI debugText; // This is for UI elements

    private int hitCounter = 0; // Counter for successful hits
    private float hitTimer = 0f; // Timer to enforce delay between hits
    private float hitDelay = 1f; // 1 second delay between hits

    private void Update()
    {
        // If hitTimer is greater than 0, countdown
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the pickaxe hit an ore and if the hitTimer is 0 or less (meaning 1 second has passed)
        if (other.CompareTag("Ores") && hitTimer <= 0)
        {
            hitCounter++; // Increment the hit counter
            hitTimer = hitDelay; // Reset the hit timer

            // Check if the hitCounter has reached 5 hits
            if (hitCounter >= 5)
            {
                playerController.SpawnMaterial();
                hitCounter = 0; // Reset the hit counter
            }

            // Optionally, update debug text to show hit count
            if (debugText != null)
            {
                debugText.text = "Hits: " + hitCounter;
            }
        }
    }
}

