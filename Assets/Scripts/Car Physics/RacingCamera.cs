using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingCamera : MonoBehaviour
{
    public CarController carController;
    public Rigidbody subjectRb;

    public Vector3 positionSpeedPerAxis;
    public float overallPositionalSpeed;
    public Vector3 positionBounds;
    public float rotationSpeed;
    public float lookAhead;

    public Transform airborneCursor;

    public Vector3 localVelocity;

    private Vector3 targetRotation;

    private Vector3 targetPosition;

    private Vector3 vehiclePosition;

    public float forwardOffset;

    void Start()
    {
        transform.parent = null;
        GameObject airCursor = new GameObject();
        airborneCursor = airCursor.transform;
        airborneCursor.transform.name = "AirCursor";
        airborneCursor.transform.parent = null;
    }

    void FixedUpdate()
    {
        localVelocity = subjectRb.transform.InverseTransformDirection(subjectRb.velocity);

        targetRotation = subjectRb.transform.localEulerAngles;

/*        if (carController.isGrounded)
        {
            targetRotation = subjectRb.transform.localEulerAngles;
        }

        else
        {
            airborneCursor.LookAt(subjectRb.velocity.normalized * lookAhead + (carController.transform.forward * forwardOffset));
            targetRotation = airborneCursor.eulerAngles;
        }*/

        if(carController.isGrounded)
        {
            transform.localEulerAngles = new Vector3(
            Mathf.LerpAngle(transform.localEulerAngles.x, targetRotation.x, rotationSpeed * Time.deltaTime),
            Mathf.LerpAngle(transform.localEulerAngles.y, targetRotation.y, rotationSpeed * Time.deltaTime),
            Mathf.LerpAngle(transform.localEulerAngles.z, targetRotation.z, rotationSpeed * Time.deltaTime)
            );
        }
        else
        {
            transform.eulerAngles = new Vector3(
            Mathf.LerpAngle(transform.eulerAngles.x, transform.eulerAngles.x, rotationSpeed * Time.deltaTime),
            Mathf.LerpAngle(transform.eulerAngles.y, transform.eulerAngles.y, rotationSpeed * Time.deltaTime),
            Mathf.LerpAngle(transform.eulerAngles.z, 0, rotationSpeed * Time.deltaTime)
            );
        }

        

        vehiclePosition = Vector3.Slerp(vehiclePosition, subjectRb.transform.position, overallPositionalSpeed * Time.deltaTime);

        transform.position = vehiclePosition
            +
            (subjectRb.transform.right * Mathf.Clamp((localVelocity.x * positionSpeedPerAxis.x), -positionBounds.x, positionBounds.x))
            +
            (subjectRb.transform.up * Mathf.Clamp((Mathf.Abs(subjectRb.velocity.y) * positionSpeedPerAxis.y), 0, positionBounds.y))
            +
           (subjectRb.transform.forward * Mathf.Clamp((localVelocity.z * positionSpeedPerAxis.z), positionBounds.z, 0));
    }
}
