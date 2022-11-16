using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cameraHolder;
    public Transform cameraTransform;
    public float mouseSensitivity;

    private Vector2 rotation;

    static bool Focused
    {
        get => Cursor.lockState == CursorLockMode.Locked;
        set
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = value == false;
        }
    }

    void OnEnable()
    {
        Focused = true;
    }

    void OnDisable() => Focused = false;


    void Update()
    {
        rotation.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotation.x = Mathf.Clamp(rotation.x -= Input.GetAxis("Mouse Y") * mouseSensitivity, -90, 90);
        cameraHolder.eulerAngles = new Vector3(0, rotation.y, 0);
        cameraTransform.localEulerAngles = new Vector3(rotation.x, 0, 0);

        if(Input.GetMouseButtonDown(0))
        {
            Focused = true;
        }
        // Leave cursor lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Focused = false;
        }
    }
}
