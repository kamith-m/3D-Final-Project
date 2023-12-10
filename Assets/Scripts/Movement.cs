using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    // using https://developer.oculus.com/documentation/unity/unity-ovrinput

    public float movementSpeed = 1.0f;
    public float rotationSpeed = 45.0f;

    public float snapTurnAmount = 45.0f;
    public float snapTurnTime = 0.1f;

    private bool isTurning = false;
    private float startYaw;
    private float endYaw;
    private float t = 1.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Snap Turn
        if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x > 0.9 && !isTurning)
        {
            isTurning = true;
            //transform.Rotate(Vector3.up, snapTurnAmount, Space.World);
            startYaw = transform.rotation.eulerAngles.y;
            endYaw = startYaw + snapTurnAmount;
            t = 0.0f;
        }
        else if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x < -0.9 && !isTurning)
        {
            isTurning = true;
            //transform.Rotate(Vector3.up, -snapTurnAmount, Space.World);
            startYaw = transform.rotation.eulerAngles.y;
            endYaw = startYaw - snapTurnAmount;
            t = 0.0f;
        }
        else if (Mathf.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x) < 0.1 && isTurning)
        {
            isTurning = false;
        }


        if (t < 1.0f)
        {
            t += Time.deltaTime / snapTurnTime;
            float yaw = Mathf.Lerp(startYaw, endYaw, t);
            Vector3 curRot = transform.rotation.eulerAngles;

            transform.rotation = Quaternion.Euler(curRot.x, yaw, curRot.z);
        }

        //Movement
        Vector2 movement = new Vector2(movementSpeed, movementSpeed);
        movement *= OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        movement *= Time.deltaTime;
        transform.Translate(new Vector3(movement.x, 0.0f, movement.y));

    }
}