using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystemManager : TickBlock
{
    static WeaponSystemManager instance;

    WeaponBlock[] weapons;

    public static WeaponSystemManager Instance()
    {
        return instance;
    }

    public override void DoInitTick()
    {
        weapons = null;
        weapons = FindObjectsOfType<WeaponBlock>();

        foreach (WeaponBlock weapon in weapons)
        {
            weapon.LinkGuns();
        }
        foreach (WeaponBlock weapon in weapons)
        {
            weapon.PrimeGuns();
        }
        Debug.Log("Weapon init");
    }

    public override void DoTick()
    {
        return;
    }
}
