using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretControls : MonoBehaviour
{
    [SerializeField]
    Transform CameraHolder;
    [SerializeField]
    Transform CameraZoomer;
    [SerializeField]
    bool seperateCam;
    [SerializeField]
    float turnSpeed;
    [SerializeField]
    float turretMoveSpeed;
    [SerializeField]
    float turretSmoothing;

    [SerializeField]
    float zoomSpeed;
    [SerializeField]
    float cameraSmoothing;

    float turretTarget;
    float cameraDist = 1;

    [SerializeField]
    Vector2 cameraBounds;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float rotY = 0;
        float rotX = 0;

        rotY = Input.GetAxis("Mouse Y");
        rotX = Input.GetAxis("Mouse X");

        if (seperateCam)
        {
            CameraHolder.localEulerAngles += (Vector3.up * rotX * turnSpeed * Time.deltaTime);
            turretTarget = Mathf.MoveTowardsAngle(turretTarget, CameraHolder.localEulerAngles.y, turretMoveSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3
                (
                    0,
                    Mathf.LerpAngle(transform.localEulerAngles.y, turretTarget, turretSmoothing * Time.deltaTime),
                    0
                );
        }
        else
        {
            CameraHolder.localEulerAngles += (Vector3.up * rotX * turnSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3
                (
                    0,
                    Mathf.LerpAngle(transform.localEulerAngles.y, CameraHolder.localEulerAngles.y, turretSmoothing * Time.deltaTime),
                    0
                );
        }

        CameraHolder.localEulerAngles = new Vector3
            (
            CameraHolder.localEulerAngles.x + (rotY * turnSpeed * Time.deltaTime),
            CameraHolder.localEulerAngles.y,
            0);

        cameraDist = Mathf.Clamp((cameraDist + (-Input.mouseScrollDelta.y * zoomSpeed * cameraDist)), -cameraBounds.y, -cameraBounds.x);

        CameraZoomer.localPosition = new Vector3
            (
                0,
                0,
                Mathf.Lerp(CameraZoomer.localPosition.z, cameraDist, cameraSmoothing * Time.deltaTime)
            );
    }
}
