using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    public GameObject waterSmoke; // Assign the Water_Smoke object in the inspector
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleWaterSmoke()
    {
        if (waterSmoke != null)
        {
            bool isActive = waterSmoke.activeSelf;

            if (!isActive)
            {
                waterSmoke.SetActive(true);
                anim.SetBool("LeverUp", true);
            }
            else
            {
                waterSmoke.SetActive(false);
                anim.SetBool("LeverUp", false);
            }
        }
    }
}
