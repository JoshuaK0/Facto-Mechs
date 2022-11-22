using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : InteractableBlock
{
    [SerializeField]
    BulletMod weaponMod;

    [SerializeField]
    BulletMod bulletMod;

    [SerializeField] BulletMod combinedMod;

    [SerializeField]
    List<Transform> bulletBehaviours = new List<Transform>();

    Transform muzzle;

    [SerializeField] int ammo = 0;

    float lastFireTime;

    void Update()
    {
        if (ammo <= 0)
        {
            GetNewBullets();
        }
        else if (Input.GetMouseButton(0))
        {
            if(CanFire())
            {
                ammo--;
                GameObject newBullet = new GameObject();
                newBullet.transform.parent = null;
                newBullet.transform.rotation = transform.rotation;
                newBullet.transform.position = muzzle.position + muzzle.forward * 3f;
                BulletDriver bulletDriver = newBullet.AddComponent<BulletDriver>();

                List<BulletBehaviour> behaviourList = new List<BulletBehaviour>();

                foreach (Transform t in weaponMod.behaviours)
                {
                    GameObject newBehaviour = (GameObject)Instantiate(t.gameObject, transform.position, transform.rotation);
                    newBehaviour.transform.parent = bulletDriver.transform;
                    behaviourList.Add(newBehaviour.GetComponent<BulletBehaviour>());
                }

                foreach (Transform t in bulletBehaviours)
                {
                    GameObject newBehaviour = (GameObject)Instantiate(t.gameObject, transform.position, transform.rotation);
                    newBehaviour.transform.parent = bulletDriver.transform;
                    behaviourList.Add(newBehaviour.GetComponent<BulletBehaviour>());
                }
                bulletDriver.SetMod(ModCombiner.CombineTwoMods(bulletDriver.gameObject.AddComponent<BulletMod>(), combinedMod));
                bulletDriver.SetBehaviours(behaviourList);
                bulletDriver.BulletDriverStart();
            }
        }
    }

    bool CanFire()
    {
        if(Time.time > lastFireTime + combinedMod.fireRate.x)
        {
            lastFireTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    void GetNewBullets()
    {
        foreach(Transform t in bulletBehaviours)
        {
            Destroy(t.gameObject);
        }
        bulletBehaviours.Clear();

        InteractableBlock backAmmo = GetInteractable(-transform.forward);
        if (backAmmo != null)
        {
            MovablePart backMovable = backAmmo.GetComponent<MovablePart>();
            if (backMovable != null)
            {
                bulletMod = backMovable.GetRoot().bulletMod;
                ammo = bulletMod.ammo;

                List<Transform> newBehaviours = bulletMod.behaviours;
                foreach (Transform t in bulletMod.behaviours)
                {
                    GameObject newBehaviour = (GameObject)Instantiate(t.gameObject, transform.position, transform.rotation);
                    newBehaviour.transform.parent = this.transform ;
                    bulletBehaviours.Add(newBehaviour.transform);
                }
                if(combinedMod != null)
                {
                    Destroy(combinedMod);
                }
                
                combinedMod = gameObject.AddComponent<BulletMod>();
                combinedMod = ModCombiner.CombineTwoMods(combinedMod, bulletMod);
                combinedMod = ModCombiner.CombineTwoMods(combinedMod, weaponMod);
                backMovable.GetRoot().DestroyBlock();
            }
        }
    }
    public void SetMod(BulletMod newMod)
    {
        weaponMod = newMod;
    }

    public void SetMuzzle(Transform newMuzzle)
    {
        muzzle = newMuzzle;
    }
}
