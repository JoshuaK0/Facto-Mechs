using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlock : InteractableBlock
{
    [SerializeField]
    WeaponBlock frontWeapon;
    [SerializeField]
    WeaponBlock backWeapon;
    public WeaponBlock muzzle;

    [SerializeField]
    List<WeaponBlock> weaponChain = new List<WeaponBlock>();

    [SerializeField]
    bool isPrimer;

    public BulletMod weaponMod;
    public BulletMod weaponChainMods;

    [SerializeField]
    WeaponSystem weaponSystem;

    [SerializeField]
    bool isWeaponBase;

    public void LinkGuns()
    {
        InteractableBlock frontBlock = GetInteractable(transform.forward);
        InteractableBlock backBlock = GetInteractable(-transform.forward);

        if(frontBlock != null)
        {
            frontWeapon = frontBlock.GetComponent<WeaponBlock>();
        }
        if(backBlock != null)
        {
            backWeapon = backBlock.GetComponent<WeaponBlock>();
        }
    }

    public void PrimeGuns()
    {
        if(!isWeaponBase)
        {
            return;
        }
        GetPrimer();
        if(isPrimer)
        {
            weaponChainMods = gameObject.AddComponent<BulletMod>();
            GetWeaponChain(this);
            CalculateStats();
            weaponSystem = gameObject.AddComponent<WeaponSystem>();

            weaponSystem.SetMuzzle(muzzle.transform);
        }
        if(weaponSystem != null && weaponChainMods != null)
        {
            weaponSystem.SetMod(weaponChainMods);
        }
        
    }

    public WeaponBlock GetPrimer()
    {
        if (backWeapon == null)
        {
            isPrimer = true;
            return this;
        }
        else
        {
            isPrimer = false;
            return (backWeapon.GetPrimer());
        }
    }

    public void GetWeaponChain(WeaponBlock primer)
    {
        primer.AddBlockToWeaponChain(this);
        if(frontWeapon != null)
        {
            frontWeapon.GetWeaponChain(primer);
        }
        else
        {
            primer.muzzle = this;
        }
    }

    public void AddBlockToWeaponChain(WeaponBlock newBlock)
    {
        weaponChain.Add(newBlock);
    }

    void CalculateStats()
    {
        List<BulletMod> modList = new List<BulletMod>();
        foreach(WeaponBlock weaponBlock in weaponChain)
        {
            modList.Add(weaponBlock.weaponMod);
        }
        weaponChainMods = ModCombiner.CombineModList(modList, weaponChainMods);
    }
}
