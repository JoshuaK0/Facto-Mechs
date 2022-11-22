using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDriver : MonoBehaviour
{
    [SerializeField]
    List<BulletBehaviour> behaviours;
    public Rigidbody rb;

    [SerializeField] BulletMod mods;
    public void BulletDriverStart()
    {
        StandardBulletBehaviour();
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

    public void SetMod(BulletMod newMod)
    {
        mods = newMod;
    }

    void InitBullet()
    {
        rb = gameObject.AddComponent<Rigidbody>();
    }

    void StandardBulletBehaviour()
    {

    }
}
