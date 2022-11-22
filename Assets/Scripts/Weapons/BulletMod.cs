using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMod : MonoBehaviour
{
    public Vector2 damage;
    public Vector2 accuracy;
    public Vector2 muzzleVelocity;
    public Vector2 fireRate;
    public Vector2 rifling;
    public int ammo;

    public List<Transform> behaviours = new List<Transform>();

    public bool hasCore;
}
