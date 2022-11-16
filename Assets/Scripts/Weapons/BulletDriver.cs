using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDriver : MonoBehaviour
{
    [SerializeField]
    List<BulletBehaviour> behaviours;
    public Rigidbody rb;
    public void BulletDriverStart()
    {
        InitBullet();
        foreach(BulletBehaviour b in behaviours)
        {
            b.SetTarget(this);
            b.BulletStart();
        }  
    }
    public void SetBehaviours(List<BulletBehaviour> newBehaviours)
    {
        behaviours = newBehaviours;
    }

    void InitBullet()
    {
        rb = gameObject.AddComponent<Rigidbody>();
    }
}
