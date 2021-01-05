using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraScript : NetworkBehaviour
{

    public Transform playerBody;
    public float mouseSensitivity;

    float xAxisClamp = 0.0f;

    bool state = true;

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButtonDown(1))
            {
                state = false;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                state = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (state)
            {
                transform.LookAt(playerBody);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                RotateCamera();
            }
        }



    }

    void RotateCamera()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = MouseX * mouseSensitivity;
        float rotAmountY = MouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotCam = transform.rotation.eulerAngles;
        Vector3 targetRotBody = playerBody.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;
        targetRotCam.z = 0;
        targetRotBody.y += rotAmountX;

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotCam.x = 270;
        }


        transform.rotation = Quaternion.Euler(targetRotCam);
        playerBody.rotation = Quaternion.Euler(targetRotBody);
    }


}

