using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    protected BulletDriver target;

    public void SetTarget(BulletDriver newTarget)
    {
        target = newTarget;
    }

    public abstract void BulletStart();

    public abstract void BulletUpdate();

    public abstract void BulletHit();
}
