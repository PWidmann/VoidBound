using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    float mouseSensitivity = 300f;

    public Transform playerBody;
    float mouseX;
    float mouseY;

    float xRotation = 0f;

    Settings settings;

    // Start is called before the first frame update
    void Start()
    {
        settings = new Settings();
    }

    private void LateUpdate()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * settings.mouseSensitivity * Time.smoothDeltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * settings.mouseSensitivity * Time.smoothDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
 
}
