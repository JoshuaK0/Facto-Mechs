using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSuspension : MonoBehaviour
{
    public Transform skidMarks;
    public CarController carController;
    public Rigidbody rb;
    public Transform wheelPivot;
    public Transform wheelModel;
    public Vector3 wheelModelPosition;
    public float wheelSpeed;
    public float wheelSusSpeed;

    public bool isGrounded;

    [Header("Suspension")]
    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springForce;
    private float damperForce;
    private float springVelocity;

    private Vector3 suspensionForce;

    [Header("Wheel")]
    public float maxSpeed;
    public float wheelRadius;
    public float wheelWidth;
    
    private float wheelAngle;
    public float slipRatio;

    private Vector3 localWheelVelocity;

    
    
    public float friction;

    private float sidewaysForce;
    private float forwardForce;
    public float steerAngle;

    public AnimationCurve driftGripCurve;



    void Start()
    {
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, carController.steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            if(slipRatio <= carController.driftThreshold)
            {
                skidMarks.gameObject.SetActive(true);
            }
            else
            {
                skidMarks.gameObject.SetActive(false);
            }
            skidMarks.position = hit.point;
            isGrounded = true;
            lastLength = springLength;

            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;
            suspensionForce = (springForce + damperForce) * transform.up;

            wheelModel.localPosition = Vector3.Slerp(wheelModel.localPosition, wheelModelPosition + new Vector3(0, -springLength, 0), wheelSusSpeed * Time.deltaTime);


            Vector3 up = hit.normal;
            //Make sure the velocity is normalized
            Vector3 vel = rb.transform.forward.normalized;
            //Project the two vectors using the dot product
            Vector3 forward = vel - up * Vector3.Dot(vel, up);

            //Set the rotation with relative forward and up axes
            Quaternion rot = Quaternion.LookRotation(forward.normalized, up);
            wheelPivot.rotation = Quaternion.Slerp(wheelModel.rotation, rot, wheelSpeed * Time.deltaTime);
            skidMarks.rotation = rot;



            localWheelVelocity = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            slipRatio = Mathf.Abs(localWheelVelocity.z) / (Mathf.Abs(localWheelVelocity.x) + Mathf.Abs(localWheelVelocity.z));

            forwardForce = Input.GetAxis("Vertical") * carController.driveForce * carController.torqueCurve.Evaluate(Mathf.InverseLerp(0, maxSpeed, rb.velocity.magnitude));
            if(slipRatio != float.NaN)
            {
                sidewaysForce = localWheelVelocity.x * carController.grip * carController.gripCurve.Evaluate(Mathf.Clamp((1 - slipRatio), 0, 1));
            }
            else
            {
                sidewaysForce = 1;
            }
            
            rb.AddForceAtPosition(forwardForce * Vector3.Cross(transform.right, hit.normal), hit.point, ForceMode.Acceleration);
            
            Vector3 antiSlip = -sidewaysForce * transform.right;
            Vector3 frictionVector = -localWheelVelocity.z * friction * transform.forward;

            float speedControl = Mathf.Max(0, localWheelVelocity.z - maxSpeed);
            rb.AddForceAtPosition(speedControl * -transform.forward, hit.point, ForceMode.Acceleration);

            rb.AddForceAtPosition(suspensionForce, hit.point, ForceMode.Acceleration);
            
            rb.AddForceAtPosition(antiSlip, hit.point, ForceMode.Acceleration);
            rb.AddForceAtPosition(frictionVector, hit.point, ForceMode.Acceleration);
        }
        else
        {
            wheelPivot.localRotation = Quaternion.Slerp(wheelModel.localRotation, Quaternion.identity, wheelSpeed * Time.deltaTime);
            wheelModel.localPosition = Vector3.Slerp(wheelModel.localPosition, wheelModelPosition + new Vector3(0, -maxLength, 0), wheelSusSpeed * Time.deltaTime);
            isGrounded = false;
            rb.AddForce(-Vector3.up * carController.airborneDownforce * Time.deltaTime, ForceMode.Acceleration);
        }

    }

    public void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;

        Gizmos.DrawCube(Vector3.zero, new Vector3(wheelWidth, 0.001f, wheelRadius * 2f));
        Gizmos.DrawCube(-Vector3.up * (restLength + springTravel + wheelRadius), new Vector3(wheelWidth, 0.001f, wheelRadius * 2f));
        Gizmos.DrawCube(new Vector3(0, (restLength + springTravel + wheelRadius) / -2, wheelRadius), new Vector3(wheelWidth, restLength + springTravel + wheelRadius, 0.001f));
        Gizmos.DrawCube(new Vector3(0, (restLength + springTravel + wheelRadius) / -2, -wheelRadius), new Vector3(wheelWidth, restLength + springTravel + wheelRadius, 0.001f));

        Gizmos.matrix = Matrix4x4.identity;
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            Gizmos.DrawRay(transform.position + transform.up * -hit.distance, Vector3.Cross(transform.right, hit.normal).normalized * carController.wheelBase / 5f);
        }
    }
}