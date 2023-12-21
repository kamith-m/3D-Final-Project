using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firepit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firepitsmoke;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleFirepit()
    {
        if (firepitsmoke != null)
        {
            bool isActive = firepitsmoke.activeSelf;

            if (!isActive)
            {
                firepitsmoke.SetActive(true);
            }
            else
            {
                firepitsmoke.SetActive(false);
            }
        }
    }
}
