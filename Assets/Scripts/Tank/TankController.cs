using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] Transform tankBody;
    [SerializeField] Transform tankTurret;
    [SerializeField] Transform turretCamera;
    [SerializeField] bool affectCamera;

    void Update()
    {
        DoTurn();
        DoMove();
    }

    void DoTurn()
    {
        float rot = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        tankBody.localEulerAngles +=new Vector3(0, rot, 0);

        if(affectCamera)
        {
            turretCamera.localEulerAngles += new Vector3(0, rot, 0);
            tankTurret.localEulerAngles += new Vector3(0, rot, 0);
        }
    }

    void DoMove()
    {
        float moveAmount = maxSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        transform.position += (tankBody.forward * moveAmount);
    }
}
