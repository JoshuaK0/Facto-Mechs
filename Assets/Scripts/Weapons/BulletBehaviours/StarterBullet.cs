using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterBullet : BulletBehaviour
{
    [SerializeField] float velocity;

    [SerializeField] GameObject visualPrefab;

    public override void BulletStart()
    {
        GameObject newVisual = Instantiate(visualPrefab, target.transform.position + transform.forward * 5, target.transform.rotation);
        newVisual.transform.parent = this.transform;
        target.rb.useGravity = false;
        target.rb.AddForce(transform.forward * velocity, ForceMode.VelocityChange);
    }
    
    public override void BulletHit()
    {
        throw new System.NotImplementedException();
    }

    public override void BulletUpdate()
    {
        throw new System.NotImplementedException();
    }
}
