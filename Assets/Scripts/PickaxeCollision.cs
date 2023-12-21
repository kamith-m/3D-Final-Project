using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeCollision : MonoBehaviour
{ 
    public PlayerController playerController;

    void OnTriggerEnter(Collider other)
    {
        playerController.HandlePickaxeTrigger(other);
    }
}
