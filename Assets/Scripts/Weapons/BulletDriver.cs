using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletDriver : MonoBehaviour
{
    [SerializeField]
    List<BulletBehaviour> behaviours;
    public Rigidbody rb;

    [SerializeField] BulletMod mods;
    public void BulletDriverStart()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        StandardBulletBehaviour();
        foreach (BulletBehaviour b in behaviours)
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

    void StandardBulletBehaviour()
    {
        rb.useGravity = false;
        transform.RotateAround(transform.position, transform.up, Random.Range(-mods.accuracy.x, mods.accuracy.x));
        transform.RotateAround(transform.position, transform.right, Random.Range(-mods.accuracy.x, mods.accuracy.x));
        transform.RotateAround(transform.position, transform.forward, Random.Range(1, 360));
        rb.AddForce(transform.forward * mods.muzzleVelocity.x, ForceMode.VelocityChange);
    }

    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, mods.rifling.x * Time.deltaTime);
    }
}
