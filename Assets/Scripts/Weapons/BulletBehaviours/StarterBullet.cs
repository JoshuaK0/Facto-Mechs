using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterBullet : BulletBehaviour
{
    [SerializeField] GameObject visualPrefab;

    public override void BulletStart()
    {
        GameObject newVisual = Instantiate(visualPrefab);
        newVisual.transform.parent = target.transform;
        newVisual.transform.localPosition = Vector3.zero;
        newVisual.transform.localRotation = Quaternion.identity;
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
