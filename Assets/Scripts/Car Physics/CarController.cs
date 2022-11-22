using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Components")]
    private WheelSuspension leftSteeringWheel;
    private WheelSuspension rightSteeringWheel;
    public Rigidbody carRb;
    public List<WheelSuspension> wheels = new List<WheelSuspension>();
    public bool isGrounded;

    public float slipRatio;
    public float driftTorque;
    public float driftHandling;
    public float driftStopping;
    private float driftFriction;
    public float driftBonus;
    private float driftBonusCurrent;

    public float airControl;
    public float airControlDelay;
    private float currentAirControl = 0f;

    [Header("Car Specs")]

    public float airborneDownforce;
    public float wheelBase;
    public float rearTrack;

    public List<CarAxle> carAxles = new List<CarAxle>();

    public Vector2 frontAxlePosition;
    public Vector2 rearAxlePosition;

    public float minTurnAmount;
    public float turnScaling;
    public float turnRadius;

    public Vector3 centerOfMass;

    
    public float steerTime;
    public float driveForce;
    public float grip;
    public float driftGrip;
    public float steeringDriftGrip;
    public AnimationCurve gripCurve;
    public AnimationCurve torqueCurve;

    [Header("Inputs")]
    public float steerInput;
    public float driftThreshold;
    public float manualDriftThreshold;

    private float ackermannAngleLeft;
    private float ackermannAngleRight;

    bool isDrifting = false;

    void Start()
    {
        carRb.centerOfMass = centerOfMass;

        float frontAxle = carAxles[0].axlePosition.x;
        float backAxle = carAxles[0].axlePosition.x;
        foreach (CarAxle axle in carAxles)
        {
            if (axle.axlePosition.x > frontAxle)
            {
                frontAxle = axle.axlePosition.x;
            }
            if ((axle.axlePosition.x < backAxle))
            {
                backAxle = axle.axlePosition.x;
                rearTrack = axle.axleTrack;
            }

            GameObject leftWheel = Instantiate(axle.leftWheel, transform.position + Vector3.forward * axle.axlePosition.x + Vector3.up * axle.axlePosition.y + new Vector3(-axle.axleTrack / 2, 0, 0), Quaternion.identity, this.transform);
            leftWheel.transform.localPosition = Vector3.forward * axle.axlePosition.x + Vector3.up * axle.axlePosition.y + new Vector3(-axle.axleTrack / 2, 0, 0);
            GameObject rightWheel = Instantiate(axle.rightWheel, transform.position + Vector3.forward * axle.axlePosition.x + Vector3.up * axle.axlePosition.y + new Vector3(+axle.axleTrack / 2, 0, 0), Quaternion.identity, this.transform);
            rightWheel.transform.localPosition = Vector3.forward * axle.axlePosition.x + Vector3.up * axle.axlePosition.y + new Vector3(+axle.axleTrack / 2, 0, 0);

            leftWheel.GetComponent<WheelSuspension>().rb = carRb;
            leftWheel.GetComponent<WheelSuspension>().carController = this;
            wheels.Add(leftWheel.GetComponent<WheelSuspension>());
            wheels.Add(rightWheel.GetComponent<WheelSuspension>());
            rightWheel.GetComponent<WheelSuspension>().rb = carRb;
            rightWheel.GetComponent<WheelSuspension>().carController = this;

            if (axle.steering == true)
            {
                leftSteeringWheel = leftWheel.GetComponent<WheelSuspension>();
                rightSteeringWheel = rightWheel.GetComponent<WheelSuspension>();
                leftWheel.GetComponent<WheelSuspension>().isSteering = true;
                rightWheel.GetComponent<WheelSuspension>().isSteering = true;
            }
        }
        wheelBase = frontAxle - backAxle;

        float toalSlipRatio = 0;
        foreach (WheelSuspension wheel in wheels)
        {
            if (wheel.isGrounded == true)
            {
                isGrounded = true;
            }
            toalSlipRatio += wheel.slipRatio;
        }
        slipRatio = toalSlipRatio / wheels.Count;
    }

    void FixedUpdate()
    {
        steerInput = Input.GetAxis("Horizontal");

        isGrounded = false;
        float toalSlipRatio = 0;
        foreach(WheelSuspension wheel in wheels)
        {
            if(wheel.isGrounded == true)
            {
                isGrounded = true;
            }
            toalSlipRatio += wheel.slipRatio;
        }
        slipRatio = toalSlipRatio / wheels.Count;

        turnRadius = Mathf.Clamp((transform.InverseTransformDirection(carRb.velocity).z * turnScaling), minTurnAmount, Mathf.Infinity);
        
        if (steerInput > 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
        }
        else if (steerInput < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
        }
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        leftSteeringWheel.steerAngle = ackermannAngleLeft;
        rightSteeringWheel.steerAngle = ackermannAngleRight;

        
        if(!isGrounded)
        {
            currentAirControl += airControlDelay * Time.deltaTime;
            carRb.AddTorque(transform.up * steerInput * airControl, ForceMode.Acceleration);
            carRb.AddTorque(transform.right * -Input.GetAxis("AirControl") * airControl, ForceMode.Acceleration);
            carRb.AddTorque(transform.forward * -Input.GetAxis("AirSpin") * airControl, ForceMode.Acceleration);
        }
        else
        {
            currentAirControl = 0f;
        }

        
        if (Input.GetKey(KeyCode.Space) && slipRatio <= manualDriftThreshold)
        {
            isDrifting = true;
        }
        if (slipRatio >= manualDriftThreshold)
        {
            isDrifting = false;
        }

        if (isDrifting)
        {
            carRb.AddTorque(transform.up * steerInput * driftBonus, ForceMode.Acceleration);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(centerOfMass, new Vector3(rearTrack, 1f, wheelBase) * (carRb.mass / 5000f));

        
        foreach (CarAxle axle in carAxles)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(centerOfMass, centerOfMass + Vector3.forward * axle.axlePosition.x);
            Gizmos.DrawLine(centerOfMass + Vector3.forward * axle.axlePosition.x, Vector3.forward * axle.axlePosition.x + Vector3.up * axle.axlePosition.y);
            Gizmos.DrawLine(Vector3.forward * axle.axlePosition.x + Vector3.up * axle.axlePosition.y + new Vector3(-axle.axleTrack/2, 0, 0), Vector3.forward * axle.axlePosition.x + Vector3.up * axle.axlePosition.y + new Vector3(+axle.axleTrack / 2, 0, 0));
        }
    }

    
}
