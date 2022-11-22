using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterBehaviour : BulletBehaviour
{
    [SerializeField] GameObject visualPrefab;
    [SerializeField] float thrustForce;

    public override void BulletStart()
    {
        //GameObject newVisual = Instantiate(visualPrefab, target.transform.position + transform.forward * 5, target.transform.rotation);
        //newVisual.transform.parent = this.transform;
    }
    
    public override void BulletHit()
    {
        throw new System.NotImplementedException();
    }

    public override void BulletUpdate()
    {
        throw new System.NotImplementedException();
    }

    void FixedUpdate()
    {
        if(target != null)
        {
            target.rb.AddForce(transform.up * thrustForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}
